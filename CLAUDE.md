# Forma de atuar

Atue como um arquiteto e engenheiro de software experiente.

Não implemente apenas o que foi solicitado.

Antes de escrever código:

1. Analise a arquitetura existente.
2. Verifique se a solicitação viola alguma regra arquitetural.
3. Identifique alternativas melhores quando existirem.
4. Explique os trade-offs relevantes.
5. Somente depois proponha a implementação.

Sempre priorize:

- legibilidade
- simplicidade
- baixo acoplamento
- alta coesão
- testabilidade
- manutenção
- extensibilidade
- não duplicação de código

# Arquitetura e Padrões de Projeto (Core)

Base de conhecimento dos padrões arquiteturais e táticos usados no back-end desta solução (.NET / C#, PostgreSQL + EF Core). Este documento descreve **como o código está organizado hoje** e serve como referência para manter consistência ao evoluir a base.

> Idioma do domínio: mensagens de negócio e erros são em **português**; nomes de tipos, métodos e namespaces em **inglês**.

---

## 1. Visão geral: Clean Architecture + DDD

A solução (`Core.sln`) é dividida em quatro projetos, seguindo a **Regra da Dependência**: o código-fonte só depende para *dentro*, em direção ao domínio. Nada no núcleo conhece detalhes de infraestrutura ou de apresentação.

```
┌─────────────────────────────────────────────────────────────┐
│  Presentation  (Presentation/Web/APIs/REST)                  │
│  Controllers, DI, Middleware, Validações de entrada          │
│      │ depende de ▼                                           │
├─────────────────────────────────────────────────────────────┤
│  Application                                                  │
│  Services (casos de uso), DTOs, Queries (interfaces),        │
│  Mappings (AutoMapper), interfaces de Services/Security      │
│      │ depende de ▼                                           │
├─────────────────────────────────────────────────────────────┤
│  Domain   ◄──────────── NÚCLEO, sem dependências externas     │
│  Entities, ValueObjects, Interfaces (Repositories, UoW),     │
│  Exceptions, Validators, Enums                               │
├─────────────────────────────────────────────────────────────┤
│  Infrastructure  (implementa interfaces do Domain/Application)│
│  EF Core (DbContext, Mappings), Repositories, Queries,       │
│  Security (JWT, hashing), Email (SMTP)                        │
└─────────────────────────────────────────────────────────────┘
```

Regra prática de dependências:

| Projeto          | Pode referenciar                        |
| ---------------- | --------------------------------------- |
| `Domain`         | *(nada)* — núcleo puro                   |
| `Application`    | `Domain`                                |
| `Infrastructure` | `Domain`, `Application`                  |
| `Presentation`   | `Application`, `Infrastructure`, `Domain` (apenas na composição de DI) |

A **inversão de dependência** é o que permite `Infrastructure` implementar contratos definidos em `Domain`/`Application` sem que o núcleo saiba disso. Toda a ligação concreta acontece em um só lugar: as *extensions* de DI na camada de apresentação (ver §9).

---

## 2. Domain — o coração do modelo

### 2.1 Entidades e a raiz `Entity`

Toda entidade herda de `Entity` (`Domain/Entities/Entity.cs`), que define a identidade (`Id`) e uma `Description` abstrata:

```csharp
public abstract class Entity
{
    public long Id { get; set; }
    public abstract string Description { get; protected set; }
}
```

Entidades sem descrição própria (ex.: documentos, onde a "descrição" é derivada) usam `EntityWithoutDescription`, que gera a `Description` a partir de `GetDescription()`.

Princípios de modelagem de entidade aplicados aqui:

- **Encapsulamento forte**: setters são `private`/`private init`. O estado só muda por métodos de negócio (`Fill(...)`, `SetXxx(...)`), nunca por atribuição externa direta.
- **Construtor de ORM protegido**: cada entidade tem um construtor sem parâmetros marcado `// Constructor only for ORM.` usado apenas pelo EF Core na materialização.
- **Invariantes validadas na construção/alteração**: o objeto nunca existe em estado inválido — a validação roda dentro do próprio método de negócio.
- **Coleções expostas como somente-leitura**: coleções filhas ficam em campo privado (`List<T>`) e são expostas como `IReadOnlyCollection<T>`.

Exemplo (aggregate root `Document<TProduct>` em `Domain/Entities/Commercial/Documents/Document.cs`):

```csharp
private readonly List<TProduct> _products = new();
public IReadOnlyCollection<TProduct> Products => _products;

public void Fill(long userId, long customerId, Payment payment, /* ... */ IReadOnlyCollection<TProduct> products)
{
    LastUpdateDateTime = DateTime.UtcNow;
    SetUserId(userId);          // valida internamente
    SetCustomerId(customerId);
    _payment = payment;

    if (products == null || !products.Any())
        throw new BusinessRuleException($"O Orçamento: {this.Description} deve possuir ao menos um produto.");

    EntityCollection.Sync(_products, products, Id);   // reconcilia filhos
    SetValues();                                       // recalcula totais
    ValidateInstallment();                             // invariantes de negócio
}
```

### 2.2 Aggregates e entidades filhas

- `Document<TProduct>` é uma **raiz de agregado** (aggregate root): controla seus produtos, o pagamento e os totais. As entidades concretas `Budget` (orçamento) e `Order` (pedido) apenas especializam a raiz genérica.
- Entidades filhas implementam `IEntityChild` (`SetFatherEntityId(long)`) e `ICopyable<T>` (`Copy(T)`), permitindo que a raiz reconcilie sua coleção sem que a camada de aplicação manipule filhos diretamente.
- A reconciliação de coleções é centralizada em `Domain/Utilities/Entities/EntityCollection.Sync(...)`: remove os que saíram, copia sobre os existentes (por `Id`) e adiciona os novos (com o `Id` do pai). Esse é o padrão para atualizar coleções de agregados sem `clear + re-add` (que quebraria o tracking do EF).

### 2.3 Value Objects

Objetos sem identidade própria, definidos por seus valores, ficam em `Domain/ValueObjects`. Ex.: `Payment`, `Installment`, `DownPayment`. Características:

- Imutáveis (`private init`), sem `Id`.
- Validam suas próprias invariantes no construtor.
- Construtor privado sem argumentos para o ORM (mapeados como *owned types* — ver §7).

```csharp
public class Payment
{
    public PaymentMethod Method { get; private init; }
    public decimal DiscountValue { get; private init; }
    public Installment? Installment { get; private init; }

    public Payment(PaymentMethod method, decimal discountValue, Installment? installment)
    {
        Validator.ValidateEnum(method, "O método de pagamento");
        Method = method; DiscountValue = discountValue; Installment = installment;
        HasInstallment = installment != null;
    }
}
```

### 2.4 Validação de domínio

`Domain/Utilities/Entities/Validator.cs` é um utilitário `internal static` com guard clauses genéricas (`ValidateValue`, `ValidateEnum`, `ValidateEmail`, `ValidateCpfCnpj`, ...). Ele reforça invariantes **dentro** do domínio e lança `BusinessRuleException` com mensagem em português. Não confundir com a validação de *entrada* (FluentValidation na apresentação — §8): esta protege as regras de negócio; aquela protege o formato da requisição.

### 2.5 Exceções de domínio

Duas exceções tipadas comunicam falhas de negócio para fora do núcleo:

- `BusinessRuleException` → violação de regra de negócio (vira `400 Bad Request`).
- `NotFoundException` → registro inexistente (vira `404 Not Found`).

O tratamento e a tradução para HTTP acontecem na camada de apresentação (§6).

---

## 3. Padrão Repository

Contratos em `Domain/Interfaces/Repositories`, implementações em `Infrastructure/Repositories`.

Hierarquia dos contratos:

```
IRepository<TEntity>            // marcador base
   └── IRepositoryCrud<TEntity> // operações CRUD genéricas
          └── ILocalityRepository, ICustomerRepository, ... // específicos
```

- `IRepositoryCrud<TEntity>` (`Domain/Interfaces/Repositories/IRepositoryCrud.cs`) define o contrato genérico: `SaveAsync`, `GetByIdsAsync`, `GetIdsAsync`, `ExistsByIdAsync`, `DeleteAsync`.
- A implementação genérica `RepositoryCrud<TEntity>` (`Infrastructure/Repositories/RepositoryCrud.cs`) resolve tudo via `DbSet<TEntity>`. Pontos de destaque:
  - `SaveAsync` decide *insert vs. update* pelo `Id` (`Id == 0` → `AddRange`; `Id > 0` → `UpdateRange`). **Não** chama `SaveChanges` — isso é responsabilidade do Unit of Work.
  - `DeleteAsync` usa `ExecuteSqlRawAsync` com `DELETE ... = ANY(@ids)` (delete em lote via SQL, sem carregar entidades).
  - `ApplyIncludesForGetByIds` é um *hook* `virtual` para repositórios concretos adicionarem `Include` de agregados (ex.: carregar produtos junto do documento).

Repositórios concretos herdam a base genérica e adicionam apenas o que é específico:

```csharp
public class LocalityRepository : RepositoryCrud<Locality>, ILocalityRepository
{
    // apenas comportamento específico de Locality (ex.: GetQuantityAsync)
}
```

> **Repository escreve, Query lê.** O repositório é usado para carregar agregados e persistir mudanças; a leitura otimizada para telas/relatórios é feita pelas Queries (§5).

---

## 4. Unit of Work

Contrato em `Domain/Interfaces/Data/IUnitOfWork.cs`, implementação em `Infrastructure/Persistence/UnitOfWork.cs`. Envolve a transação e o `SaveChanges` do `DbContext`:

- `BeginTransactionAsync` — abre a transação (idempotente: `??=`).
- `FlushAsync` — `SaveChangesAsync` (persiste sem fechar a transação).
- `CommitAsync` — dá flush e faz commit, depois descarta a transação.
- `RollbackAsync` — desfaz e descarta.
- `IsTransactionBegun` — permite compor operações.

O padrão de uso nos services é sempre:

```csharp
await _unitOfWork.BeginTransactionAsync(ct);
await _repositoryCrud.SaveAsync(entities, ct);
await _unitOfWork.CommitAsync(ct);
```

O `UnitOfWork` e o `AppDbContext` são registrados com tempo de vida **Scoped** (um por requisição HTTP), de modo que todos os repositórios/queries de uma mesma requisição compartilham a mesma transação e o mesmo *change tracker*.

---

## 5. CQRS "leve": separação Command/Query

O projeto **não** usa MediatR nem um barramento de comandos, mas aplica a ideia central do CQRS: **caminhos de escrita e de leitura são separados**.

| Caminho   | Contrato (Application)                | Implementação (Infrastructure) | Retorna                 |
| --------- | ------------------------------------- | ------------------------------ | ----------------------- |
| Escrita   | `IRepositoryCrud<TEntity>`            | `RepositoryCrud<TEntity>`      | Entidades de domínio    |
| Leitura   | `I...Query` / `IEntityCrudQuery<...>` | `...Query : DetailDtoQuery`    | **DTOs** (nunca entidades) |

Queries (`Infrastructure/Queries`) leem direto do `DbContext` com `AsNoTracking()` e projetam para DTOs — sem materializar o agregado inteiro. A base `DetailDtoQuery<TEntity, TDetailDto>` (`Infrastructure/Queries/DetailDtoQuery.cs`) provê:

- `_query = _context.Set<TEntity>().AsNoTracking()` — leitura sem tracking (mais rápida, sem efeitos colaterais).
- `GetDetailAsync(id)` — busca + `Include`s (hook `ApplyIncludes`) + projeção via **AutoMapper**.
- `ApplyPaging(query, filter)` — paginação padrão (`OrderByDescending(Id)` + `Skip/Take`).

Listagens fazem projeção explícita para o DTO de listagem (`new XxxListing { ... }`), evitando *over-fetching*. Filtros são objetos (`XxxListingFilter`) aplicados condicionalmente:

```csharp
if (filter.Description != null)
    query = query.Where(l => EF.Functions.ILike(l.Description, filter.Description));
```

**Regra de ouro:** uma Query nunca retorna uma `Entity`; sempre um DTO. Isso mantém o modelo de domínio isolado do contrato de leitura da API.

---

## 6. Application — casos de uso (Services)

Os *services* orquestram o caso de uso: validam entrada, transformam DTO ↔ entidade, coordenam repository + query dentro de um Unit of Work. Contratos em `Application/Interfaces/Services`, implementações em `Application/Services`.

### 6.1 `CrudService` genérico (Template Method)

`Application/Services/CrudService.cs` implementa o fluxo CRUD completo uma única vez e delega as partes específicas para métodos abstratos — clássico **Template Method**:

```csharp
public abstract class CrudService<TEntity, TSaveDto, TDetailDto, TListing, TFilter>
    : ICrudService<TSaveDto, TDetailDto, TListing, TFilter>
{
    // fluxo pronto: CreateAsync, UpdateAsync, DeleteAsync, GetByIdAsync, GetAllAsync

    protected abstract TEntity CreateEntity(TSaveDto dto);              // "como criar"
    protected abstract void UpdateEntity(TSaveDto dto, TEntity entity); // "como atualizar"
    protected abstract Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids);
    protected virtual Task SaveAsync(List<TEntity> entities, ...);      // gancho p/ estender
}
```

Responsabilidades já resolvidas pela base:
- Validação de que há DTOs (`ValidateDtos`).
- `TransformToEntity`: carrega os existentes por `Id` (update) e cria os novos (insert) numa só passagem.
- Verificação de existência de IDs antes de deletar (`ValidateIds` → `NotFoundException`).
- Abertura/commit do Unit of Work.

Um service concreto declara os tipos e implementa só o específico:

```csharp
public class LocalityService
    : CrudService<Locality, LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>,
      ILocalityService
{
    protected override Locality CreateEntity(LocalitySaveDto dto) => /* ... */;
    protected override void UpdateEntity(LocalitySaveDto dto, Locality e) => e.Fill(...);
    protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids) => Task.FromResult(false);
}
```

### 6.2 Especialização por herança de service

Quando um grupo de entidades compartilha comportamento, cria-se um service intermediário. Ex.: `DocumentService<...>` herda de `CrudService<...>` e adiciona regras comuns a orçamentos e pedidos (montagem de `Payment` a partir do DTO, envio de e-mail com o PDF ao cliente após salvar, `IServiceScopeFactory` para trabalho em background). `BudgetService` e `OrderService` apenas especializam `DocumentService`.

Sobrescrever `SaveAsync` (que é `virtual`) é o gancho padrão para acrescentar efeitos ao salvar sem reescrever o fluxo — ver `DocumentService.SaveAsync`.

### 6.3 DTOs

Em `Application/DataTransferObjects`, com sufixos convencionais por finalidade:

| Sufixo         | Finalidade                                        |
| -------------- | ------------------------------------------------- |
| `...SaveDto`   | Entrada de create/update (herda `EntitySaveDto`)  |
| `...DetailDto` | Saída de detalhe (GET por id)                     |
| `...Listing`   | Item de listagem (herda `EntityListing`)          |
| `...ListingFilter` | Filtros de listagem/paginação (herda `ListingFilter`) |
| `...Report` / `...ReportFilter` / `...ReportSummary` | Relatórios |

DTOs cruzam a fronteira da API; **entidades de domínio nunca são expostas** diretamente.

### 6.4 Mapeamento com AutoMapper (Profiles)

Cada mapeamento é um `Profile` em `Application/Mappings` (ex.: `BudgetDetailProfile`). Profiles genéricos reaproveitam a configuração entre tipos análogos (ex.: `DocumentDetailProfile<TEntity, TProduct, TDto>`). Todos são registrados por varredura de assembly (`AddAutoMapper(..., typeof(CustomerProfile).Assembly)`), então basta criar o Profile — não é preciso registrá-lo manualmente.

---

## 7. Persistência com EF Core

- **DbContext**: `Infrastructure/Persistence/AppDbContext.cs`. `OnModelCreating` faz `ApplyConfigurationsFromAssembly(...)`, então cada entidade tem sua configuração descoberta automaticamente.
- **Mapeamentos (Fluent API)**: em `Infrastructure/Persistence/Mappings`, um arquivo por entidade implementando `IEntityTypeConfiguration<T>`. Há uma base `EntityMapping<T>` que mapeia `Id`/`Description` e expõe `GetTableName()` abstrato — mantendo as convenções (nomes de coluna em snake_case, chave primária etc.) num só lugar.
- **Owned types**: value objects como `Payment`/`Installment` são configurados como *owned entities* do agregado dono.
- **Migrations**: em `Infrastructure/Persistence/Migrations`, nomeadas por ticket (`dev_7`, `dev_9`, ...). Há um `AppDbContextFactory` (design-time factory) para gerar/aplicar migrations pela CLI.
- **Banco**: PostgreSQL (Npgsql). Buscas textuais case-insensitive usam `EF.Functions.ILike`. Deletes em lote usam SQL parametrizado com `= ANY(@ids)`.

> Convenção herdada: o namespace de persistência é `Infrastructure.Persistece` (grafia mantida por consistência com o restante do código existente).

---

## 8. Presentation — API REST

### 8.1 Controllers em camadas

- `Controller` (base, `[Authorize]`) — centraliza logging, o Unit of Work e o **tratamento de exceções**.
- `CrudController<TSaveDto, TDetailDto, TListing, TFilter>` — expõe os endpoints REST padrão (`GET {id}`, `POST`, `PUT {id}`, `DELETE {id}`, `GET` de listagem abstrato) delegando ao `ICrudService` correspondente. Controllers concretos herdam e, no geral, só precisam implementar `GetAll`.

```csharp
[HttpPost]
public async Task<ActionResult<TSaveDto>> Create([FromBody] TSaveDto dto, CancellationToken ct)
    => await ProcessRequestAsync<TSaveDto>(async () =>
    {
        Entity entity = (await _service.CreateAsync([dto], ct)).First();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }, ct);
```

### 8.2 Tratamento de erros centralizado

`Controller.ProcessRequestAsync(...)` envolve a ação num `try/catch` que traduz exceções de domínio em respostas HTTP e faz `rollback` do Unit of Work:

| Exceção                        | Resposta HTTP                    |
| ------------------------------ | -------------------------------- |
| `BusinessRuleException`        | `400 Bad Request` (com mensagem) |
| `NotFoundException`            | `404 Not Found`                  |
| `UnauthorizedAccessException`  | `401 Unauthorized`               |
| `PostgresException` FK (23001) | `409 Conflict` (mensagem amigável) |
| Qualquer outra                 | `500` (Problem, sem vazar detalhe) |

Também desembrulha `InnerException` (ex.: exceções do EF que embrulham a de negócio). Isso mantém os métodos de ação limpos — nada de `try/catch` espalhado.

### 8.3 Validação de entrada (FluentValidation)

`Presentation/.../Validations/FluentValidationAction.cs` é um `IAsyncActionFilter` que, antes de cada ação, procura um `IValidator<T>` registrado para cada argumento e valida automaticamente. Falhas viram `ModelState` → `400` padronizado. Assim, validar um DTO exige apenas criar um `AbstractValidator<TDto>`; nada muda no controller.

> **Divisão de responsabilidades de validação:** FluentValidation valida o *formato/contrato* da requisição (obrigatoriedade, tamanho, ranges); o `Validator` de domínio e os métodos das entidades garantem as *regras de negócio*.

---

## 9. Injeção de Dependência (Composition Root)

O ponto único de ligação entre interfaces e implementações fica na apresentação, organizado por *extension methods* (`Presentation/.../Extensions`) e orquestrado no `Program.cs`:

```csharp
builder.Services.AddApplicationServices();                       // services, queries, AutoMapper
builder.Services.AddInfrastructureServices(builder.Configuration); // DbContext, UoW, repositories, security, email
builder.Services.AddAuthenticationConfiguration(...);
builder.Services.AddCorsConfiguration(...);
builder.Services.AddSwaggerConfiguration();
```

Cada extension agrupa registros por área (`AddCustomerServices`, `AddBudgetServices`, ...), mantendo o `Program.cs` enxuto. Convenções:

- Tempo de vida padrão: **Scoped** (por requisição) — services, queries, repositories, `IUnitOfWork`, `AppDbContext`.
- Opções externas via *Options pattern*: `services.Configure<Options>(configuration.GetSection(...))` (JWT, e-mail).
- Ao adicionar uma feature nova, registre o trio **Service + Query + Repository** na extension da sua área.

---

## 10. Segurança

Em `Infrastructure/Security` (contratos em `Application/Interfaces/Security`):

- `IJwtTokenGenerator` / `TokenGenerator` — geração de JWT (config via Options pattern).
- `IPasswordHasherService` / `PasswordHasherService` — hashing de senha.
- `IHttpContextCurrentUser` / `HttpContextCurrentUser` — expõe o usuário autenticado a partir do `HttpContext`, evitando que a camada de aplicação dependa de `Microsoft.AspNetCore.*`.
- Autenticação por refresh token (`RefreshToken` + `RefreshTokenRepository`).

Controllers são protegidos por padrão (`[Authorize]` na base `Controller`); endpoints públicos precisam de `[AllowAnonymous]` explícito.

---

## 11. Convenções e checklist ao adicionar uma feature

Ao criar um novo recurso CRUD (ex.: `Foo`), siga o mesmo caminho das features existentes:

1. **Domain** — `Foo : Entity` (ou o agregado apropriado), com invariantes validadas; se necessário, `IFooRepository : IRepositoryCrud<Foo>`.
2. **Application** — DTOs (`FooSaveDto`, `FooDetailDto`, `FooListing`, `FooListingFilter`); `IFooService : ICrudService<...>` + `FooService : CrudService<...>`; `IFooQuery` + Profile de AutoMapper.
3. **Infrastructure** — `FooMapping : EntityMapping<Foo>`; `FooQuery : DetailDtoQuery<Foo, FooDetailDto>`; `FooRepository : RepositoryCrud<Foo>` (se houver contrato específico); *migration*.
4. **Presentation** — `FoosController : CrudController<...>` (implementa `GetAll`); `FooSaveDtoValidator : AbstractValidator<FooSaveDto>` se precisar validar entrada; registrar Service/Query/Repository na extension de DI.

Convenções gerais:

- **`CancellationToken` em toda operação assíncrona** e propagado até o EF Core.
- **Nunca exponha entidades na API** — sempre DTOs.
- **Não chame `SaveChanges` fora do Unit of Work.**
- **Mensagens de negócio em português**, lançadas como `BusinessRuleException`/`NotFoundException`.
- **Encapsulamento**: mudanças de estado só via métodos de negócio da entidade; setters permanecem privados.
- **Leitura via Query (`AsNoTracking` + projeção); escrita via Repository + UoW.**

---

## 12. Resumo dos padrões aplicados

### 12.1 Padrões GoF (Gang of Four)

Padrões clássicos do catálogo GoF efetivamente presentes no código:

| Padrão (GoF)                  | Categoria    | Onde / como se manifesta                                          |
| ----------------------------- | ------------ | ---------------------------------------------------------------- |
| **Template Method**           | Comportamental | `CrudService` define o fluxo CRUD e delega passos abstratos (`CreateEntity`, `UpdateEntity`, `EntitiesCanBeDeleted`); também em `DocumentService`, `Controller.ProcessRequestAsync`, `DetailDtoQuery.ApplyIncludes/ApplyPaging`, `EntityMapping.Configure`, `EntityWithoutDescription.GetDescription` |
| **Factory Method**            | Criacional   | `CrudService.CreateEntity(TSaveDto)` — subclasses decidem qual entidade instanciar; `AppDbContextFactory` (`IDesignTimeDbContextFactory`); `DocumentService.CreatePayment(...)` (método-fábrica estático); uso de `IServiceScopeFactory` |
| **Prototype (cópia de estado)** | Criacional | `ICopyable<T>.Copy(source)` — reconcilia entidades filhas copiando estado sobre a instância já rastreada em `EntityCollection.Sync` (em vez de recriar) |
| **Adapter**                   | Estrutural   | `HttpContextCurrentUser` adapta `IHttpContextAccessor`/`ClaimsPrincipal` (ASP.NET) para o contrato de domínio `IHttpContextCurrentUser`; AutoMapper `Profile`s adaptam Entidade ↔ DTO |
| **Facade**                    | Estrutural   | *Extension methods* de DI (`AddApplicationServices`, `AddInfrastructureServices`) fecham um subsistema de registros atrás de uma chamada simples; os *Services* de aplicação são fachadas sobre Domain + Repository + Query + UoW |
| **Strategy**                  | Comportamental | Implementações intercambiáveis atrás de interfaces resolvidas por DI (`IEmailService`→`SmtpEmailService`, `IPasswordHasherService`, `IJwtTokenGenerator`); `FluentValidationAction` seleciona em runtime o `IValidator<T>` conforme o tipo do argumento |
| **Chain of Responsibility**   | Comportamental | Pipeline de *middleware*/*action filters* do ASP.NET (`FluentValidationAction` com `Order`); tratamento de exceção em `Controller` que encadeia tipos e cai no `InnerException` como próximo elo |

### 12.2 Padrões arquiteturais e de aplicação (Enterprise / PoEAA)

Padrões que não são do catálogo GoF, mas estruturam a solução:

| Padrão                        | Onde                                                              |
| ----------------------------- | ---------------------------------------------------------------- |
| Clean Architecture (camadas)  | Solução dividida em Domain/Application/Infrastructure/Presentation |
| DDD (Entities, VOs, Aggregates) | `Domain/Entities`, `Domain/ValueObjects`               |
| Aggregate Root                | `Document<TProduct>` controlando produtos/pagamento             |
| Repository                    | `IRepositoryCrud` / `RepositoryCrud<TEntity>`                    |
| Unit of Work                  | `IUnitOfWork` / `UnitOfWork`                                     |
| CQRS                          | Queries (DTO, `AsNoTracking`) vs. Repositories (entidades)       |
| Dependency Injection / Inversion | Extensions de DI (composition root)                          |
| Options Pattern               | Config de JWT e e-mail                                           |
| Guard Clauses                 | `Validator` do domínio                                           |
| DTO                           | `DataTransferObjects` + AutoMapper `Profile`s                    |
| Marker Interface              | `IRepository<TEntity>` (interface vazia usada como restrição/tipo base) |

### 12.3 Padrões ainda não utilizados — diretriz para uso futuro

Os padrões abaixo **não existem hoje** na base de código — de qualquer catálogo (GoF, DDD, PoEAA/Enterprise ou resiliência/cloud). Ficam documentados aqui apenas como referência de *quando* (e só quando) a necessidade real aparecer; não devem ser aplicados por antecipação (YAGNI) — introduzir um padrão sem o gatilho concreto é complexidade acidental. Use esta tabela para reconhecer o sintoma e escolher o padrão certo antes de implementar.

| Padrão                  | Origem                 | Gatilho concreto neste projeto — quando considerar                                                                                                                                                          |
| ------------------------ | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Abstract Factory**     | GoF – Criacional        | Ao adicionar um novo tipo de documento comercial (ex.: Fatura, Nota Fiscal) que exija sempre um conjunto coerente de Service/Query/Repository/Mapping. Hoje a variação é resolvida por generics + herança (`Document<TProduct>`); migre se a quantidade de tipos crescer e a composição por herança ficar rígida demais. |
| **Builder**              | GoF – Criacional        | Quando a criação de um agregado exigir muitos parâmetros opcionais e passos condicionais. O construtor de `Document` já tem ~8 parâmetros; se crescer mais, extraia um `DocumentBuilder` para evitar construtor telescópico e centralizar a ordem de validação. |
| **Singleton**            | GoF – Criacional        | Evite implementar a mão. O contêiner de DI já cobre o caso via `AddSingleton` (ex.: cache em memória, opções imutáveis). Só considere o padrão clássico para utilitário puro, sem estado mutável e sem necessidade de substituição em teste — e mesmo assim prefira DI. |
| **Composite**            | GoF – Estrutural        | Estruturas hierárquicas recursivas de profundidade arbitrária (ex.: árvore de filtros de relatório combináveis com AND/OR). A hierarquia `Territory`/`State`/`Locality` hoje é fixa em 3 níveis via FK simples — não precisa de Composite enquanto isso não mudar. |
| **Decorator**            | GoF – Estrutural        | Comportamento transversal opcional sobre uma implementação existente, sem alterá-la (ex.: `CachingProductRepository : IProductRepository`, `RetryEmailService : IEmailService` envolvendo `SmtpEmailService`). Preferível a herança quando o extra é configurável via DI. |
| **Proxy**                | GoF – Estrutural        | Controlar acesso/custo de algo caro ou remoto (ex.: lazy-loading de anexos grandes como `PDF`/`CustomerSignatureImage`; proxy de circuit breaker para chamadas externas de e-mail/pagamento). |
| **Bridge**               | GoF – Estrutural        | Desacoplar uma abstração de sua implementação quando ambas variam independentemente (ex.: separar "gerar documento" da tecnologia concreta de renderização de PDF, permitindo troca de motor sem tocar em `DocumentService`). |
| **Command**              | GoF – Comportamental    | Encapsular uma operação de negócio como objeto (ex.: `ConverterOrcamentoEmPedidoCommand`, `AprovarOrcamentoCommand`) — relevante se o projeto adotar um *dispatcher*/mediator entre Controller e Service, habilitando fila, retry ou undo. |
| **Mediator**             | GoF – Comportamental    | Se Controllers/Services acumularem muitas dependências cruzadas entre si, considere um mediator (ex.: MediatR) para desacoplar o Controller da orquestração direta entre múltiplos Services. |
| **Observer / Domain Events** | GoF – Comportamental | Reagir a algo que aconteceu no domínio sem acoplar quem gerou o evento a quem reage. Hoje `DocumentService.SaveAsync` dispara e-mail via `Task.Run` diretamente dentro do Service; um `DocumentSavedEvent` publicado pelo agregado e consumido por um handler dedicado desacoplaria a notificação da persistência e eliminaria o fire-and-forget embutido. |
| **State**                | GoF – Comportamental    | Entidades com ciclo de vida e regras por estado (ex.: se `Order`/`Budget` ganharem status como Rascunho/Aprovado/Cancelado/Convertido, modele as transições como State em vez de `if/switch` espalhados pelo Service). |
| **Memento**              | GoF – Comportamental    | Capturar/restaurar um estado anterior de um agregado complexo (ex.: permitir desfazer a edição de um `Budget` antes de confirmar, sem persistir estados intermediários). |
| **Visitor**              | GoF – Comportamental    | Operações que variam por tipo concreto sobre uma hierarquia de entidades sem poluir as classes (ex.: exportação/relatório que trata `Budget` e `Order` de forma distinta) — alternativa a generics se os tipos concretos divergirem muito entre si. |
| **Specification**        | DDD                     | Regra de negócio encapsulada como predicado reutilizável e composável via `And`/`Or`/`Not` (ex.: `OrcamentoPodeSerAprovadoSpecification`). Hoje regras de elegibilidade como `EntitiesCanBeDeleted` e `ValidateInstallment` estão embutidas no Service/Entity; extraia para Specification quando a mesma regra precisar ser reaproveitada em mais de um lugar (ex.: Service, filtro de relatório, validação de UI) sem duplicar `if`s. |
| **Saga / Process Manager** | Enterprise (PoEAA)    | Operação de negócio que atravessa múltiplos agregados ou serviços externos e precisa de compensação em caso de falha parcial (ex.: criar `Order` + reservar estoque + cobrar em gateway de pagamento). Hoje tudo roda dentro de uma única transação via `IUnitOfWork`; migre para Saga quando um dos passos deixar de compartilhar essa transação (chamada externa que não pode ser revertida por `RollbackAsync`). |
| **Outbox Pattern**        | Enterprise / Resiliência | Garantir que um efeito colateral (e-mail, evento) só dispare se a transação de banco realmente confirmou, mesmo em caso de falha do processo entre o commit e o disparo. Hoje `DocumentService` dispara e-mail via `Task.Run` fire-and-forget após o `SaveAsync`; se a confiabilidade dessa notificação se tornar crítica, registre-a como um "evento pendente" na mesma transação e processe-a de forma assíncrona e garantida. |
| **Anti-Corruption Layer** | DDD                    | Ao integrar com um sistema externo (gateway de pagamento, ERP terceiro, serviço de CEP/frete) sem deixar o modelo externo "contaminar" o domínio. Um adapter dedicado traduz o contrato externo para os Value Objects/DTOs internos, mantendo `Domain` livre de conceitos que não são seus. |
| **Null Object**           | GoF (variante)         | Evitar checagens repetidas de `null` para um relacionamento opcional. |
| **Circuit Breaker / Retry** | Resiliência (cloud) | Política de resiliência para chamadas a serviços externos (SMTP, gateway de pagamento) que hoje são chamadas diretas sem retry (`SmtpEmailService`). Considere uma biblioteca de resiliência (ex.: Polly) se falhas externas passarem a impactar a disponibilidade do fluxo principal. |

**Regra de aplicação:** ao identificar um gatilho da tabela durante o desenvolvimento, siga o fluxo da seção "Forma de atuar" no topo deste documento — analise a arquitetura existente, confirme que o gatilho é real (não hipotético) e só então proponha a introdução do padrão, explicando o trade-off frente à alternativa mais simples já em uso.

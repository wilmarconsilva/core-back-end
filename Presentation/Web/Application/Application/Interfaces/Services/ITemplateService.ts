export interface ITemplateService {
  getTemplate(name: string): Promise<string>;
}

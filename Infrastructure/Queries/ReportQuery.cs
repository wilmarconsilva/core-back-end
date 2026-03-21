using Application.Interfaces.Queries;
using Infrastructure.Persistece;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Queries
{
    public abstract class ReportQuery<TReport, TFilter> : AppDbContextAcessor, IReportQuery<TReport, TFilter>
    {
        protected ReportQuery(AppDbContext context) : base(context)
        {
        }

        public abstract Task<List<TReport>> Get(TFilter filter, CancellationToken cancellationToken);

        protected abstract string GetTableName();

        protected DbCommand CreateCommand()
        {
            return _context.Database.GetDbConnection().CreateCommand();
        }

        protected static void AddParameter(DbCommand command, ICollection<DbParameter> parameters, string name, object value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameters.Add(parameter);
        }

        protected static object[] GetParameters(IEnumerable<DbParameter> parameters)
        {
            return parameters.Cast<object>().ToArray();
        }
    }
}

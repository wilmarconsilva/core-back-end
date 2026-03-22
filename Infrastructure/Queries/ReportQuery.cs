using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
using Infrastructure.Persistece;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Queries
{
    public abstract class ReportQuery<TReport, TListingFilter> : AppDbContextAcessor, IReportQuery<TReport, TListingFilter>
        where TReport : class
        where TListingFilter : ListingFilter
    {
        protected const string RegistersStartParameterName = "@registers_start";
        protected const string RegistersEndParameterName = "@registers_end";

        protected ReportQuery(AppDbContext context) : base(context)
        {
        }

        public abstract Task<List<TReport>> GetAsync(TListingFilter filter, CancellationToken cancellationToken);

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

        protected static void AddListingFilterParameters(DbCommand command, ICollection<DbParameter> parameters, ListingFilter filter)
        {
            AddParameter(command, parameters, RegistersStartParameterName, filter.RegistersStart);
            AddParameter(command, parameters, RegistersEndParameterName, filter.RegistersEnd);
        }

        protected static string GetListingFilterSql()
        {
            return $"OFFSET {RegistersStartParameterName}\nLIMIT {RegistersEndParameterName}";
        }

        protected static object[] GetParameters(IEnumerable<DbParameter> parameters)
        {
            return parameters.Cast<object>().ToArray();
        }
    }
}

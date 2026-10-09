using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Documents.Budgets.Queries.Interfaces;
using Application.Documents.Budgets.Services;
using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Reports;
using Application.Documents.Orders.Queries;
using Application.Documents.Orders.Services;

namespace Application.UnitTests.Documents.Services
{
    public class DocumentReportServiceTests
    {
        [Fact]
        public async Task BudgetReportService_Get_SumsValueTotal()
        {
            IBudgetReportQuery query = Substitute.For<IBudgetReportQuery>();
            List<BudgetReport> reports = [new() { ValueTotal = 100 }, new() { ValueTotal = 50.5m }];
            query.GetAsync(Arg.Any<BudgetReportFilter>(), Arg.Any<CancellationToken>()).Returns(reports);

            BudgetReportListing listing = await new BudgetReportService(query).Get(new BudgetReportFilter(), TestContext.Current.CancellationToken);

            listing.Registers.ShouldBeSameAs(reports);
            listing.Summary!.ValueTotal.ShouldBe(150.5m);
        }

        [Fact]
        public async Task OrderReportService_Get_NoRegisters_SumsZero()
        {
            IOrderReportQuery query = Substitute.For<IOrderReportQuery>();
            query.GetAsync(Arg.Any<OrderReportFilter>(), Arg.Any<CancellationToken>()).Returns(new List<OrderReport>());

            OrderReportListing listing = await new OrderReportService(query).Get(new OrderReportFilter(), TestContext.Current.CancellationToken);

            listing.Registers.ShouldBeEmpty();
            listing.Summary!.ValueTotal.ShouldBe(0);
        }

        [Fact]
        public async Task OrderReportService_Get_SumsValueTotal()
        {
            IOrderReportQuery query = Substitute.For<IOrderReportQuery>();
            query.GetAsync(Arg.Any<OrderReportFilter>(), Arg.Any<CancellationToken>()).Returns([new OrderReport { ValueTotal = 10 }, new OrderReport { ValueTotal = 20 }]);

            OrderReportListing listing = await new OrderReportService(query).Get(new OrderReportFilter(), TestContext.Current.CancellationToken);

            listing.Summary!.ValueTotal.ShouldBe(30);
        }
    }
}

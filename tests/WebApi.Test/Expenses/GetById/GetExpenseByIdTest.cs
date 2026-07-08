using CashFlow.Communication.Enums;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest: CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_MemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId);
        response.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().ShouldBeLessThanOrEqualTo(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();

        Enum.IsDefined(typeof(PaymentType), paymentType).ShouldBeTrue();
    }
}

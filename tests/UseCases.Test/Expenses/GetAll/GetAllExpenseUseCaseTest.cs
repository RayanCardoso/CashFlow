using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Expenses.GetAll;

public class GetAllExpenseUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();

        result.Expenses.ShouldNotBeNull();
        result.Expenses.ShouldNotBeEmpty();

        foreach (var expense in result.Expenses)
        {
            expense.Id.ShouldBeGreaterThan(0);
            expense.Title.ShouldNotBeNullOrEmpty();
            expense.Amount.ShouldBeGreaterThan(0);
        }
    }

    private GetAllExpenseUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllExpenseUseCase(repository, mapper, loggedUser);
    }
}

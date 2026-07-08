using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using Shouldly;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(repositoryWriteOnly, repository, unitOfWork, loggedUser);

    }
}

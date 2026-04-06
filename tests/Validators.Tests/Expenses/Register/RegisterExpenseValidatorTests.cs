using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact] 
        public void Sucess()
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ErrorTitleEmpty(string title)
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = title;

            // Act
            var result = validator.Validate(request);

            // Assert 
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.TITLE_REQUIRED)
            );
        }

        [Fact]
        public void ErrorDateFuture()
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Date = DateTime.UtcNow.AddDays(1);

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE)
            );
        }

        [Fact]
        public void ErrorPaymentTypeInvalid ()
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.PaymentType = (PaymentType) 999;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.PAYMENT_TYPE_INVALID)
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-7)]
        public void ErrorAmountInvalid(decimal amount)
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Amount = amount;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO)
            );
        }
    }
}

using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredExpenseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterExpenseJson request,
        [FromServices] IRegisterExpenseUseCase useCase
    )
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    //[HttpGet]
    //[ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
    //public async Task<IActionResult> GetAllExpenses([FromServices] IGetAllExpenseUseCase useCase)
    //{
    //    var response = await useCase.Execute();

    //    if (response.Expenses.Count != 0)
    //        return Ok(response);

    //    return NoContent();
    //}
}

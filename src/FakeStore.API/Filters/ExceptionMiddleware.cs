using FakeStore.Communication.Responses;
using FakeStore.Exception;
using FakeStore.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FakeStoreException)
        {
            HandleProjectexception(context);
        }
        else
        {
            ThrowUnknowError(context);
        }
    }

    private void HandleProjectexception(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException)
        {
            var ex = (ErrorOnValidationException)context.Exception;
            var errorResponde = new ResponseErrorJson(ex.Errors);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponde);
        }
        else
        {
            var errorResponde = new ResponseErrorJson(context.Exception.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponde);
        }
    }
    private void ThrowUnknowError(ExceptionContext context)
    {
        var errorResponde = new ResponseErrorJson(ResourceErrorMessages.ERRO_DESCONHECIDO);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponde);
    }
}

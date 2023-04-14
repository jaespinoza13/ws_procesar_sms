using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Filters;

public class DailyRequestFilter : IActionFilter
{
    private readonly ILogger _logger;
    private readonly IDailyRequest _dailyRequest;
    public DailyRequestFilter(ILogger<DailyRequestFilter> logger, IDailyRequest daily)
    {
        this._logger = logger;
        this._dailyRequest = daily;
    }
    void IActionFilter.OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            //DAILY REQUEST FILTER
            var endpoint = context.HttpContext.Request.Path;
            string[] operacion = endpoint.Value!.Split( "/" );
            _dailyRequest.controlPeticionesDiaras( operacion[3].ToUpper() );
        }
        else
        {
            throw new ValidationException();
        }

    }
    void IActionFilter.OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation( "Despues de la peticion" );
    }
}


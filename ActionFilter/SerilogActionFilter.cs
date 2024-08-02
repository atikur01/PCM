using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace PCM.ActionFilter
{
    public class SerilogActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var controllerName = context.Controller.GetType().Name;
            var parameters = context.ActionArguments;

            Log.Information("Executing action {ActionName} in controller {ControllerName} with parameters {Parameters}",
                actionName, controllerName, parameters);

            var executedContext = await next();

            if (executedContext.Exception == null)
            {
                Log.Information("Action {ActionName} executed successfully", actionName);
            }
            else
            {
                Log.Error(executedContext.Exception, "Action {ActionName} threw an exception", actionName);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Serilog; 

namespace PCM.ActionFilter
{
    // This class implements the IAsyncActionFilter interface to create a custom action filter
    // that logs the execution details of actions in ASP.NET Core MVC controllers.
    public class SerilogActionFilter : IAsyncActionFilter
    {
        // This method is called before and after the action method is executed.
        // It provides an opportunity to run code both before the action starts and after it finishes.
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Extracting the action name from the context
            var actionName = context.ActionDescriptor.DisplayName;

            // Extracting the controller name by getting the type of the controller from the context
            var controllerName = context.Controller.GetType().Name;

            // Extracting the action method parameters from the context
            var parameters = context.ActionArguments;

            // Logging the details of the action method being executed
            Log.Information("Executing action {ActionName} in controller {ControllerName} with parameters {Parameters}",
                actionName, controllerName, parameters);

            // Proceeding to execute the action method
            var executedContext = await next();

            // Checking if the action executed without any exceptions
            if (executedContext.Exception == null)
            {
                // Logging a message indicating that the action executed successfully
                Log.Information("Action {ActionName} executed successfully", actionName);
            }
            else
            {
                // Logging an error message with details of the exception if the action threw an exception
                Log.Error(executedContext.Exception, "Action {ActionName} threw an exception", actionName);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters
{
    public class ActionExecutionLoggingFilter(ILogger<ActionExecutionLoggingFilter> logger) : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation(
                "Executing action {ActionName} with {ArgumentCount} argument(s).",
                context.ActionDescriptor.DisplayName,
                context.ActionArguments.Count);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;

            if (context.Exception is not null && !context.ExceptionHandled)
            {
                logger.LogError(context.Exception, "Action {ActionName} failed.", actionName);
                return;
            }

            logger.LogInformation("Finished action {ActionName}.", actionName);
        }
    }
}

using Crebito.Common.ErrorNotifications;

namespace Crebito.Api.EndpointFilters;

public class ErrorNotificationFilter : IEndpointFilter
{
    private readonly ErrorNotificationService _notificationService;

    public ErrorNotificationFilter(ErrorNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (_notificationService.HasError)
        {
            switch (_notificationService.ErrorType)
            {
                case NotificationErrorType.NotFound: return Results.NotFound(new { _notificationService.Errors });
                case NotificationErrorType.Domain: return Results.UnprocessableEntity(new { _notificationService.Errors });
            }
        }

        return result;
    }
}
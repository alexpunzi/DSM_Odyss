using Microsoft.AspNetCore.Mvc;

namespace DSM_Odyss.Web.Utils;

public static class ControllerJsonOrViewExtensions
{
    public static bool WantsJson(this Controller controller)
    {
        var accept = controller.Request.Headers.Accept;
        return accept.Any(a => a is not null
            && a.Contains("application/json", StringComparison.OrdinalIgnoreCase)
            && !a.Contains("text/html", StringComparison.OrdinalIgnoreCase));
    }
}

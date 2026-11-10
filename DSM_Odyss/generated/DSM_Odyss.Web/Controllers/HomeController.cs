using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DSM_Odyss.Web.ViewModels;

namespace DSM_Odyss.Web.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}

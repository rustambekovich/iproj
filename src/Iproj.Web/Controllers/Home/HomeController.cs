using IdentityServer4.Services;
using Iproj.Helpers;
using Iproj.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iproj.Controllers.Home;

[SecurityHeaders]
[Authorize]
public class HomeController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;

    public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Redirect("Account/Profile");
    }

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }

        ViewBag.IsErrorPage = true;  // Set a flag to indicate error page

        return View("Error", vm);
    }
}
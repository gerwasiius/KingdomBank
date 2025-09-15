using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
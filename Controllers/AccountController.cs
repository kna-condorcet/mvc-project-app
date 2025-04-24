using Condorcet.B2.AspnetCore.MVC.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Condorcet.B2.AspnetCore.MVC.Application.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View();


        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View();

        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        else
            return RedirectToAction("Index", "Home");
    }
}
namespace BTL_WebNC.Controllers;
using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller {
    public IActionResult Index() {
        
        return View();
    }

    public IActionResult Notifications() {
        return View();
    }

    public IActionResult SavedReviews() {
        return View();
    }
}
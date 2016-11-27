using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Notown.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Error/{ErrorCode}")]
        [AllowAnonymous]
        public IActionResult Error(int ErrorCode)
        {
            if (ErrorCode == 400)
                ViewData["Message"] = "Something went wrong, please try to go back and try again.";

            if (ErrorCode == 401)
                ViewData["Message"] = "You are not authorized to access this information.";

            else if (ErrorCode == 403)
                ViewData["Message"] = "You are forbidden from accessing this information.";

            else if (ErrorCode == 404)
                ViewData["Message"] = "The page you are looking for has either been moved or is no longer available. Please make sure you correctly typed the URL.";

            return View(ErrorCode);
        } 
    }
}
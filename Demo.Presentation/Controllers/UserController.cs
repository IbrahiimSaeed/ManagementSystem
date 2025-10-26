using Demo.DataAccess.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class UserController(UserManager<ApplicationUser> _userManager) : Controller
    {
        #region Index
        public IActionResult Index(string searchValue)
        {
            return View();
        } 
        #endregion
    }
}

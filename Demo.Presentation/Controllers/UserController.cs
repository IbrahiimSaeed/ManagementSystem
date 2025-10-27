using Demo.DataAccess.Models.IdentityModule;
using Demo.Presentation.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    //IWebHostEnvironment ==> For Check Environment In Catch 
    public class UserController(UserManager<ApplicationUser> _userManager, IWebHostEnvironment _env) : Controller
    {
        #region Index
        [HttpGet]
        public IActionResult Index(string searchValue)
        {
            var usersQuery = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
                usersQuery = usersQuery.Where(u => u.Email.ToLower().Contains(searchValue.ToLower()));
            var users = usersQuery.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            }).ToList();
            foreach (var user in users)
                user.Roles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(user.Id).Result).Result;

            return View(users);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id is null) return BadRequest();
            var user = _userManager?.FindByIdAsync(id).Result;
            if (user == null) return NotFound();
            var userVM = new UserViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = id,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(userVM);
        }

        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id is null) return BadRequest();
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null) return NotFound();
            var userVM = new UserViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = id,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(userVM);
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel userViewModel, string id)
        {
            if (!ModelState.IsValid) return View(userViewModel);
            if (userViewModel.Id != id) return BadRequest();
            string message = "";
            try
            {
                var user = _userManager.FindByIdAsync(id).Result;
                if (user == null) return NotFound();
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                var result = _userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    message = "User Can Not Be Updated ";
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "An Error Happen When Update The User ";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(userViewModel);
        }
        #endregion

        #region Delete
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null) return NotFound();
            string message = "";
            try
            {
                var result = _userManager.DeleteAsync(user).Result;
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    message = "User Can Not Be Deleted ";
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "An Error Happen When Delete The User ";
            }
            ModelState.AddModelError("", message);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

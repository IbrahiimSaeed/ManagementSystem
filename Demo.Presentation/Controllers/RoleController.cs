using Demo.Presentation.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class RoleController(RoleManager<IdentityRole> _roleManager ,IWebHostEnvironment _env) : Controller
    {
        #region Index
        [HttpGet]
        public IActionResult Index(string searchValue)
        {
            var rolesQuery = _roleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
                rolesQuery = rolesQuery.Where(r => r.Name.ToLower().Contains(searchValue.ToLower()));
            var roles = rolesQuery.Select(r => new RoleViewModel()
            {
                Id = r.Id,
                Name =r.Name
            }).ToList();
            return View(roles);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _roleManager.CreateAsync(new IdentityRole() { Name = roleViewModel.Name }).Result;
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Role Can Not Be Created");
            return View(roleViewModel);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id is null) return BadRequest();
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role == null) return NotFound();
            var roleVm = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleVm);
        }
        #endregion

        #region Edit
        public IActionResult Edit(string? id)
        {
            if (id is null) return BadRequest();
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role == null) return NotFound();
            var roleVm = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleVm);
        }
        [HttpPost]
        public IActionResult Edit(string? id, RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid) return View(roleViewModel);
            if (id != roleViewModel.Id) return BadRequest();
            string message = "";
            try
            {
                var role = _roleManager.FindByIdAsync(id).Result;
                if (role == null) return BadRequest();
                role.Name = roleViewModel.Name;
                var result = _roleManager.UpdateAsync(role).Result;
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    message = "Role Can Not Be Updated";
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Role Can Not Be Updated";
            }
            return View(roleViewModel);
        }
        #endregion

        #region Delete
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role == null) return NotFound();
            string message = "";
            try
            {
                var result = _roleManager.DeleteAsync(role).Result;
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    message = "The Role Can Not Be Deleted ";
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "An Error Happen When Delete The Role ";
            }
            ModelState.AddModelError("", message);
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}

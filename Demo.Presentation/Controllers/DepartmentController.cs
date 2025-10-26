using Demo.BusinessLogic.DTOS;
using Demo.BusinessLogic.DTOS.DepartmentDTOS;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    [Authorize]
    public class DepartmentController(IDepartmentService _departmentService,
        IWebHostEnvironment _env, ILogger<DepartmentController> _logger) : Controller
    {
        #region Index
        public IActionResult Index()
        {
            ViewData["Message"] = "Hello from view data";
            ViewBag.Message = "Hello from view bag";
            var department = _departmentService.GetAllDepartments();
            return View(department);
        }
        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] //Attribute ==> Action Filter
        public IActionResult Create(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = _departmentService.AddDepartment(new CreateDepartmentDto()
                    {
                        Code = departmentViewModel.Code,
                        Description = departmentViewModel.Description,
                        Name = departmentViewModel.Name,
                        DateOfCreation = departmentViewModel.CreatedOn
                    });
                    string message;
                    if (result > 0)
                        message = "Department created successfully";
                    else
                        message = "Department can not be created";
                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError(ex.Message);
                    }
                    else
                    {
                        _logger.LogError($"Department Can not be Created because {ex}");
                        // return View(departmentDto);
                        return View("ErrorView", ex);
                    }
                }
            }

            return View(departmentViewModel);

        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            //return View(department);
            var departmentvm = new DepartmentViewModel() //3shan h bind 3la 2 classes 1.DetailsDto 2.UpdateDTO
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreatedOn = department.CreatedOn.HasValue ? department.CreatedOn.Value : default
            };
            return View(departmentvm);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, DepartmentViewModel departmentvm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!id.HasValue) return BadRequest();
                    var updatedDeptDto = new UpdatedDepartmentDto()
                    {
                        Id = id.Value,
                        Code = departmentvm.Code,
                        Name = departmentvm.Name,
                        Description = departmentvm.Description,
                        DateOfCreation = departmentvm.CreatedOn
                    };
                    int result = _departmentService.UpdateDepartment(updatedDeptDto);
                    if (result > 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError(string.Empty, "department can not be updated");
                }

                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError(ex.Message);
                    }
                    else
                    {
                        _logger.LogError($"Department Can not be Created because {ex}");
                        // return View(departmentDto);
                        return View("ErrorView", ex);
                    }
                }
            }
            return View(departmentvm);
        }
        #endregion

        #region Delete
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue) return BadRequest();
        //    var department = _departmentService.GetDepartmentById(id.Value);
        //    if(department is null) return NotFound();
        //    return View(department);
        //}

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool isdeleted = _departmentService.DeleteDepartment(id);
                if (isdeleted)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Department can not be deleted");
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex.Message);
                    ModelState.AddModelError(string.Empty, "Department can not be deleted");
                }
                else
                {
                    _logger.LogError($"Department Can not be Created because {ex}");
                    // return View(departmentDto);
                    return View("ErrorView", ex);
                }
            }
            return RedirectToAction(nameof(Delete), new { id });
        }

        #endregion
    }
}

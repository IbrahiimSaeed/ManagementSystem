using Demo.BusinessLogic.DTOS.DepartmentDTOS;
using Demo.BusinessLogic.DTOS.EmployeeDTOS;
using Demo.BusinessLogic.Services.Classes;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Models.EmployeeModule;
using Demo.DataAccess.Models.Shared;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class EmployeeController(IEmployeeService _employeeService ,IWebHostEnvironment _env,ILogger<EmployeeController> _logger) : Controller
    {
        #region Index
        [HttpGet]
        public IActionResult Index(string? EmployeeSearchName)
        {
            var employees = _employeeService.GetAllEmployees(EmployeeSearchName);
            return View(employees);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = _employeeService.CreateEmployee(new CreateEmployeeDto() 
                    { 
                        Name = employeeViewModel.Name,
                        Age = employeeViewModel.Age,
                        Address = employeeViewModel.Address,
                        IsActive = employeeViewModel.IsActive,
                        DepartmentId = employeeViewModel.DepartmentId,
                        Salary = employeeViewModel.Salary,
                        Email = employeeViewModel.Email,
                        EmployeeType = employeeViewModel.EmployeeType,
                        Gender = employeeViewModel.Gender,
                        PhoneNumber = employeeViewModel.PhoneNumber,
                        HiringDate = employeeViewModel.HiringDate,
                    });
                    if (result > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Employee Can not be Created");
                    }

                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError(ex.Message);
                    }
                    else
                    {
                        _logger.LogError($"Employee Can not be Created because {ex}");
                        // return View(departmentDto);
                        return View("ErrorView", ex);
                    }
                }
            }

            return View(employeeViewModel);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            return View(employee);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            var employeeViewModel = new EmployeeViewModel() //need department id
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Email = employee.Email,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType),
                DepartmentId = employee.DepartmentId
            };
            return View(employeeViewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int? id,EmployeeViewModel employeeViewModel)
        {
            if (!id.HasValue) return BadRequest();
            if (!ModelState.IsValid) return View(employeeViewModel);
            try
            {
                int result = _employeeService.UpdateEmployee(new UpdatedEmployeeDto()
                {
                    Address = employeeViewModel.Address,
                    Age= employeeViewModel.Age,
                    IsActive= employeeViewModel.IsActive,
                    Gender = employeeViewModel.Gender,
                    EmployeeType = employeeViewModel.EmployeeType,
                    Email = employeeViewModel.Email,
                    Name = employeeViewModel.Name,
                    HiringDate = employeeViewModel.HiringDate,
                    Salary  = employeeViewModel.Salary,
                    DepartmentId = employeeViewModel.DepartmentId,
                    PhoneNumber= employeeViewModel.PhoneNumber,
                    Id = id.Value
                });
                if (result > 0)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee can not be updated");
                    return View(employeeViewModel);
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex.Message);
                    return View(employeeViewModel);
                }
                else
                {
                    _logger.LogError($"Employee Can not be Created because {ex}");
                    // return View(departmentDto);
                    return View("ErrorView", ex);
                }
            }
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool isdeleted = _employeeService.DeleteEmployee(id);
                if (isdeleted)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee can not be deleted");
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex.Message);
                    ModelState.AddModelError(string.Empty, "Employee can not be deleted");
                }
                else
                {
                    _logger.LogError($"Employee Can not be Created because {ex} ");
                    // return View(departmentDto);
                    return View("ErrorView", ex);
                }
            }
            return RedirectToAction(nameof(Delete), new { id });
        }
        #endregion
    }
}

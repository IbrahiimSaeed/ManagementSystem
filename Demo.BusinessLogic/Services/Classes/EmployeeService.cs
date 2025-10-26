using AutoMapper;
using Demo.BusinessLogic.DTOS.EmployeeDTOS;
using Demo.BusinessLogic.Services.AttachmentService;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Data.Repositories.Interfaces;
using Demo.DataAccess.Models.EmployeeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.Classes
{
    public class EmployeeService(IUnitOfWork _unitOfWork,IMapper _mapper,IAttachmentService _attachmentService) : IEmployeeService
    {
        public IEnumerable<EmployeeDto> GetAllEmployees(string? EmployeeSearchName, bool withTracking = false)
        {
            IEnumerable<Employee> employees;
            if (!String.IsNullOrWhiteSpace(EmployeeSearchName))
                employees = _unitOfWork.EmployeeRepository.GetAll(e => e.Name.ToLower().Contains(EmployeeSearchName.ToLower()));
            else
                employees = _unitOfWork.EmployeeRepository.GetAll(withTracking);

            var employeeDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            return employeeDto;
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            return employee is null ? null : _mapper.Map<Employee, EmployeeDetailsDto>(employee);
        }
        public int CreateEmployee(CreateEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<CreateEmployeeDto, Employee>(employeeDto);
            if (employeeDto.Image is not null)
            {
                string? imgName = _attachmentService.Upload(employeeDto.Image, "images");
                employee.ImageName = imgName;
            }
            _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.SaveChanges();
        }

        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {

            if(employeeDto.Image is not null)
            {
                string? newImageName = _attachmentService.Upload(employeeDto.Image, "images");
                if(newImageName is not null)
                {
                    employeeDto.ImageName = newImageName;
                }
            }
            _unitOfWork.EmployeeRepository.Update(_mapper.Map<UpdatedEmployeeDto, Employee>(employeeDto));
            return _unitOfWork.SaveChanges();
        }

        public bool DeleteEmployee(int id)
        {
            //Soft Delete ==> update [IsDeleted = true]
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee is null) return false;
            else
            {
                employee.IsDeleted = true;
                _unitOfWork.EmployeeRepository.Update(employee);
                return _unitOfWork.SaveChanges() > 0 ? true : false;
            }
        }

    }
}

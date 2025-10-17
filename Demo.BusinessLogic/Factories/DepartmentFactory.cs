using Demo.BusinessLogic.DTOS.DepartmentDTOS;
using Demo.DataAccess.Models.DepartmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Factories
{
    internal static class DepartmentFactory
    {
        public static DepartmentDto ToDepartmentDtop(this Department d)
        {
            return new DepartmentDto()
            {
                DeptId = d.Id,
                Name = d.Name,
                Description = d.Description,
                Code= d.Code,
                DateOfCreation = d.CreatedOn.HasValue ? DateOnly.FromDateTime(d.CreatedOn.Value) : default
            };
        }

        public static DepartmentDetailsDto ToDepartmentDetailsDto(this Department department)
        {
            return new DepartmentDetailsDto()
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreatedBy = department.CreatedBy,
                IsDeleted = department.IsDeleted,
                ModifiedBy = department.ModifiedBy,
                CreatedOn = department.CreatedOn.HasValue ? DateOnly.FromDateTime(department.CreatedOn.Value) : default,
                ModifiedOn = department.ModifiedOn.HasValue ? DateOnly.FromDateTime(department.ModifiedOn.Value) : default
            };
        }

        public static Department ToEntity(this CreateDepartmentDto createDepartmentDto)
        {
            return new Department()
            {
                Name = createDepartmentDto.Name,
                Description = createDepartmentDto.Description,
                Code = createDepartmentDto.Code,
                CreatedOn = createDepartmentDto.DateOfCreation.ToDateTime(new TimeOnly())
            };
        }

        public static Department ToEntity(this UpdatedDepartmentDto updateDepartmentDto)
        {
            return new Department()
            {
                Id = updateDepartmentDto.Id,
                Name = updateDepartmentDto.Name,
                Description = updateDepartmentDto.Description,
                Code = updateDepartmentDto.Code,
                CreatedOn = updateDepartmentDto.DateOfCreation.ToDateTime(new TimeOnly())
            };
        }
    }
}

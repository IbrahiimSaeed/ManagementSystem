using Demo.DataAccess.Models.EmployeeModule;
using Demo.DataAccess.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Data.Configurations
{
    internal class EmployeeConfigurations : BaseEntityConfigurations<Employee>, IEntityTypeConfiguration<Employee>
    {
        public new void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Address).HasColumnType("varchar(50)");
            builder.Property(E => E.Name).HasColumnType("varchar(50)");
            builder.Property(E => E.Salary).HasColumnType("decimal(10.2)");
            builder.Property(E => E.Gender).HasConversion((empGender) => empGender.ToString(),
                (gender) => (Gender)Enum.Parse(typeof(Gender), gender));
            builder.Property(E => E.EmployeeType).HasConversion((empType) => empType.ToString(),
                (type) => (EmployeeType)Enum.Parse(typeof(EmployeeType), type));
            base.Configure(builder);


        }
    }
}

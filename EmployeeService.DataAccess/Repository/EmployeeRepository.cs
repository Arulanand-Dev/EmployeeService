using EmployeeService.DataAccess.DBContext;
using EmployeeService.DataAccess.Repository.Interface;
using EmployeeService.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DataAccess.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        public readonly AppDbContext appDbContext;
        public EmployeeRepository(AppDbContext _appDbContext): base(_appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            if (employee.Department != null)
            {
                appDbContext.Entry(employee.Department).State = EntityState.Unchanged;
            }
            var result= await appDbContext.Employees.AddAsync(employee);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await appDbContext.Employees.AsNoTracking().Include(e=>e.Department).ToListAsync();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await appDbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(emp=>emp.EmployeeId == id);    

        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = await appDbContext.Employees.FirstOrDefaultAsync(emp => emp.EmployeeId == employee.EmployeeId);
            if(result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Phone = employee.Phone;
                result.Email = employee.Email;
                if (employee.Department.DepartmentId != 0)
                {
                    result.Department.DepartmentId = employee.Department.DepartmentId;
                }
                await appDbContext.SaveChangesAsync();
                return employee;
            }
                return null;

        }
    }
}

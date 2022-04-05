using EmployeeService.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DataAccess.DataAccess.Interface
{
    public interface IEmployeeDataAccess
    {
        Task<Employee> GetEmployee(int id);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<int> UpdateEmployeePatch(int id, JsonPatchDocument employee);
    }
}

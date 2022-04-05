using EmployeeService.DataAccess.DataAccess.Interface;
using EmployeeService.DataAccess.Repository.Interface;
using EmployeeService.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DataAccess.DataAccess
{
    public class EmployeeDataAccess : IEmployeeDataAccess
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeDataAccess(IEmployeeRepository _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            try
            {
                var newEmployee = await employeeRepository.AddEmployee(employee);
                return newEmployee;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            try 
            {
                return await employeeRepository.GetEmployees();
            }
            catch
            {
                throw;
            }
        }

        public Task<Employee> GetEmployee(int id)
        {
            try
            {
                return employeeRepository.GetAllIncluding(e=>e.EmployeeId==id, e=>e.Department)
                    .SingleAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            try
            {
                return await employeeRepository.UpdateEmployee(employee);
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateEmployeePatch(int id, JsonPatchDocument employee)
        {
            try
            {
                return await employeeRepository.UpdatePatch(id, employee);
            }
            catch
            {
                throw;
            }
        }
    }
}

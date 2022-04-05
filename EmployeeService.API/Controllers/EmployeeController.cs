using EmployeeService.DataAccess.DataAccess.Interface;
using EmployeeService.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EmployeeService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeDataAccess employeeDataAccess;

        public EmployeeController(IEmployeeDataAccess _employeeDataAccess)
        {
            employeeDataAccess = _employeeDataAccess;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            try
            {
                var result = await employeeDataAccess.GetEmployees();

                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<Employee>>> GetEmployee(int id)
        {
            try
            {
                var result = await employeeDataAccess.GetEmployee(id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<Employee>>> CreateEmployee(Employee employee)
        {
            try
            {
                if(employee == null)
                {
                    return BadRequest();
                }
                var result = await employeeDataAccess.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployee),
new { id = employee.EmployeeId }, result);
                //return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error creating employee");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<Employee>>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                if (id != employee.EmployeeId)
                {
                    return NotFound($"Employee with id ={id} not found");
                }
                var result = await employeeDataAccess.UpdateEmployee(employee);
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error retrieving data from the database");
            }
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, JsonPatchDocument employee)
        {
            try
            {
                if(employee== null)
                {
                    return BadRequest();
                }

                var result = await employeeDataAccess.UpdateEmployeePatch(id, employee);
                if (result == 1)
                {

                    return await employeeDataAccess.GetEmployee(id);
                }
                else
                {
                    return StatusCode(StatusCodes.Status304NotModified,
                        $"Employee update failed for Id:{id}");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     $"Employee update failed for Id:{id}");
            }
        }
    }
}

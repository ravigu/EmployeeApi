using EmployeeApi.Data;
using EmployeeApi.DTOs;
using EmployeeApi.Interfaces;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var employees = await _employeeRepository.GetAllAsync();
        var result = employees.Select(x => new EmployeeDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        await _employeeRepository.AddAsync(employee);

        return Ok(employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        var existing = await _employeeRepository.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        existing.Name = employee.Name;
        existing.Email = employee.Email;

         await _employeeRepository.UpdateAsync(existing);

        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        await _employeeRepository.UpdateAsync(employee);
        return NoContent();
    }
}
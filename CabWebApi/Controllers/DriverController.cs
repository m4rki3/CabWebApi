using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriverController: ControllerBase
{
	private readonly IModelService<Driver> driverService;
    public DriverController(IModelService<Driver> driverService)
    {
		this.driverService = driverService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(List<Driver>), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get()
    {
        List<Driver> dbDrivers = await driverService.GetAllAsync();

        if (dbDrivers.Count == 0)
            return NotFound("Drivers are not found in database");

        return Ok(dbDrivers);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        Driver? dbDriver = await driverService.GetAsync(id);

        if (dbDriver is null)
            return NotFound("Driver with requested id is not found in database");

        return Ok(dbDriver);
    }

    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Driver), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Driver driver)
    {
        Driver? dbDriver = await driverService.GetAsync(driver.Id);

        if (dbDriver is null)
            return NotFound("Driver with requested id is not found in database");

        DriverBuilder builder = new(dbDriver);
        builder.Named(driver.Name)
               .HasPhoneNumber(driver.PhoneNumber)
               .HasEmail(driver.Email)
               .HasBirthDate(driver.BirthDate)
               .Earns(driver.Salary)
               .HasLicense(driver.DrivingLicense);
        dbDriver = builder.Build();
        await driverService.UpdateAsync(dbDriver);

        return Ok(dbDriver);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        Driver? dbDriver = await driverService.GetAsync(id);

        if (dbDriver is null)
            return NotFound("Driver with requested id is not found in database");

        await driverService.DeleteAsync(dbDriver);

        return Ok("Driver is deleted successfully");
    }
}
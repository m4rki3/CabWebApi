using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;
using CarWebApi.Infrastructure.Business;
using Microsoft.AspNetCore.Mvc;

namespace CabWebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
	private readonly IModelService<Car> carService;
	public CarController(IModelService<Car> carService)
	{
		this.carService = carService;
	}

	[HttpGet]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(List<Car>), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get()
	{
		List<Car> dbCars = await carService.GetAllAsync();

		if (dbCars.Count == 0)
			return NotFound("Cars are not found in database");

		return Ok(dbCars);
	}

	[HttpGet]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get(int id)
	{
		Car? dbCar = await carService.GetAsync(id);

		if (dbCar is null)
			return NotFound("Car with requested id is not found in database");

		return Ok(dbCar);
	}

	[HttpPut]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(Car car)
	{
		Car? dbCar = await carService.GetAsync(car.Id);

		if (dbCar is null)
			return NotFound("Car with requested id is not found in database");

		CarBuilder builder = new(dbCar);
		builder.Model(car.ModelName)
			   .RegisteredAs(car.RegistrationPlate)
			   .HasDriver(car.DriverId)
			   .InStatus(car.Status);
		dbCar = builder.Build();
		await carService.UpdateAsync(dbCar);

		return Ok(dbCar);
	}

	[HttpDelete]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	public async Task<IActionResult> Delete(int id)
	{
		Car? dbCar = await carService.GetAsync(id);

		if (dbCar is null)
			return NotFound("Car with requested id is not found in database");

		await carService.DeleteAsync(dbCar);

		return Ok("Car is deleted successfully");
	}
}

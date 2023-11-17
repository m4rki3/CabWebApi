using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using CarWebApi.Infrastructure.Business;
using Microsoft.AspNetCore.Mvc;

namespace CabWebApi.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class CarController : ControllerBase
{
	private readonly IModelService<Car> carService;
	private readonly IModelService<Driver> driverService;
	public CarController(
		IModelService<Car> carService,
		IModelService<Driver> driverService)
	{
		this.carService = carService;
		this.driverService = driverService;
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

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get(int id)
	{
		Car? dbCar = await carService.GetAsync(id);

		if (dbCar is null)
			return NotFound("Car with requested id is not found in database");

		return Ok(dbCar);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id, CarModel model)
	{
		Car? dbCar = await carService.GetAsync(id);

		if (dbCar is null)
			return NotFound("Car with requested id is not found in database");

		Driver? dbDriver = await driverService.GetAsync(model.DriverId);

		if (dbDriver is null)
			return BadRequest($"{nameof(CarModel)}.{nameof(CarModel.DriverId)} has wrong value");

		List<Car> dbCars = await carService.GetAllWithAsync(
			nameof(Car.RegistrationPlate),
			model.SeriesRegistrationNumber + model.RegionCode.ToString());

		if (dbCars.Count != 0 && dbCars.SingleOrDefault()?.Id != id)
			return BadRequest($"{nameof(CarModel)}.{nameof(CarModel.SeriesRegistrationNumber)} " +
				$"or {nameof(CarModel)}.{nameof(CarModel.RegionCode)} has wrong value");

		CarBuilder builder = new(dbCar);
		builder.Model(model.ModelName)
			   .RegisteredAs(model.SeriesRegistrationNumber + model.RegionCode)
			   .HasDriver(model.DriverId)
			   .InStatus(model.Status);
		dbCar = builder.Build();
		await carService.UpdateAsync(dbCar);

		return Ok(dbCar);
	}

	[HttpDelete("{id}")]
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

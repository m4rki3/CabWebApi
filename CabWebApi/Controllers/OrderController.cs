using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController : ControllerBase
{
	private readonly ICarService carService;
	private readonly IOrderService orderService;
	private readonly IAccountService<User> userService;
	private readonly IModelService<Location> locationService;
	public OrderController(
		ICarService carService,
		IOrderService orderService,
		IAccountService<User> userService,
		IModelService<Location> locationService)
	{
		this.carService = carService;
		this.orderService = orderService;
		this.userService = userService;
		this.locationService = locationService;
	}

	[HttpGet]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetOrdersInExecution()
	{
		List<Order> orders = await orderService.GetOrdersInExecution();

		if (orders.IsNullOrEmpty())
			return NotFound("No orders in execution for this moment");

		return Ok(orders);
	}

	[HttpGet]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<int>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAvaliableCars()
	{
		List<Car> avaliableCars = await carService.GetAwaitingCarsAsync();

		if (avaliableCars.IsNullOrEmpty())
			return NotFound("No avaliable cars at this moment");

		IEnumerable<int> carsId = avaliableCars.Select(car => car.Id);
		return Ok(carsId);
	}

	[HttpGet]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetPrice(
		bool badWeather, int travelTimeMinutes, [FromQuery] int[] travelTimeMinutesToClient)
	{
		List<Car> avaliableCars = await carService.GetAwaitingCarsAsync();

		if (avaliableCars.IsNullOrEmpty())
			return BadRequest("No avaliable cars at this moment");

		List<Car> carsInWork = await carService.GetCarsInWorkAsync();

		int avaliableCarsCount = avaliableCars.Count;
		int carsInWorkCount = carsInWork.Count;

		int price = await orderService.GetPriceAsync(
			avaliableCarsCount, carsInWorkCount, badWeather,
			travelTimeMinutes, travelTimeMinutesToClient
		);

		return Ok(price);
	}

	[HttpPost]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
	public async Task<IActionResult> CreateOrder(OrderModel model)
	{
		Car? car = await carService.GetAsync(model.CarId);

		if (car is null)
			return BadRequest(
				$"{nameof(OrderModel)}.{nameof(OrderModel.CarId)} value is not found in database");

		if (car.Status != CarStatus.Awaiting)
			return BadRequest("Car is not avaliable now");

		User? user = await userService.GetAsync(model.UserId);

		if (user is null)
			return BadRequest(
				$"{nameof(OrderModel)}.{nameof(OrderModel.UserId)} value is not found in database");

		Location departure = new()
		{
			Latitude = model.DepartureLatitude,
			Longitude = model.DepartureLongitude
		};
		Location destination = new()
		{
			Latitude = model.DestinationLatitude,
			Longitude = model.DestinationLongitude
		};

		int departureId = (await locationService.CreateAsync(departure)).Item1.Entity.Id;

		int destinationId = (await locationService.CreateAsync(destination)).Item1.Entity.Id;

		OrderBuilder builder = new();
		builder.MakeWith
			   .User(model.UserId)
			   .Car(model.CarId)
			   .Price(model.Price)
			   .Status(model.Status)
			   .Route
			   .From(departureId)
			   .To(destinationId);
		Order order = builder.Build();
		Order dbOrder = (await orderService.CreateAsync(order)).Item1.Entity;

		return CreatedAtAction(nameof(CreateOrder), dbOrder);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get(int id)
	{
		Order? dbOrder = await orderService.GetAsync(id);

		if (dbOrder is null)
			return NotFound("Order with requested id is not found in database");

		return Ok(dbOrder);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id, OrderModel model)
	{
		Order? dbOrder = await orderService.GetAsync(id);

		if (dbOrder is null)
			return NotFound("Order with requested id is not found in database");

		User? dbUser = await userService.GetAsync(model.UserId);

		if (dbUser is null)
			return BadRequest($"{nameof(OrderModel)}.{nameof(OrderModel.UserId)} has wrong value");

		Car? dbCar = await carService.GetAsync(model.CarId);

		if (dbCar is null)
			return BadRequest($"{nameof(OrderModel)}.{nameof(OrderModel.CarId)} has wrong value");

		Location departure = new()
		{
			Latitude = model.DepartureLatitude,
			Longitude = model.DepartureLongitude
		};
		Location destination = new()
		{
			Latitude = model.DestinationLatitude,
			Longitude = model.DestinationLongitude
		};

		int departureId = (await locationService.CreateAsync(departure)).Item1.Entity.Id;

		int destinationId = (await locationService.CreateAsync(destination)).Item1.Entity.Id;

		OrderBuilder builder = new(dbOrder);
		builder.MakeWith
			   .User(model.UserId)
			   .Car(model.CarId)
			   .Price(model.Price)
			   .Status(model.Status)
			   .Route
			   .From(departureId)
			   .To(destinationId);
		dbOrder = builder.Build();
		await orderService.UpdateAsync(dbOrder);

		return Ok(dbOrder);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	public async Task<IActionResult> Delete(int id)
	{
		Order? dbOrder = await orderService.GetAsync(id);

		if (dbOrder is null)
			return NotFound("Order with requested id is not found in database");

		await orderService.DeleteAsync(dbOrder);

		return Ok("Order is deleted successfully");
	}
}
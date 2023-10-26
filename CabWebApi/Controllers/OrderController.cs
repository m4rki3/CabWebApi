using CabWebApi.Domain.Core;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ICarService carService;
    private readonly IOrderService orderService;
    private readonly IUserService userService;
    public OrderController(
        ICarService carService, IOrderService orderService, IUserService userService)
    {
        this.carService = carService;
        this.orderService = orderService;
        this.userService = userService;
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IEnumerable<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvaliableCars()
    {
        List<Car> avaliableCars = await carService.GetAwaitingCarsAsync();

        if (avaliableCars.IsNullOrEmpty())
            return NotFound();

        IEnumerable<int> carsId = avaliableCars.Select(car => car.Id);
        return Ok(carsId);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrice(
        bool badWeather, int travelTimeMinutes, params int[] travelTimeMinutesToClient)
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
        // 30 мин + плохая погода + время до клиента 10 минут - 400руб
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrder(OrderModel model)
    {
        Car? car = await carService.GetAsync(model.CarId);

        if (car is null)
            return BadRequest(
                $"{typeof(OrderModel)}.{nameof(OrderModel.CarId)} value is not found in database");

        if (car.Status != CarStatus.Awaiting)
            return BadRequest("Car is not avaliable now");

        User? user = await userService.GetAsync(model.UserId);

        if (user is null)
            return BadRequest(
                $"{typeof(OrderModel)}.{nameof(OrderModel.UserId)} value is not found in database");

        var locationsId = await orderService.CreateLocations(model.Departure, model.Destination);

        int departureId = locationsId.Item1;
        int destinationId = locationsId.Item2;

        Order order = orderService.FromModel(
            model.UserId, model.CarId, departureId, destinationId, model.Price
        );
        Order dbOrder = (await orderService.CreateAsync(order)).Item1.Entity;

        return CreatedAtAction(nameof(CreateOrder), dbOrder);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        Order? order = await orderService.GetAsync(id);

        if (order is null)
            return NotFound("Order with the same Id is not found in database");

        return Ok(order);
    }

    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Order order)
    {
        Order? dbOrder = await orderService.GetAsync(order.Id);

        if (dbOrder is null)
            return NotFound("Order with the same Id is not found in database");

        dbOrder = order;
        await orderService.UpdateAsync(dbOrder);

        return Ok(dbOrder);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        Order? dbOrder = await orderService.GetAsync(id);

        if (dbOrder is null)
            return NotFound("Order with the same Id is not found in database");

        await orderService.DeleteAsync(dbOrder);

        return Ok("Order is deleted successfully");
    }
}
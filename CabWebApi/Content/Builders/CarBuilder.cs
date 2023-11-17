using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public class CarBuilder
{
	protected Car car;
	public CarBuilder()
	{
		car = new();
	}
	public CarBuilder(Car car)
	{
		this.car = car;
	}
	public Car Build() => car;
	public CarBuilder Model(string modelName)
	{
		car.ModelName = modelName;
		return this;
	}
	public CarBuilder RegisteredAs(string registrationPlate)
	{
		car.RegistrationPlate = registrationPlate;
		return this;
	}
	public CarBuilder HasDriver(int driverId)
	{
		car.DriverId = driverId;
		return this;
	}
	public CarBuilder InStatus(CarStatus status)
	{
		car.Status = status;
		return this;
	}
}
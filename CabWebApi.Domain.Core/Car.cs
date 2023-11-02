namespace CabWebApi.Domain.Core;
public class Car
{
    public int Id { get; set; }
    public string RegistrationPlate { get; set; }
    public string ModelName { get; set; }
    public int DriverId { get; set; }
    public Driver Driver { get; set; }
    public CarStatus Status { get; set; }
    public IEnumerable<Order> Orders { get; set; }
}
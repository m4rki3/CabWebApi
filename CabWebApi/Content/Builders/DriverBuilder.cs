using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public class DriverBuilder : UserBuilder<Driver, DriverBuilder>
{
    public DriverBuilder() : base() { }
    public DriverBuilder(Driver driver) : base(driver) { }
    public DriverBuilder Earns(int salary)
    {
        user.Salary = salary;
        return this;
    }
    public DriverBuilder HasLicense(int drivingLicense)
    {
        user.DrivingLicense = drivingLicense;
        return this;
    }
}
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using Moq;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using S4.HseCarShop.Models;
using S4.HseCarShop.Models.Abstractions;
using S4.HseCarShop.Models.HandCar;
using S4.HseCarShop.Models.HybridCar;
using S4.HseCarShop.Models.PedalCar;
using S4.HseCarShop.Services;
using S4.HseCarShop.Services.Abstractions;
using S4.HseCarShop.Services.HandCars;
using S4.HseCarShop.Services.HybridCars;
using S4.HseCarShop.Services.PedalCars;
using System.ComponentModel.DataAnnotations;

using static Bogus.DataSets.Name;

namespace S4.HseCarShop.Tests;

public class CarShopServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ICarProvider> _carProviderMock;
    private readonly Mock<ICustomerStorage> _customersProviderMock;

    public CarShopServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _carProviderMock = _fixture.Freeze<Mock<ICarProvider>>();
        _customersProviderMock = _fixture.Freeze<Mock<ICustomerStorage>>();
    }

    #region Tests

    [Theory]
    [InlineAutoMockData(6, 5)]
    [InlineAutoMockData(3, 8)]
    [InlineAutoMockData(7, 2)]
    internal void SellCars_CustomerAlreadyHasCar_ShouldNotAssignNewCar(
        uint legStrength, uint handStrength, [Frozen] Mock<ICarProvider> carStorageMock,
        [Frozen] Mock<ICustomerStorage> customerStorageMock, CarShopService carShop)
    {
        var customer = _fixture.Build<Customer>()
            .FromFactory(() => new Customer(_fixture.Create<string>(), legStrength, handStrength))
            .Without(c => c.Car)
            .Create();

        customerStorageMock.Setup(cs => cs.GetCustomers()).Returns(new[] { customer });

        carShop.SellCars();

        Assert.Null(customer.Car);
        carStorageMock.Verify(x => x.GetCar(It.IsAny<IEnumerable<CarType>>()), Times.Never);
    }

    [Theory]
    [InlineAutoMockData(3, 3)]
    [InlineAutoMockData(6, 1)]
    [InlineAutoMockData(2, 5)]
    internal void SellCars_ValidateAssignCarsToCustomers(
        uint legStrength, uint handStrength, [Frozen] Mock<ICarProvider> carStorageMock,
        [Frozen] Mock<ICustomerStorage> customerStorageMock, CarShopService carShop)
    {
        var customer = _fixture.Build<Customer>()
            .FromFactory(() => new Customer(_fixture.Create<string>(), legStrength, handStrength))
            .Without(c => c.Car)
            .Create();
        customerStorageMock.Setup(cs => cs.GetCustomers()).Returns([customer]);

        
        carShop.SellCars();

        Assert.Null(customer.Car);

        carStorageMock.Verify(x => x.GetCar(It.IsAny<IEnumerable<CarType>>()), Times.Never);
    }

    [Theory]
    [InlineAutoMockData(5, 0)]
    [InlineAutoMockData(0, 5)]
    [InlineAutoData(5, 5)]
    internal void SellCars_OnlySuitableCarsAssigned(
        uint legStrength,
        uint handStrength,
        [Frozen] Mock<ICarProvider> carStorageMock,
        [Frozen] Mock<ICustomerStorage> customerStorageMock)
    {
        var customer = _fixture.Build<Customer>()
            .FromFactory(() => new Customer(_fixture.Create<string>(), legStrength, handStrength))
            .Without(c => c.Car)
            .Create();

        var expectedCar = _fixture.Build<PedalCar>()
            .FromFactory(() => new PedalCar(_fixture.Create<Guid>(), new PedalEngine(35)))
            .Create();

        customerStorageMock.Setup(cs => cs.GetCustomers()).Returns(new[] { customer });
        carStorageMock.Setup(cp => cp.GetCar(It.IsAny<IEnumerable<CarType>>())).Returns(expectedCar);
       
        CarShopService carShop = new(carStorageMock.Object, customerStorageMock.Object, new CarAvailabilityService(
            [new HandCarAvailabilityCheck(), new PedalCarAvailabilityCheck(), new HybridCarAvailabilityCheck()]));

        carShop.SellCars();

        carStorageMock.Verify(cp => cp.GetCar(It.IsAny<IEnumerable<CarType>>()), Times.Never);
    }

    [Theory]
    [InlineAutoMockData(3, 6)]
    [InlineAutoMockData(2, 4)]
    [InlineAutoMockData(5, 0)]
    internal void SellCars_IgnoreNonSuitableCustomers(
        uint legStrength, uint handStrength, [Frozen] Mock<ICarProvider> carStorageMock,
        [Frozen] Mock<ICustomerStorage> customerStorageMock, CarShopService carShop)
    {
        var customer = _fixture.Build<Customer>()
            .FromFactory(() => new Customer(_fixture.Create<string>(), legStrength, handStrength))
            .Without(c => c.Car)
            .Create();

        customerStorageMock.Setup(cs => cs.GetCustomers()).Returns(new[] { customer });

        carShop.SellCars();

        Assert.Null(customer.Car);
        carStorageMock.Verify(x => x.GetCar(It.IsAny<IEnumerable<CarType>>()), Times.Never);
    }

    [Theory]
    [AutoMockData]
    internal void SellCars_ValidateMultipleCustomerAssignments(
        [CustomizeCustomer(4, 8)] Customer customer1,
        [CustomizeCustomer(8, 4)] Customer customer2,
        [CustomizeCustomer(1, 1)] Customer customer3,
        [Frozen] Mock<ICarProvider> carStorageMock,
        [Frozen] Mock<ICustomerStorage> customerStorageMock
    )
    {
        var customers = new[] { customer1, customer2, customer3 };

        var expectedCars = new ICar[]
        {
        _fixture.Build<HandCar>().FromFactory(() => new HandCar(_fixture.Create<Guid>(), new HandEngine(GripsType.Rubber))).Create(),
        _fixture.Build<PedalCar>().FromFactory(() => new PedalCar(_fixture.Create<Guid>(), new PedalEngine(36))).Create(),
        _fixture.Build<HybridCar>().FromFactory(() => new HybridCar(_fixture.Create<Guid>(), new HybridEngine(GripsType.Silicone, 41))).Create(),
        };

        customerStorageMock.Setup(x => x.GetCustomers()).Returns(customers);
        carStorageMock.SetupSequence(x => x.GetCar(It.IsAny<IEnumerable<CarType>>()))
            .Returns(expectedCars[0])
            .Returns(expectedCars[1])
            .Returns(expectedCars[2]);

        CarShopService carShop = new(carStorageMock.Object, customerStorageMock.Object, new CarAvailabilityService(
            [new HandCarAvailabilityCheck(), new PedalCarAvailabilityCheck(), new HybridCarAvailabilityCheck()]));

        carShop.SellCars();

        Assert.Equal(expectedCars[0], customers[0].Car);
        Assert.Equal(expectedCars[1], customers[1].Car);
        Assert.Equal(expectedCars[2], customers[2].Car);
        carStorageMock.Verify(x => x.GetCar(It.IsAny<IEnumerable<CarType>>()), Times.Exactly(3));
    }


    #endregion Tests
}

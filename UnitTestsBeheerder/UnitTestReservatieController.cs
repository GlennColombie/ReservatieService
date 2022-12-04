using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservatieServiceBeheerderRESTService.Controllers;
using ReservatieServiceBeheerderRESTService.Exceptions;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Model.Output;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using System.Runtime.CompilerServices;

namespace UnitTestsBeheerder
{
    public class UnitTestReservatieController
    {
        private ReservatieController RC;

        private Mock<IRestaurantRepository> resRepoMock;
        private Mock<ILocatieRepository> lRepoMock;

        private Mock<LocatieManager> lMock;
        private Mock<RestaurantManager> resMock;

        private Mock<IMapFromDomain> mapMock;

        [Fact]
        public void TestGET_RestaurantIdSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            resMock = new Mock<RestaurantManager>(resRepoMock.Object, lRepoMock.Object);
            mapMock = new Mock<IMapFromDomain>();
            RC = new ReservatieController(mapMock.Object, resMock.Object);

            // Act
            var res = RC.Get(0, DateTime.Now.ToString(), DateTime.Now.AddDays(1).ToString());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_NoRestaurantFound_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            resMock = new Mock<RestaurantManager>(resRepoMock.Object, lRepoMock.Object);
            mapMock = new Mock<IMapFromDomain>();
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Throws(new RestaurantManagerException("Geen restaurant op deze id gevonden"));
            RC = new ReservatieController(mapMock.Object, resMock.Object);

            // Act
            var res = RC.Get(1, DateTime.Now.ToString(), DateTime.Now.AddDays(1).ToString());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_DTOException_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            resMock = new Mock<RestaurantManager>(resRepoMock.Object, lRepoMock.Object);
            mapMock = new Mock<IMapFromDomain>();
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            mapMock.Setup(repo => repo.MapFromReservatieDomain(It.IsAny<Reservatie>())).Throws(new MapException("Fout bij het omzetten naar DTO"));
            RC = new ReservatieController(mapMock.Object, resMock.Object);

            // Act
            var res = RC.Get(1, DateTime.Now.ToString(), DateTime.Now.AddDays(1).ToString());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_Valid_Ok()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            resMock = new Mock<RestaurantManager>(resRepoMock.Object, lRepoMock.Object);
            mapMock = new Mock<IMapFromDomain>();
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.GeefReservatiesRestaurant(It.IsAny<Restaurant>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Reservatie>() { new Reservatie()});
            mapMock.Setup(repo => repo.MapFromReservatieDomain(It.IsAny<Reservatie>())).Returns(new ReservatieRESToutputDTO());
            RC = new ReservatieController(mapMock.Object, resMock.Object);

            // Act
            var res = RC.Get(1, DateTime.Now.ToString(), DateTime.Now.AddDays(1).ToString());

            // Assert
            Assert.IsType<OkObjectResult>(res.Result);
        }
    }
}
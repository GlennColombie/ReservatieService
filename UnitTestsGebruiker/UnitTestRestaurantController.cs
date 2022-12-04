using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using ReservatieServiceGebruikerRESTService.Controllers;
using ReservatieServiceGebruikerRESTService.Exceptions;
using ReservatieServiceGebruikerRESTService.MapperInterface;
using ReservatieServiceGebruikerRESTService.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsGebruiker
{
    public class UnitTestRestaurantController
    {
        private RestaurantController RC;
        
        private Mock<IRestaurantRepository> resRepoMock;
        private Mock<ILocatieRepository> lRepoMock;
        
        private Mock<LocatieManager> lMock;        
        private Mock<RestaurantManager> resMock;
        
        private Mock<IMapFromDomain> mapMock;

        [Fact]
        public void TestGET_DTOException_BadRequest()
        {
            // Arrange
            resRepoMock = new();
            lRepoMock = new();
            mapMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            RC = new RestaurantController(mapMock.Object, resMock.Object);
            resMock.Setup(repo => repo.GeefRestaurants(It.IsAny<int?>(), It.IsAny<string>())).Returns(new List<Restaurant>() { new Restaurant()});
            mapMock.Setup(repo => repo.MapFromRestaurantDomain(It.IsAny<Restaurant>())).Throws(new MapException("Fout bij omzetten naar DTO"));

            // Act
            var result = RC.GetRestaurants(9160, "Belgisch");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void TestGET_Valid_Ok()
        {
            // Arrange
            resRepoMock = new();
            lRepoMock = new();
            mapMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            RC = new RestaurantController(mapMock.Object, resMock.Object);
            resMock.Setup(repo => repo.GeefRestaurants(It.IsAny<int?>(), It.IsAny<string>())).Returns(new List<Restaurant>() { new Restaurant() });
            mapMock.Setup(repo => repo.MapFromRestaurantDomain(It.IsAny<Restaurant>())).Returns(new RestaurantRESToutputDTO());

            // Act
            var result = RC.GetRestaurants(9160, "Belgisch");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void TESTGETRestaurantsVrijeTafels_AantalPersonenSmallerThan1_BadRequest()
        {
            // Arrange
            resRepoMock = new();
            lRepoMock = new();
            mapMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            RC = new RestaurantController(mapMock.Object, resMock.Object);

            // Act
            var result = RC.GetRestaurantsMetVrijeTafels("25/12/2022 18:30", 0, 9160, "Belgisch");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void TESTGETRestaurantsVrijeTafels_DatumPast_BadRequest()
        {
            // Arrange
            resRepoMock = new();
            lRepoMock = new();
            mapMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            RC = new RestaurantController(mapMock.Object, resMock.Object);

            // Act
            var result = RC.GetRestaurantsMetVrijeTafels("25/11/2022 18:30", 2, 9160, "Belgisch");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void TESTGETRestaurantsVrijeTafels_DTOException_BadRequest()
        {
            // Arrange
            resRepoMock = new();
            lRepoMock = new();
            mapMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            resMock.Setup(repo => repo.GeefRestaurantsMetVrijeTafels(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<string>())).Returns(new List<Restaurant>() { new Restaurant() });
            mapMock.Setup(repo => repo.MapFromRestaurantDomain(It.IsAny<Restaurant>())).Throws(new MapException("Fout bij omzetten naar DTO"));
            RC = new RestaurantController(mapMock.Object, resMock.Object);

            // Act
            var result = RC.GetRestaurantsMetVrijeTafels("25/12/2022 18:30", 2, 9160, "Belgisch");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void TESTGETRestaurantsVrijeTafels_Valid_Ok()
        {
            // Arrange
            resRepoMock = new();
            lRepoMock = new();
            mapMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            resMock.Setup(repo => repo.GeefRestaurantsMetVrijeTafels(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<string>())).Returns(new List<Restaurant>() { new Restaurant() });
            mapMock.Setup(repo => repo.MapFromRestaurantDomain(It.IsAny<Restaurant>())).Returns(new RestaurantRESToutputDTO());
            RC = new RestaurantController(mapMock.Object, resMock.Object);

            // Act
            var result = RC.GetRestaurantsMetVrijeTafels("25/12/2022 18:30", 2, 9160, "Belgisch");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}

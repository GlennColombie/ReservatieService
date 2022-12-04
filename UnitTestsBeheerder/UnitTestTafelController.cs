using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservatieServiceBeheerderRESTService.Controllers;
using ReservatieServiceBeheerderRESTService.Exceptions;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Model.Input;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsBeheerder
{
    public class UnitTestTafelController
    {
        private TafelController TC;

        private Mock<IRestaurantRepository> resRepoMock;
        private Mock<ILocatieRepository> lRepoMock;

        private Mock<LocatieManager> lMock;
        private Mock<RestaurantManager> resMock;

        private Mock<IMapToDomain> mapToMock;

        [Fact]
        public void TestPOST_RestaurantIDSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Post(-1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPOST_TafelNull_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Post(1, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPOST_DTOException_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Throws(new MapException("Fout bij omzetten naar Tafel"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Post(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPOST_RestaurantDoesntExist_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Throws(new RestaurantManagerException("Restaurant bestaat niet"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Post(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPOST_TafelExists_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(rm => rm.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.VoegTafelToe(It.IsAny<Tafel>(), It.IsAny<Restaurant>())).Throws(new RestaurantManagerException("Tafel bestaat al"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Post(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPOST_Valid_Ok()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(rm => rm.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.VoegTafelToe(It.IsAny<Tafel>(), It.IsAny<Restaurant>()));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Post(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public void TestPUT_RestaurantIDSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Put(-1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_TafelNull_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Put(1, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_DTOException_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Throws(new MapException("Fout bij omzetten naar Tafel"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Put(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_RestaurantDoesntExist_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Throws(new RestaurantManagerException("Restaurant bestaat niet"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Put(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_TafelDoesntExist_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(rm => rm.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.UpdateTafel(It.IsAny<Tafel>(), It.IsAny<Restaurant>())).Throws(new RestaurantManagerException("Tafel bestaat niet"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Put(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_Valid_Ok()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(rm => rm.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.UpdateTafel(It.IsAny<Tafel>(), It.IsAny<Restaurant>()));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Put(1, new TafelRESTinputDTO());

            // Assert
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public void TestDELETE_TafelnummerSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Delete(-1, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_RestaurantIDSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Delete(1, -1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_RestaurantDoesntExist_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Throws(new RestaurantManagerException("Restaurant bestaat niet"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Delete(1, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_TafelDoesntExist_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(rm => rm.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.VerwijderTafel(It.IsAny<Tafel>(), It.IsAny<Restaurant>())).Throws(new RestaurantManagerException("Tafel bestaat niet"));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);
            
            // Act
            var res = TC.Delete(1, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }
        
        [Fact]
        public void TestDELETE_Valid_Ok()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToTafelDomain(It.IsAny<TafelRESTinputDTO>())).Returns(new Tafel());
            resMock.Setup(rm => rm.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            resMock.Setup(repo => repo.VerwijderTafel(It.IsAny<Tafel>(), It.IsAny<Restaurant>()));
            TC = new TafelController(mapToMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = TC.Delete(1, 1);

            // Assert
            Assert.IsType<OkResult>(res);
        }
    }
}

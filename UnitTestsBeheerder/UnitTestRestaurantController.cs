using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservatieServiceBeheerderRESTService.Controllers;
using ReservatieServiceBeheerderRESTService.Exceptions;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Model.Input;
using ReservatieServiceBeheerderRESTService.Model.Output;
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
    public class UnitTestRestaurantController
    {
        private RestaurantController RC;

        private Mock<IRestaurantRepository> resRepoMock;
        private Mock<ILocatieRepository> lRepoMock;

        private Mock<LocatieManager> lMock;
        private Mock<RestaurantManager> resMock;

        private Mock<IMapFromDomain> mapFromMock;
        private Mock<IMapToDomain> mapToMock;

        [Fact]
        public void TestGET_RestaurantIdSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Get(-1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_NoRestaurantFound_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Throws(new RestaurantManagerException("Geen restaurant op deze id gevonden"));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Get(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_Valid_Ok()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.GeefRestaurant(It.IsAny<int>())).Returns(new Restaurant());
            mapFromMock.Setup(mapper => mapper.MapFromRestaurantDomain(It.IsAny<Restaurant>())).Returns(new RestaurantRESToutputDTO());
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Get(1);

            // Assert
            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_Null_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Post(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_DTOException_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToRestaurantDomain(It.IsAny<RestaurantRESTinputDTO>(), lMock.Object)).Throws(new MapException("Fout bij het omzetten naar Restaurant"));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Post(new RestaurantRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_RestaurantExists_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToRestaurantDomain(It.IsAny<RestaurantRESTinputDTO>(), lMock.Object)).Returns(new Restaurant());
            resMock.Setup(repo => repo.VoegRestaurantToe(It.IsAny<Restaurant>())).Throws(new RestaurantManagerException("Restaurant bestaat al"));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Post(new RestaurantRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_Valid_Ok()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            mapToMock.Setup(mapper => mapper.MapToRestaurantDomain(It.IsAny<RestaurantRESTinputDTO>(), lMock.Object)).Returns(new Restaurant());
            resMock.Setup(repo => repo.VoegRestaurantToe(It.IsAny<Restaurant>()));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Post(new RestaurantRESTinputDTO());

            // Assert
            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void TestPUT_RestaurantIdSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Put(-1, new RestaurantRESTinputUpdateDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_Null_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Put(1, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_RestaurantIdNotEqual_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Put(1, new RestaurantRESTinputUpdateDTO() { Id = 2});

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
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.BestaatRestaurant(It.IsAny<int>())).Returns(false);
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Put(1, new RestaurantRESTinputUpdateDTO() { Id = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void TestPUT_DTOException_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.BestaatRestaurant(It.IsAny<int>())).Returns(true);
            mapToMock.Setup(mapper => mapper.MapToRestaurantDomain(It.IsAny<int>(), It.IsAny<RestaurantRESTinputUpdateDTO>(), lMock.Object, resMock.Object)).Throws(new MapException("Fout bij het omzetten naar Restaurant"));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Put(1, new RestaurantRESTinputUpdateDTO() { Id = 1 });

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_Valid_CreatedAtAction()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.BestaatRestaurant(It.IsAny<int>())).Returns(true);
            mapToMock.Setup(mapper => mapper.MapToRestaurantDomain(It.IsAny<int>(), It.IsAny<RestaurantRESTinputUpdateDTO>(), lMock.Object, resMock.Object)).Returns(new Restaurant());
            resMock.Setup(repo => repo.UpdateRestaurant(It.IsAny<Restaurant>()));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Put(1, new RestaurantRESTinputUpdateDTO() { Id = 1 });

            // Assert
            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void TestDELETE_RestaurantIDSmallerThan0_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Delete(-1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_NoRestaurantFound_BadRequest()
        {
            // Arrange
            lRepoMock = new();
            resRepoMock = new();
            lMock = new(lRepoMock.Object);
            resMock = new(resRepoMock.Object, lRepoMock.Object);
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.GeefRestaurant(1)).Throws(new RestaurantManagerException("Restaurant niet gevonden"));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Delete(1);

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
            mapFromMock = new();
            mapToMock = new();
            resMock.Setup(repo => repo.GeefRestaurant(1)).Returns(new Restaurant());
            resMock.Setup(repo => repo.VerwijderRestaurant(It.IsAny<Restaurant>()));
            RC = new RestaurantController(mapToMock.Object, mapFromMock.Object, resMock.Object, lMock.Object);

            // Act
            var res = RC.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(res);
        }
    }
}

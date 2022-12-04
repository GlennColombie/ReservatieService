using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using ReservatieServiceGebruikerRESTService.Controllers;
using ReservatieServiceGebruikerRESTService.MapperInterface;
using ReservatieServiceGebruikerRESTService.Model.Input;
using ReservatieServiceGebruikerRESTService.Exceptions;
using ReservatieServiceRESTService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Exceptions;
using ReservatieServiceGebruikerRESTService.Model.Output;

namespace UnitTestsGebruiker
{
    public class UnitTestReservatieController
    {
        private ReservatieController RC;

        private Mock<GebruikerManager> gMock;
        private Mock<LocatieManager> lMock;
        private Mock<RestaurantManager> restMock;
        private Mock<ReservatieManager> resMock;

        private Mock<IGebruikerRepository> gRepoMock;
        private Mock<ILocatieRepository> lRepoMock;
        private Mock<IReservatieRepository> resRepoMock;
        private Mock<IRestaurantRepository> restRepoMock;

        private Mock<IMapToDomain> mapperToMock;
        private Mock<IMapFromDomain> mapperFromMock;

        [Fact]
        public void TestPOST_GebruikerIdSmallerThan0_BadRequest()
        {
            // Arrange
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);

            // Act
            var res = RC.Post(-1, 1, new ReservatieRESTinputDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_RestaurantIdSmallerThan0_BadRequest()
        {
            {
                // Arrange
                gRepoMock = new();
                lRepoMock = new();
                resRepoMock = new();
                restRepoMock = new();
                lMock = new Mock<LocatieManager>(lRepoMock.Object);
                gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
                restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
                resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
                mapperToMock = new Mock<IMapToDomain>();
                mapperFromMock = new Mock<IMapFromDomain>();
                RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);

                // Act
                var res = RC.Post(1, -1, new ReservatieRESTinputDTO());

                // Assert
                Assert.IsType<BadRequestObjectResult>(res.Result);
            }
        }

        [Fact]
        public void TestPOST_DTOException_BadRequest()
        {
            {
                // Arrange
                gRepoMock = new();
                lRepoMock = new();
                resRepoMock = new();
                restRepoMock = new();
                lMock = new Mock<LocatieManager>(lRepoMock.Object);
                gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
                restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
                resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
                mapperToMock = new Mock<IMapToDomain>();
                mapperFromMock = new Mock<IMapFromDomain>();
                mapperToMock.Setup(repo => repo.MapToReservatieDomain(1, 1, It.IsAny<ReservatieRESTinputDTO>(), gMock.Object, restMock.Object, lMock.Object)).Throws(new MapException("Fout bij het omzetten"));
                RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);

                // Act
                var res = RC.Post(1, 1, new ReservatieRESTinputDTO());

                // Assert
                Assert.IsType<BadRequestObjectResult>(res.Result);
            }
        }

        [Fact]
        public void TestPOST_Valid_Ok()
        {
            {
                // Arrange
                gRepoMock = new();
                lRepoMock = new();
                resRepoMock = new();
                restRepoMock = new();
                lMock = new Mock<LocatieManager>(lRepoMock.Object);
                gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
                restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
                resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
                mapperToMock = new Mock<IMapToDomain>();
                mapperFromMock = new Mock<IMapFromDomain>();
                Reservatie r = new();
                mapperToMock.Setup(repo => repo.MapToReservatieDomain(1, 1, new ReservatieRESTinputDTO(), gMock.Object, restMock.Object, lMock.Object)).Returns(r);
                resMock.Setup(repo => repo.VoegReservatieToe(r));
                RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);

                // Act
                var res = RC.Post(1, 1, new ReservatieRESTinputDTO());

                // Assert
                Assert.IsType<OkObjectResult>(res.Result);
            }
        }

        [Fact]
        public void TestDELETE_ReservatieIDSmallerThan0_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.Delete(-1);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_Valid_Ok()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            resMock.Setup(repo => repo.AnnuleerReservatie(1));
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.Delete(1);
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public void TestPUT_RestaurantIDSmallerThan0_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.Put(-1, new ReservatieRESTinputUpdateDTO());
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_DTOException_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            mapperToMock.Setup(repo => repo.MapToReservatieDomain(1, It.IsAny<ReservatieRESTinputUpdateDTO>(), resMock.Object)).Throws(new MapException("Fout bij omzetten naar DomainObject"));
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.Put(1, new ReservatieRESTinputUpdateDTO());
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_Valid_Ok()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            Reservatie r = new();
            mapperToMock.Setup(repo => repo.MapToReservatieDomain(1, It.IsAny<ReservatieRESTinputUpdateDTO>(), resMock.Object)).Returns(r);
            resMock.Setup(repo => repo.UpdateReservatie(r));
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.Put(1, new ReservatieRESTinputUpdateDTO());
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public void TestGET_GebruikerIDSmallerThan0_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.GetReservaties(-1, DateTime.Now.ToString(), DateTime.Now.ToString());
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_GebruikerDoesntExist_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            gMock.Setup(repo => repo.GeefGebruiker(1)).Throws(new GebruikerManagerException("Gebruiker bestaat niet"));
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.GetReservaties(1, DateTime.Now.ToString(), DateTime.Now.ToString());
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_DTOException_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            gMock.Setup(repo => repo.GeefGebruiker(1)).Returns(new Gebruiker());
            mapperFromMock.Setup(repo => repo.MapFromGebruikerDomain(It.IsAny<Gebruiker>())).Throws(new MapException("Fout bij omzetten naar DTO"));
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.GetReservaties(1, DateTime.Now.ToString(), DateTime.Now.ToString());
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_DTOException2_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            gMock.Setup(repo => repo.GeefGebruiker(1)).Returns(new Gebruiker());
            mapperFromMock.Setup(repo => repo.MapFromGebruikerDomain(It.IsAny<Gebruiker>())).Returns(new GebruikerRESToutputDTO());
            gMock.Setup(repo => repo.ZoekReservaties(It.IsAny<Gebruiker>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Reservatie>() { new Reservatie() }.AsReadOnly());
            mapperFromMock.Setup(repo => repo.MapFromReservatieDomain(It.IsAny<Reservatie>())).Throws(new MapException("Fout bij het omzetten naar DTO"));
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.GetReservaties(1, DateTime.Now.ToString(), DateTime.Now.ToString());
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_Valid_Ok()
        {
            gRepoMock = new();
            lRepoMock = new();
            resRepoMock = new();
            restRepoMock = new();
            lMock = new Mock<LocatieManager>(lRepoMock.Object);
            gMock = new Mock<GebruikerManager>(gRepoMock.Object, lRepoMock.Object);
            restMock = new Mock<RestaurantManager>(restRepoMock.Object, lRepoMock.Object);
            resMock = new Mock<ReservatieManager>(resRepoMock.Object, restRepoMock.Object, gRepoMock.Object, lRepoMock.Object);
            mapperToMock = new Mock<IMapToDomain>();
            mapperFromMock = new Mock<IMapFromDomain>();
            gMock.Setup(repo => repo.GeefGebruiker(1)).Returns(new Gebruiker());
            mapperFromMock.Setup(repo => repo.MapFromGebruikerDomain(It.IsAny<Gebruiker>())).Returns(new GebruikerRESToutputDTO());
            gMock.Setup(repo => repo.ZoekReservaties(It.IsAny<Gebruiker>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Reservatie>() { new Reservatie() }.AsReadOnly());
            mapperFromMock.Setup(repo => repo.MapFromReservatieDomain(It.IsAny<Reservatie>())).Returns(new ReservatieRESToutputDTO());
            RC = new ReservatieController(mapperToMock.Object, mapperFromMock.Object, resMock.Object, gMock.Object, restMock.Object, lMock.Object);
            var res = RC.GetReservaties(1, DateTime.Now.ToString(), DateTime.Now.ToString());
            Assert.IsType<OkObjectResult>(res.Result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using ReservatieServiceDL.Repositories;
using ReservatieServiceGebruikerRESTService.Exceptions;
using ReservatieServiceGebruikerRESTService.MapperInterface;
using ReservatieServiceGebruikerRESTService.Mappers;
using ReservatieServiceGebruikerRESTService.Model.Input;
using ReservatieServiceGebruikerRESTService.Model.Output;
using ReservatieServiceRESTService.Controllers;

namespace UnitTestsGebruiker
{
    public class UnitTestGebruikerController
    {
        private GebruikerController GC;
        private Mock<GebruikerManager> gMock;
        private Mock<LocatieManager> lMock;
        private Mock<IGebruikerRepository> gRepoMock;
        private Mock<ILocatieRepository> lRepoMock;
        private Mock<IMapToDomain> mapperToMock;
        private Mock<IMapFromDomain> mapperFromMock;

        [Fact]
        public void TestGET_InvalidID_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Get(-1);
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_GebruikerNotFound_NotFound()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            gMock.Setup(repo => repo.GeefGebruiker(1)).Throws(new GebruikerException("Gebruiker niet gevonden"));
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Get(1);
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_DTOException_NotFound()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            gMock.Setup(repo => repo.GeefGebruiker(1)).Throws(new GebruikerException("Gebruiker niet gevonden"));
            mapperFromMock.Setup(repo => repo.MapFromGebruikerDomain(It.IsAny<Gebruiker>())).Throws(new MapException("GebruikerDTO niet gevonden"));
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Get(1);
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public void TestGET_ValidID_Ok()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            Gebruiker g = new("Colombie", "glenn.colombie@student.hogent.be", "0494386634")
            {
                Locatie = new(9160, "Lokeren", null, null)
            };
            gMock.Setup(repo => repo.GeefGebruiker(1)).Returns(g);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Get(1);
            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_GebruikerNull_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Post(null);
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_GebruikerExists_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            LocatieRESTinputDTO ldto = new()
            {
                Postcode = 9160,
                Gemeente = "Lokeren",
                Straat = null,
                Huisnummer = null
            };
            GebruikerRESTinputDTO dto = new()
            {
                Naam = "Colombie",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = ldto
            };
            Locatie l = new(9160, "Lokeren", null, null);
            Gebruiker g = new()
            {
                Naam = "Colombie",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = l
            };
            mapperToMock.Setup(repo => repo.MapToGebruikerDomain(dto, lMock.Object)).Returns(g);
            gMock.Setup(repo => repo.GebruikerRegistreren(g)).Throws(new GebruikerManagerException("Gebruiker bestaat al"));
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Post(dto);
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact]
        public void TestPOST_ValidGebruiker_Ok()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            LocatieRESTinputDTO ldto = new()
            {
                Postcode = 9160,
                Gemeente = "Lokeren",
                Straat = null,
                Huisnummer = null
            };
            GebruikerRESTinputDTO dto = new()
            {
                Naam = "Colombie",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = ldto
            };
            Locatie l = new(9160, "Lokeren", null, null);
            Gebruiker g = new()
            {
                Naam = "Colombie",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = l
            };
            mapperToMock.Setup(repo => repo.MapToGebruikerDomain(dto, lMock.Object)).Returns(g);
            gMock.Setup(repo => repo.GebruikerRegistreren(g));
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Post(dto);
            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void TestPUT_IdSmallerThan0_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Put(-1, null);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_GebruikerNull_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Put(1, null);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_IdNotEqualToGebruikerId_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Put(1, new GebruikerRESTinputUpdateDTO() { GebruikerId = 2 });
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_GebruikerDoesntExist_NotFound()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            gMock.Setup(repo => repo.BestaatGebruiker(1)).Returns(false);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Put(1, new GebruikerRESTinputUpdateDTO() { GebruikerId = 1 });
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public void TestPUT_DTOException_NotFound()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            gMock.Setup(repo => repo.BestaatGebruiker(1)).Returns(true);
            mapperFromMock.Setup(repo => repo.MapFromGebruikerDomain(null)).Throws(new MapException("Gebruiker niet gevonden"));
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Put(1, new GebruikerRESTinputUpdateDTO() { GebruikerId = 1 });
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestPUT_Valid_OK()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            LocatieRESTinputDTO ldto = new()
            {
                Postcode = 9160,
                Gemeente = "Lokeren",
                Straat = null,
                Huisnummer = null
            };
            GebruikerRESTinputUpdateDTO dto = new()
            {
                GebruikerId = 1,
                Naam = "Colombie",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = ldto
            };
            Locatie l = new(9160, "Lokeren", null, null);
            Locatie l2 = new(9160, "Lokeren", null, null)
            {
                LocatieId = 1
            };
            Gebruiker db = new()
            {
                GebruikerId = 1,
                Naam = "Colombie",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = l2
            };
            Gebruiker g = new()
            {
                GebruikerId = 1,
                Naam = "Update",
                Email = "glenn.colombie@student.hogent.be",
                Telefoonnummer = "0494386634",
                Locatie = l2
            };
            gMock.Setup(repo => repo.BestaatGebruiker(1)).Returns(true);
            
            //mapperFromMock.Setup(repo => repo.MapFromGebruikerDomain(g)).Returns(new GebruikerRESToutputDTO() { GebruikerId = 1 });
           
            mapperToMock.Setup(repo => repo.MapToGebruikerDomain(dto, lMock.Object, gMock.Object)).Returns(g);
            gMock.Setup(repo => repo.GebruikerUpdaten(g));
            
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            
            var res = GC.Put(1, dto);
            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void TestDELETE_IdSmallerThan0_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Delete(-1);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_GebruikerDoesntExist_BadRequest()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            gMock.Setup(repo => repo.GeefGebruiker(1)).Throws(new GebruikerManagerException("Geen gebruiker gevonden op id"));
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Delete(1);
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public void TestDELETE_Valid_NoContent()
        {
            gRepoMock = new();
            lRepoMock = new();
            mapperToMock = new();
            mapperFromMock = new();
            gMock = new(gRepoMock.Object, lRepoMock.Object);
            lMock = new(lRepoMock.Object);
            gMock.Setup(repo => repo.GeefGebruiker(1)).Returns(new Gebruiker() { GebruikerId = 1});
            GC = new(mapperToMock.Object, mapperFromMock.Object, gMock.Object, lMock.Object);
            var res = GC.Delete(1);
            Assert.IsType<NoContentResult>(res);
        }
    }
}
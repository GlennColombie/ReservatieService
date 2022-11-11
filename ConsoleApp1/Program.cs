// See https://aka.ms/new-console-template for more information

using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;
using ReservatieServiceDL;
using ReservatieServiceDL.Repositories;

string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReservatieService;Integrated Security=True";
GebruikerManager gm = new(new GebruikerRepository(connectionString), new LocatieRepository(connectionString));
Locatie l = new(9160, "Lokeren");
Locatie l2 = new(9200, "Aalst");
//Gebruiker g = new(1,"Colombie", "test@test.be", "test", l2);
//gm.GebruikerRegistreren(new("Meyners", "test2@test.be", "test", l));
Gebruiker g = gm.GeefGebruiker(6);
g.ZetNaam("test25");
gm.GebruikerUpdaten(g);
Console.WriteLine("end");
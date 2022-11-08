// See https://aka.ms/new-console-template for more information

using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;
using ReservatieServiceDL;

GebruikerManager gm = new(new GebruikerRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=ReservatieService;Integrated Security=True"));
Locatie l = new(1, 9160, "Lokeren", "Hillarestraat", "67");
Gebruiker g = new(1,"Colombie", "test@test.be", "0494386634", l);
gm.GebruikerRegistreren(g);
Console.WriteLine("end");
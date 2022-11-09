// See https://aka.ms/new-console-template for more information

using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;
using ReservatieServiceDL;

GebruikerManager gm = new(new GebruikerRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=ReservatieService;Integrated Security=True"));
Locatie l = new(1, 9160, "Lokeren", "Hillarestraat", "67");
Locatie l2 = new(2, 9200, "Aalst");
Gebruiker g = new(1,"Colombie", "test@test.be", "test", l2);
gm.GebruikerUpdaten(g);
Console.WriteLine("end");
// See https://aka.ms/new-console-template for more information

using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Entities;
using ReservatieServiceDL;
using ReservatieServiceDL.Repositories;
using System.Text.RegularExpressions;
using ReservatieServiceBL.Interfaces;

string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReservatieService;Integrated Security=True";
//GebruikerManager gm = new(new GebruikerRepository(connectionString), new LocatieRepository(connectionString));
////RestaurantManager rm = new(new RestaurantRepository(connectionString), new LocatieRepository(connectionString));
//Locatie l = new(9160, "Lokeren");
//l.ZetId(4);
//Locatie l2 = new(9200, "Aalst");
////Gebruiker g = new(1,"Colombie", "test@test.be", "test", l2);
////gm.GebruikerRegistreren(new("Meyners", "test2@test.be", "test", l));
////Gebruiker g = gm.GeefGebruiker(6);
////g.ZetNaam("test25");
////gm.GebruikerUpdaten(g);
////Restaurant r = new("Cardis", l, "testcardis", "testcardis", Keuken.Belgisch);
////List<Restaurant> restaurants = (List<Restaurant>)rm.GeefAlleRestaurants();
////RestaurantManager rm = new(new RestaurantRepository(connectionString), new LocatieRepository(connectionString), new TafelRepository(connectionString));
////List<Restaurant> restaurants = (List<Restaurant>)rm.GeefAlleRestaurants();

////var regex = @"^(((\+|00)32[ ]?(?:\(0\)[ ]?)?)|0){1}(4(60|[789]\d)\/?(\s?\d{2}\.?){2}(\s?\d{2})|(\d\/?\s?\d{3}|\d{2}\/?\s?\d{2})(\.?\s?\d{2}){2})$";
////Console.WriteLine(Regex.IsMatch("0494386634", regex));
////DateOnly d = new(18, 11, 2022);
//TimeOnly begin = new(18, 30);
//TimeOnly eind = new(20, 00);
//TimeOnly res = new(19, 30);
//TimeOnly reseind = res.AddHours(1.5);
//if ((reseind > begin && reseind < eind) || (res > begin && res < eind)) Console.WriteLine("true");

//ILocatieRepository lr = new LocatieRepository();
//LocatieManager lm = new(lr);
//Locatie l = new(9160, "Lokeren", "Hillarestraat", "67");
//lm.VoegLocatieToe(l);
//var loc = lm.GeefLocatie(1);
//IGebruikerRepository gr = new GebruikerRepository();
//GebruikerManager gm = new(gr, lr);
//var exists = lr.BestaatLocatie(l);
//Locatie dbloc = lr.GeefLocatie(l);
//Gebruiker g = new("test", "test.test@student.hogent.be", "0494386635");
//g.Locatie = dbloc;
//var g2 = gm.GeefGebruiker(10);
//g2.ZetNaam("update");
////gm.GebruikerRegistreren(g);
//gm.GebruikerVerwijderen(g2);
//var gs = gm.GeefGebruikers();

//IRestaurantRepository rr = new RestaurantRepository();
//RestaurantManager rm = new(rr, lr);
//Locatie l2 = new(9160, "Lokeren", null, null);
//Restaurant r = new("Cardis", "cardis@test.be", "0498663258", Keuken.Belgisch);
//r.Locatie = l2;
//Restaurant r2 = rm.GeefRestaurant(1);
//r2.ZetNaam("Gouden Wok");
//rm.VerwijderRestaurant(r2);
//var rrr = rm.GeefRestaurants();
//Tafel t = new(2, 3);
//Tafel db = rm.GeefTafel(2, r2);
//db.ZetAantalPlaatsen(4);
//rm.UpdateTafel(db, r2);
//var tafels = rm.GeefAlleTafelsVanRestaurant(r2);

//IReservatieRepository resr = new ReservatieRepository();
//ReservatieManager resm = new(resr, rr, gr, lr);

// GeefGebruiker include reservaties
//var g = gm.GeefGebruiker(8);
// GeefRestaurant .include tafels, reservaties
//var r = rm.GeefRestaurant(1);
// GeefTafel include reservaties
//var t = rm.GeefTafel(1, r);
//Reservatie reservatie = new("12/12/2022", "18:30", 2);
//reservatie.ZetEinduur();
//reservatie.Gebruiker = g;
//reservatie.Restaurant = r;
//reservatie.Tafel = t;

//await resm.VoegReservatieToeAsync(reservatie);
//await resm.VoegReservatieToeAsync(reservatie);
//var res = await resm.GeefReservatieAsync(1);
//await resm.AnnuleerReservatieAsync(2);
//var res = await gm.ZoekReservatiesAsync(g, "10/12/2022", "11/12/2022");

// Expected: List met Gouden Wok
// result: gouden wok met tafel: tafelnummer 3
//var res = await rm.GeefRestaurantsMetVrijeTafelsAsync("11/12/2022 21:00", 3, 9160, "Chinees");
//var r = rm.GeefRestaurant(1);
//var res = await rm.GeefReservatiesRestaurantAsync(r, DateTime.Parse("12/12/2022"));

Console.WriteLine("end");
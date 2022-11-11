using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Interfaces;

public interface ILocatieRepository
{
    void VoegLocatieToe(Locatie locatie);
    bool BestaatLocatie(Locatie locatie);
    void UpdateLocatie(Locatie locatie);
    void VerwijderLocatie(Locatie locatie);
    Locatie GeefLocatie(Locatie locatie);
}
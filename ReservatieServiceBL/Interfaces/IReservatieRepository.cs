using ReservatieServiceBL.Entities;

namespace ReservatieServiceBL.Interfaces;

public interface IReservatieRepository
{
    void VoegReservatieToe(Reservatie reservatie);
    void AnnuleerReservatie(int reservatieNummer);
    void UpdateReservatie(Reservatie reservatie);
    Reservatie GeefReservatie(int reservatienummer);
    bool BestaatReservatie(Reservatie reservatie);
}
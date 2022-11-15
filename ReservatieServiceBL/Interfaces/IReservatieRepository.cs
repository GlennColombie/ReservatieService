using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Interfaces;

public interface IReservatieRepository
{
    void VoegReservatieToe(Reservatie reservatie);
    void DeleteReservatie(Reservatie reservatie);
    void UpdateReservatie(Reservatie reservatie);
    Reservatie GeefReservatie(int id);
    IEnumerable<Reservatie> ZoekReservaties(DateTime? begindatum, DateTime? einddatum);
    bool BestaatReservatie(Reservatie reservatie);
}
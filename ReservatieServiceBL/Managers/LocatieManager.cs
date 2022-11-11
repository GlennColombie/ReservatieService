using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Managers;

public class LocatieManager
{
    private ILocatieRepository _locatieRepository;

    public LocatieManager(ILocatieRepository locatieRepository)
    {
        _locatieRepository = locatieRepository;
    }

    public void VoegLocatieToe(Locatie locatie)
    {
        if (locatie == null) throw new LocatieManagerException("VoegLocatieToe - null");
        try
        {
            if (_locatieRepository.BestaatLocatie(locatie)) throw new LocatieManagerException("VoegLocatieToe - bestaat al");
            _locatieRepository.VoegLocatieToe(locatie);
        }
        catch (Exception ex)
        {
            throw new LocatieManagerException("VoegLocatieToe", ex);
        }
    }

    public void UpdateLocatie(Locatie locatie)
    {
        if (locatie == null) throw new LocatieManagerException("UpdateLocatie - null");
        try
        {
            if (!_locatieRepository.BestaatLocatie(locatie)) throw new LocatieManagerException("UpdateLocatie - bestaat niet");
            _locatieRepository.UpdateLocatie(locatie);
        }
        catch (Exception ex)
        {
            throw new LocatieManagerException("UpdateLocatie", ex);
        }
    }

    public void VerwijderLocatie(Locatie locatie)
    {
        if (locatie == null) throw new LocatieManagerException("VerwijderLocatie - null");
        try
        {
            if (!_locatieRepository.BestaatLocatie(locatie)) throw new LocatieManagerException("VerwijderLocatie - bestaat niet");
            _locatieRepository.VerwijderLocatie(locatie);
        }
        catch (Exception ex)
        {
            throw new LocatieManagerException("VerwijderLocatie", ex);
        }
    }
}
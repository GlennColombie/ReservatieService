using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;

namespace ReservatieServiceBL.Managers;

public class LocatieManager
{
    private ILocatieRepository _locatieRepository;

    public LocatieManager(ILocatieRepository locatieRepository)
    {
        _locatieRepository = locatieRepository;
    }

    public LocatieManager()
    {
    }

    public virtual void VoegLocatieToe(Locatie locatie)
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

    public virtual void UpdateLocatie(Locatie locatie)
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

    public virtual void VerwijderLocatie(Locatie locatie)
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

    public virtual Locatie GeefLocatie(Locatie locatie)
    {
        if (locatie == null) throw new LocatieManagerException("GeefLocatie - null");
        try
        {
            if (!_locatieRepository.BestaatLocatie(locatie)) _locatieRepository.VoegLocatieToe(locatie);
            return _locatieRepository.GeefLocatie(locatie);
        }
        catch (Exception ex)
        {
            throw new LocatieManagerException("GeefLocatie", ex);
        }
    }
}
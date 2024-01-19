using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class CountryRepository:ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Country> GetCountries()
    {
        return _context.Countries.ToList();
    }

    public Country GetCountry(int id)
    {
        return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
    }

    public Country GetCountryByOwner(int ownerId)
    {
        return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
    }

    public ICollection<Owner> GetOwnersFromACountry(int countryId)
    {
        return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
    }

    public bool CountryExists(int countryId)
    {
        return _context.Countries.Any(c => c.Id==countryId);
    }

    public bool CreateCountry(Country country)
    {
        _context.Add(country);
        return Save();
    }

    public bool UpdateCountry(Country country)
    {
        _context.Update(country);
        return Save();
    }

    public bool DeleteCountry(Country country)
    {
        _context.Remove(country);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}
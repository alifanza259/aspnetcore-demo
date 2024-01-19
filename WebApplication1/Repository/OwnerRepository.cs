using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class OwnerRepository: IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Owner> GetOwners()
    {
        return _context.Owners.ToList();
    }

    public Owner GetOwner(int id)
    {
        return _context.Owners.Where(o => o.Id == id).FirstOrDefault();
    }

    public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
    {
        return _context.PokemonOwners.Where(po => po.PokemonId == pokeId).Select(po => po.Owner).ToList();
    }

    public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
    {
        return _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToList();
    }

    public bool OwnerExists(int ownerId)
    {
        return _context.Owners.Any(o => o.Id == ownerId);
    }

    public bool CreateOwner(Owner owner)
    {
        _context.Add(owner);
        return Save();
    }

    public bool UpdateOwner(Owner owner)
    {
        _context.Update(owner);
        return Save();
    }

    public bool DeleteOwner(Owner owner)
    {
        _context.Remove(owner);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}
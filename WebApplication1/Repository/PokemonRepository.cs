using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class PokemonRepository:IPokemonRepository
{
     private readonly DataContext _context;
     
     // Constructor
     public PokemonRepository(DataContext context)
     {
          _context = context;
     }
     
     // methods
     public ICollection<Pokemon> GetPokemons()
     {
          return _context.Pokemon.OrderBy(p => p.Id).ToList();
     }

     public Pokemon GetPokemon(int id)
     {
          return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
     }

     public Pokemon GetPokemon(string name)
     {
          return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
     }

     public decimal GetPokemonRating(int pokeId)
     {
          var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

          if (review.Count() <= 0) return 0;

          return (decimal)review.Sum(r => r.Rating) / review.Count();
     }

     public bool PokemonExists(int pokeId)
     {
          return _context.Pokemon.Any(p => p.Id == pokeId);
     }

     public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
     {
          var pokemonOwnerEntity = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
          var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

          var pokemonOwner = new PokemonOwner()
          {
               Owner = pokemonOwnerEntity,
               Pokemon = pokemon
          };

          // TODO: Kind of transaction(?) Need to validate. 
          _context.Add(pokemonOwner);

          var pokemonCategory = new PokemonCategory()
          {
               Category = category,
               Pokemon = pokemon
          };
          _context.Add(pokemonCategory);
          _context.Add(pokemon);
          
          return Save();
     }

     public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
     {
          _context.Update(pokemon);
          return Save();
     }

     public bool DeletePokemon(Pokemon pokemon)
     {
          _context.Remove(pokemon);
          return Save();
     }

     public bool Save()
     {
          var saved = _context.SaveChanges();
          return saved > 0;
     }
}
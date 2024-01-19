using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController:Controller
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(pokemons);
    }

    [HttpGet("{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int pokeId)
    {
        if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();

        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        return Ok(pokemon);
    }

    [HttpGet("{pokeId}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonRating(int pokeId)
    {
        if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();

        var rating = _pokemonRepository.GetPokemonRating(pokeId);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(rating);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromQuery] int ownerId, [FromQuery] int categoryId,
        [FromBody] PokemonDto pokemonCreate)
    {
        if (pokemonCreate == null) return BadRequest(ModelState);
    
        var pokemons = _pokemonRepository.GetPokemons()
            .Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
            .FirstOrDefault();

        if (pokemons != null)
        {
            ModelState.AddModelError("", "Owner already exist");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
        if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
        {
            ModelState.AddModelError("","Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdatePokemon([FromQuery] int ownerId,[FromQuery] int categoryId, int pokemonId, [FromBody] PokemonDto pokemonUpdate)
    {
        if (pokemonUpdate == null) return BadRequest();
        if (_pokemonRepository.PokemonExists(pokemonId) == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (pokemonUpdate.Id != pokemonId) return BadRequest();

        var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);
        if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
        {
            ModelState.AddModelError("","Something's wrong when updating pokemon");
        }
        
        return NoContent();
    }
    
    [HttpDelete("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeletePokemon(int pokemonId)
    {

        var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);
        if (pokemonToDelete == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
        {
            ModelState.AddModelError("","something went wrong when deleting pokemon");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
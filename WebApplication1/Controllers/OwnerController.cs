using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController:Controller
{
    private readonly IMapper _mapper;
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;

    public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Owner))]
    public IActionResult GetOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int ownerId)
    {
        var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));
        if (owner == null) return NotFound();

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(owner);
    }

    [HttpGet("pokemon/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwnerOfAPokemon(int pokeId)
    {
        var owner = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(pokeId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(owner);
    }

    [HttpGet("{ownerId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

        var pokemon = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(pokemon);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
    {
        if (ownerCreate == null || countryId == 0) return BadRequest(ModelState);
        
        var owner = _ownerRepository.GetOwners()
            .Where(c => c.Name.Trim().ToLower() == ownerCreate.Name.Trim().ToLower())
            .FirstOrDefault();
        if (owner != null)
        {
            ModelState.AddModelError("","Owner already exist");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ownerMap= _mapper.Map<Owner>(ownerCreate);
        ownerMap.Country = _countryRepository.GetCountry(countryId);
        if (!_ownerRepository.CreateOwner(ownerMap))
        {
            ModelState.AddModelError("","Error while creating owner");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Success");
    }
    
    [HttpPut("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto ownerUpdate)
    {
        if (ownerUpdate == null) return BadRequest();
        if (_ownerRepository.OwnerExists(ownerId) == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (ownerUpdate.Id != ownerId) return BadRequest();

        var ownerMap = _mapper.Map<Owner>(ownerUpdate);
        if (!_ownerRepository.UpdateOwner(ownerMap))
        {
            ModelState.AddModelError("","Something's wrong when updating owner");
        }
        
        return NoContent();
    }
    
    [HttpDelete("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOwner(int ownerId)
    {

        var ownerToDelete = _ownerRepository.GetOwner(ownerId);
        if (ownerToDelete == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_ownerRepository.DeleteOwner(ownerToDelete))
        {
            ModelState.AddModelError("","something went wrong when deleting owner");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
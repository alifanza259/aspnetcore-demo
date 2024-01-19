using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController:Controller
{
    private readonly IMapper _mapper;
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Country))]
    public IActionResult GetCountries()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(countries);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("owner/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountryByOwner(int ownerId)
    {
        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("{countryId}/owner")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    public IActionResult GetOwnersFromACountry(int countryId)
    {
        var owners = _mapper.Map<List<Owner>>(_countryRepository.GetOwnersFromACountry(countryId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
    {
        if (countryCreate == null) return BadRequest(ModelState);

        var country = _countryRepository.GetCountries()
            .Where(c => c.Name.Trim().ToLower() == countryCreate.Name.Trim().ToLower())
            .FirstOrDefault();
        if (country != null)
        {
            ModelState.AddModelError("","Country already exist");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var countryMap = _mapper.Map<Country>(countryCreate);

        if (!_countryRepository.CreateCountry(countryMap))
        {
            ModelState.AddModelError("","Error while creating country");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Success");
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate)
    {
        if (countryUpdate == null) return BadRequest();
        if (_countryRepository.CountryExists(countryId) == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (countryUpdate.Id != countryId) return BadRequest();

        var countryMap = _mapper.Map<Country>(countryUpdate);
        if (!_countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("","Something's wrong when updating country");
        }
        
        return NoContent();
    }
    
    [HttpDelete("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCountry(int countryId)
    {

        var countryToDelete = _countryRepository.GetCountry(countryId);
        if (countryToDelete == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_countryRepository.DeleteCountry(countryToDelete))
        {
            ModelState.AddModelError("","something went wrong when deleting country");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
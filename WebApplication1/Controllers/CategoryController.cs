using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController:Controller
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Category))]
    public IActionResult GetCategories()
    {
        var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    public IActionResult GetCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }
        var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(category);
    }

    [HttpGet("pokemon/{categoryId}")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByCategoryId(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }

        var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(categoryId));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
    {
        if (categoryCreate == null) return BadRequest(ModelState);

        var category = _categoryRepository.GetCategories()
            .Where(c => c.Name.Trim().ToLower() == categoryCreate.Name.TrimEnd().ToLower())
            .FirstOrDefault();

        if (category != null)
        {
            ModelState.AddModelError("", "Category already exist");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var categoryMap = _mapper.Map<Category>(categoryCreate);
        if (!_categoryRepository.CreateCategory(categoryMap))
        {
            ModelState.AddModelError("","Something went wrong while creating category");
            return StatusCode(500, ModelState);
        }

        return Ok("Success");
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryUpdate)
    {
        if (categoryUpdate == null) return BadRequest(ModelState);

        if (categoryId != categoryUpdate.Id) return BadRequest(ModelState);

        if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var categoryMap = _mapper.Map<Category>(categoryUpdate);

        if (!_categoryRepository.UpdateCategory(categoryMap))
        {
            ModelState.AddModelError("","something wrong when updating category");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int categoryId)
    {

        var categoryToDelete = _categoryRepository.GetCategory(categoryId);
        if (categoryToDelete == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_categoryRepository.DeleteCategory(categoryToDelete))
        {
            ModelState.AddModelError("","something went wrong when deleting category");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
}
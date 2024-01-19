using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IPokemonRepository _pokemonRepository;

    public ReviewController(IReviewRepository reviewRepository, IPokemonRepository pokemonRepository,
        IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _pokemonRepository = pokemonRepository;
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Review))]
    public IActionResult GetReviews()
    {
        var reviews = _mapper.Map<List<Review>>(_reviewRepository.GetReviews());

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(reviews);
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    public IActionResult GetReview(int reviewId)
    {
        var review = _mapper.Map<Review>(_reviewRepository.GetReview(reviewId));
        if (review == null) return NotFound();

        return Ok(review);
    }

    [HttpGet("pokemon/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    public IActionResult GetReviewsOfAPokemon(int pokeId)
    {
        var reviews = _mapper.Map<List<Review>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromBody] ReviewDto reviewCreate, [FromQuery] int reviewerId,
        [FromQuery] int pokemonId)
    {
        if (reviewCreate == null) return BadRequest(ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reviewMap = _mapper.Map<Review>(reviewCreate);
        reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
        reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
        if (!_reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while creating review");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
    {
        if (updatedReview == null)
            return BadRequest(ModelState);

        if (reviewId != updatedReview.Id)
            return BadRequest(ModelState);

        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewMap = _mapper.Map<Review>(updatedReview);

        if (!_reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong updating review");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
    [HttpDelete("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
        var reviewToDelete = _reviewRepository.GetReview(reviewId);
        if (reviewToDelete == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_reviewRepository.DeleteReview(reviewToDelete))
        {
            ModelState.AddModelError("","something went wrong when deleting review");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController:Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    public IActionResult GetReviewers()
    {
        var reviewers = _mapper.Map<List<Reviewer>>(_reviewerRepository.GetReviewers());

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(reviewers);
    }

    [HttpGet("{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewer(int reviewerId)
    {
        var reviewer = _mapper.Map<Reviewer>(_reviewerRepository.GetReviewer(reviewerId));
        if (reviewer == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(reviewer);
    }

    [HttpGet("{reviewerId}/review")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewsByReviewer(int reviewerId)
    {
        if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();

        var reviews = _mapper.Map<List<Review>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(reviews);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
    {
        if (reviewerCreate == null) return BadRequest(ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
        if (!_reviewerRepository.CreateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong while creating reviewer");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{reviewerId}")]
    [ProducesResponseType(204)] 
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerUpdate)
    {
        if (reviewerUpdate== null) return BadRequest();
        if (_reviewerRepository.ReviewerExists(reviewerId) == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (reviewerUpdate.Id != reviewerId) return BadRequest();

        var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);
        if (!_reviewerRepository.UpdateReviewer(reviewerMap))
        {
            ModelState.AddModelError("","Something's wrong when updating reviewer");
        }
        
        return NoContent();
    }
    
    [HttpDelete("{reviewerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReviewer(int reviewerId)
    {

        var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
        if (reviewerToDelete == null) return NotFound();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
        {
            ModelState.AddModelError("","something went wrong when deleting reviewer");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
}
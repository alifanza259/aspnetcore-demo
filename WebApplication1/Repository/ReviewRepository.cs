using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class ReviewRepository:IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Review> GetReviews()
    {
        return _context.Reviews.ToList();
    }

    public Review GetReview(int reviewId)
    {
        return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
    }

    public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
    {
        return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
    }

    public bool ReviewExists(int reviewId)
    {
        return _context.Reviews.Any(r => r.Id == reviewId);
    }

    public bool CreateReview(Review review)
    {
        _context.Add(review);
        return Save();
    }

    public bool UpdateReview(Review review)
    {
        _context.Add(review);
        return Save();
    }

    public bool DeleteReview(Review review)
    {
        _context.Remove(review);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0; 
    }
}
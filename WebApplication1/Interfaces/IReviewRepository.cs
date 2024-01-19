using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface IReviewRepository
{
    ICollection<Review> GetReviews();

    Review GetReview(int reviewId);

    ICollection<Review> GetReviewsOfAPokemon(int pokeId);
    
    bool ReviewExists(int reviewId);

    bool CreateReview(Review review);

    bool UpdateReview(Review review);
    bool DeleteReview(Review review);
    bool Save();
}
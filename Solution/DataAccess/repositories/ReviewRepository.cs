using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.repositories;

public class ReviewRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;
    
    public ReviewRepository(IDbContextFactory<DataAccessContext> dataAccessContext)
    {
        _contextFactory = dataAccessContext;
    }
    
    public Review InsertReview(Review newReview)
    {
        using var context = _contextFactory.CreateDbContext();
        var review = context.Reviews.Add(newReview);
        context.SaveChanges();
        return review.Entity;
    }
    
    public List<Review> GetDepositReviews(int depositId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Reviews.Where(n => n.DepositId == depositId).ToList();
    }
    
}
using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.repositories;

public class PromotionRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;

    public PromotionRepository(IDbContextFactory<DataAccessContext> context)
    {
        _contextFactory = context;
    }

    public Promotion InsertPromotion(Promotion newPromotion)
    {
        using var context = _contextFactory.CreateDbContext();
        var promotion = context.Promotions.Add(newPromotion);
        context.SaveChanges();
        return promotion.Entity;
    }

    public void UpdatePromotion(string? tag, int discountPercentage, DateTime startDate, DateTime endDate,
        int promotionId)
    {
        using var context = _contextFactory.CreateDbContext();
        try
        {
            new Promotion(tag, discountPercentage, startDate, endDate);
            var promotion = GetPromotion(promotionId);
            promotion.Tag = tag;
            promotion.DiscountPercentage = discountPercentage;
            promotion.StartDate = startDate;
            promotion.EndDate = endDate;
            context.Promotions.Update(promotion);
            context.SaveChanges();
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
    }

    public void DeletePromotion(Promotion promotion)
    {
        using var context = _contextFactory.CreateDbContext();
        if (promotion == null) throw new NullReferenceException();
        context.Promotions.Remove(promotion);
        context.SaveChanges();
    }

    public Promotion GetPromotion(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        var promotion = context.Promotions.Find(id);
        if (promotion != null)
        {
            return promotion;
        }

        throw new NullReferenceException();
    }

    public List<Promotion> GetPromotions()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Promotions.ToList();
    }

    public bool PromotionIsAplyingToAnyDeposit(int promotionId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Set<Dictionary<string, object>>("DepositPromotion")
            .Any(dp => dp["PromotionId"].Equals(promotionId));
    }
    

    public List<Promotion> GetPromotionsInDeposit(int depositId)
    {
        using var context = _contextFactory.CreateDbContext();

        var promotionIds = context.Set<Dictionary<string, object>>("DepositPromotion")
            .Where(dp => dp["DepositId"].Equals(depositId))
            .Select(dp => (int)dp["PromotionId"])
            .ToList();
        var promotions = new List<Promotion>();
        foreach (var promotionId in promotionIds)
        {
            var promotion = GetPromotion(promotionId);
            promotions.Add(promotion);
        }
        return promotions;
    }


    public void AddPromotionToDeposit(int depositId, int promotionId)
    {
        using var context = _contextFactory.CreateDbContext();
        var depositPromotion = new Dictionary<string, object>
        {
            { "PromotionId", promotionId },
            { "DepositId", depositId }
        };
        context.Set<Dictionary<string, object>>("DepositPromotion").Add(depositPromotion);
        context.SaveChanges();
    }
}
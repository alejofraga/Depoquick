namespace BusinessLogic;

public class PromotionDisponibilityDto
{
    public PromotionDisponibilityDto()
    {
        
    }
    public PromotionDisponibilityDto(string? setTag, int setDiscountPercentage)
    {
        Tag = setTag;
        DiscountPercentage = setDiscountPercentage;
    }

    public string? Tag { get; set; }
    public int DiscountPercentage { get; set; }
}
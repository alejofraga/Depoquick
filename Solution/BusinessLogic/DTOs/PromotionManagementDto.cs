namespace BusinessLogic;

public class PromotionManagementDto
{
    public PromotionManagementDto()
    {
        
    }
    
    public int Id { get; set; }
    public string Tag { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DiscountPercentage { get; set; }
    
    public PromotionManagementDto(int id, string tag, DateTime startDate, DateTime endDate, int discountPercentage)
    {
        Id = id;
        Tag = tag;
        StartDate = startDate;
        EndDate = endDate;
        DiscountPercentage = discountPercentage;
    }
}
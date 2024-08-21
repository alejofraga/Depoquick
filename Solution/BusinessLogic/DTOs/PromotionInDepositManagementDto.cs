namespace BusinessLogic;

public class PromotionInDepositManagementDto
{
    public PromotionInDepositManagementDto()
    {
        
    }
    
    public PromotionInDepositManagementDto(int id, string tag)
    {
        Id = id;
        Tag = tag;
    }
    
    public int Id { get; set; }
    public string Tag { get; set; }
}
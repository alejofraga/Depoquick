namespace BusinessLogic;

public class DepositManagementDto
{
    public DepositManagementDto()
    {
        
    }
    
    public DepositManagementDto(int id, char area, string size, bool conditioning, List<PromotionInDepositManagementDto> promotions)
    {
        Id = id;
        Area = area;
        Size = size;
        Conditioning = conditioning;
        Promotions = promotions;
    }
    
    public string Name { get; set; }
    public int Id { get; set; }
    public char Area { get; set; }
    public string Size { get; set; }
    public bool Conditioning { get; set; }
    public List<PromotionInDepositManagementDto> Promotions { get; set; }
    
}
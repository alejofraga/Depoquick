namespace BusinessLogic;

public class DepositDisponibilityDto
{
    public DepositDisponibilityDto()
    {
        DateRangesList = new List<DateRangeDto>();
        PromotionDtoList = new List<PromotionDisponibilityDto>();
    }
    
    public DepositDisponibilityDto (string? expectedDepositDtoName,
        char expectedDepositDtoArea,
        string expectedDepositDtoSize, 
        bool expectedDepositDtoConditioning, 
        bool expectedDepositHasPromotions, 
        bool expectedAreDisponibilityRangeRegistered)
    {
        Name = expectedDepositDtoName;
        Area = expectedDepositDtoArea;
        Size = expectedDepositDtoSize;
        Conditioning = expectedDepositDtoConditioning;
        HasPromotions = expectedDepositHasPromotions;
        AreDisponibilityRangesRegistered = expectedAreDisponibilityRangeRegistered;
        DateRangesList = new List<DateRangeDto>();
        PromotionDtoList = new List<PromotionDisponibilityDto>();
    }
    
    public string? Name { get; set; }
    public char Area { get; set; }
    public string Size { get; set; }
    public bool Conditioning { get; set; }
    public bool HasPromotions { get; set; }
    public bool AreDisponibilityRangesRegistered { get; set; }

    public List<PromotionDisponibilityDto> PromotionDtoList { get; set; }
    public List<DateRangeDto> DateRangesList { get; set; }
    
}
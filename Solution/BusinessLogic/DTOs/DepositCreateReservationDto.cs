namespace BusinessLogic;

public class DepositCreateReservationDto
{
    public DepositCreateReservationDto(int incommingDepositId,
        char incommingDepositArea,
        string incommingDepositSize,
        bool incommingDepositConditionig,
        List<PromotionInDepositCreateReservationDto> incommingPromotionList,
        List<ReviewInDepositCreateReservationDto> incommingReviewList,
        bool incommingContainsAnyReviews,
        bool incommingHasPromotions)
    {
         Id = incommingDepositId;
         Area = incommingDepositArea;
         Size = incommingDepositSize;
         Conditionig = incommingDepositConditionig;
         PromotionDtoList = incommingPromotionList;
         ReviewDtoList = incommingReviewList;
         ContainsAnyReviews = incommingContainsAnyReviews;
         HasPromotions = incommingHasPromotions;
    }

    public DepositCreateReservationDto()
    {
        
    }
    
    public int Id  { get; set; }
    
    public string Name { get; set; }
    public char Area { get; set; }
    public string Size { get; set; }
    public bool Conditionig  { get; set; }
    public List<PromotionInDepositCreateReservationDto> PromotionDtoList  { get; set; }
    public List<ReviewInDepositCreateReservationDto> ReviewDtoList { get; set; }
    public bool ContainsAnyReviews  { get; set; }
    public bool HasPromotions  { get; set; }
}
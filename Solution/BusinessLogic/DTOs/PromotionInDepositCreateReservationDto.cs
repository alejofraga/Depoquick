namespace BusinessLogic;

public class PromotionInDepositCreateReservationDto
{
    public PromotionInDepositCreateReservationDto(string incommingTag, int incommingDiscountPercentage)
    {
        Tag = incommingTag;
        DiscountPercentage = incommingDiscountPercentage;
    }

    public PromotionInDepositCreateReservationDto()
    {
    }

    public string Tag { get; set; }
    public int DiscountPercentage { get; set; }

}
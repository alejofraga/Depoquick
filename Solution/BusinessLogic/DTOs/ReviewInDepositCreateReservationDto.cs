namespace BusinessLogic;

public class ReviewInDepositCreateReservationDto
{
    public ReviewInDepositCreateReservationDto(int incommingValoration, string incommingComment)
    {
        Valoration = incommingValoration;
        Comment = incommingComment;
    }
    public ReviewInDepositCreateReservationDto()
    {
        
    }

    public string Comment { get; set; }
    public int Valoration { get; set; }
}

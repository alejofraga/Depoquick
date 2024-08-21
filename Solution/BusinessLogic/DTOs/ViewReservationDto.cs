namespace BusinessLogic;

public class ViewReservationDto
{
    
    public ViewReservationDto()
    {
    }
    
    public ViewReservationDto(DateTime startDate, DateTime endDate, bool isConfirmed, bool isRejected,
        int depositId, bool isReviewed, int id, string depositName,string rejectedMessage, int paymentStatus)
    {
        StartDate = startDate;
        EndDate = endDate;
        IsConfirmed = isConfirmed;
        IsRejected = isRejected;
        DepositId = depositId;
        IsReviewed = isReviewed;
        Id = id;
        DepositName = depositName;
        RejectedMessage = rejectedMessage;
        PaymentStatus = paymentStatus;
    }
    
    public DateTime StartDate { set; get; }
    public DateTime EndDate { set; get; }
    public bool IsConfirmed { set; get; }
    public bool IsRejected { set; get; }
    public int DepositId { set; get; }
    
    public string DepositName { set; get; }
    public bool IsReviewed { set; get; }
    public int Id { set; get; }
    public string RejectedMessage { set; get; }
    
    public int PaymentStatus { get; set; }

}
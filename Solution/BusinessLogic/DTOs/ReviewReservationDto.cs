namespace BusinessLogic;

public class ReviewReservationDto
{
    public ReviewReservationDto ()
    {
        
    }
    
    public ReviewReservationDto(int reservationId, int depositId,string depositName,DateTime startDate, DateTime endDate, float cost)
    {
        ReservationId = reservationId;
        DepositId = depositId;
        StartDate = startDate;
        EndDate = endDate;
        Cost = cost;
        DepositName = depositName;
    }
    
    public int ReservationId { get; set; }
    public int DepositId { get; set; }
    
    public string DepositName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public float Cost { get; set; }
    
}
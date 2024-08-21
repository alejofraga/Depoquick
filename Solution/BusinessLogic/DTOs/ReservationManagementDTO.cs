namespace BusinessLogic;

public class ReservationManagementDTO
{
    
    public int DepositId { get; set; }
    
    public string DepositName { get; set; }
    public int ReservationId { get; set; }
    public string ClientName { get; set; }
    public string ClientMail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public ReservationManagementDTO(int depositId, int reservationId,string depositName, string clientName, string clientMail, DateTime startDate, DateTime endDate)
    {
        DepositId = depositId;
        ReservationId = reservationId;
        ClientName = clientName;
        ClientMail = clientMail;
        StartDate = startDate;
        EndDate = endDate;
        DepositName = depositName;
    }
    public ReservationManagementDTO()
    {
    }
}
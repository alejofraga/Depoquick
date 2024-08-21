namespace BusinessLogic;

public class NotificationsDto
{
    public NotificationsDto()
    {
        
    }
    
    public string ClientEmail { get; set; }
    
    public int Id { get; set; }
    public int NotificationType { get; set; }
    public string DepositName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public NotificationsDto(string clientEmail, int notificationType, string depositName, DateTime startDate, DateTime endDate)
    {
        ClientEmail = clientEmail;
        NotificationType = notificationType;
        DepositName = depositName;
        StartDate = startDate;
        EndDate = endDate;
    }
    
}
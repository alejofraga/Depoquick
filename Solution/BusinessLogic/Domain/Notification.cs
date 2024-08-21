namespace BusinessLogic.Domain;

public class Notification
{
    public Notification()
    {
        
    }
    public Client Client { get; set; }
    
    public string ClientEmail { get; set; }
    
    public int Id { get; set; }
    public int NotificationType { get; set; }
    public string DepositName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsValidNotificationType(int type)
    {
        return type == 0 || type == 1;
    }
    
    public Notification(int notificationType, string depositName, DateTime startDate, DateTime endDate)
    {
        if (!IsValidNotificationType(notificationType))
        {
            throw new ArgumentException();
        }
        NotificationType = notificationType;
        DepositName = depositName;
        StartDate = startDate;
        EndDate = endDate;
    }
}
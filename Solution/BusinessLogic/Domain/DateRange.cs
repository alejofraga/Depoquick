namespace BusinessLogic.Domain;

public class DateRange
{
    public DateRange()
    {
        
    }
    
    public DateRange(DateTime setStartDate, DateTime setEndDate)
    {
        StartDate = setStartDate;
        EndDate = setEndDate;
    }
    
    public int Id { get; set; }
    
    public int DepositId { get; set; }
    
    public Deposit Deposit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}
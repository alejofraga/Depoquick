namespace BusinessLogic;

public class DateRangeDto
{


    public DateRangeDto()
    {
        
    }
    
    public DateRangeDto(DateTime setStartDate, DateTime setEndDate)
    {
        StartDate = setStartDate;
        EndDate = setEndDate;
    }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}

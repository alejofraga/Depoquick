using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;
using ArgumentException = System.ArgumentException;

namespace DataLayer.repositories;

public class DepositRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;

    public DepositRepository(IDbContextFactory<DataAccessContext> dataAccessContext)
    {
        _contextFactory = dataAccessContext;
    }

    public Deposit InsertDeposit(Deposit newDeposit)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var deposit = context.Deposits.Add(newDeposit);
            context.SaveChanges();
            return deposit.Entity;
        }
        catch (DbUpdateException)
        {
            throw new ArgumentException();
        }
    }

    public void DeleteDeposit(Deposit deposit)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Deposits.Remove(deposit);
        context.SaveChanges();
    }

    public void AddDisponibilityDateRangeToDeposit(int depositId, DateTime startDate, DateTime endDate)
    {
        if (!DateRangeIsContainedDepositDisponibilityList(startDate, endDate, depositId))
        {
            using var context = _contextFactory.CreateDbContext();

            if (startDate >= endDate)
            {
                throw new ArgumentException("La fecha de inicio debe ser antes que la fecha de fin");
            }
            if (startDate < DateTime.Today)
            {
                throw new ArgumentException("La fecha de inicio no puede estar en el pasado");
            }

            var dateRange = new DateRange
            {
                DepositId = depositId,
                StartDate = startDate,
                EndDate = endDate
            };

            context.DateRanges.Add(dateRange);
            context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("El rango de fechas ya está definido");
        }
    }
    
    public List<Deposit> GetDepositsAvailableBetweenDates(DateTime startDate, DateTime endDate)
    {
        using var context = _contextFactory.CreateDbContext();
        var deposits = context.Deposits.ToList();
        var depositsAvailable = new List<Deposit>();
        foreach (var deposit in deposits)
        {
            if (IsAvailableInRange(startDate, endDate,deposit.Id))
            {
                depositsAvailable.Add(deposit);
            }
        }

        return depositsAvailable;
    }

    public bool DateRangeIsContainedDepositDisponibilityList(DateTime disponibilityStartDate,
        DateTime disponibilityEndDate, int depositId)
    {
        var disponibilityDateRangeList = GetDepositDisponibility(depositId);
        bool isDefinedOrContained = disponibilityDateRangeList.Any(dateRange =>
            dateRange.StartDate <= disponibilityStartDate && dateRange.EndDate >= disponibilityEndDate);

        return isDefinedOrContained;
    }
    
    public bool IsAvailableInRange(DateTime availableFromStartDate,DateTime availableToEndDate, int depositId)
    {
        var _disponibilityDateRangeList = GetDepositDisponibility(depositId);
        foreach (var dateRange in _disponibilityDateRangeList)
        {
            if ((availableFromStartDate >= dateRange.StartDate && availableFromStartDate < dateRange.EndDate) &&
                (availableToEndDate <= dateRange.EndDate && availableToEndDate > availableFromStartDate)
               )
            {
                return true;
            }
        }
        
        return false;
    }
    
    
    public void RemoveDisponibilityDateRangeToDeposit(DateTime fromStartDate, DateTime toEndDate, int depositId)
    {
        var context = _contextFactory.CreateDbContext();

        var depositDisponibility = GetDepositDisponibility(depositId);

        var dateRangeToRemove = depositDisponibility.FirstOrDefault(dr =>
            dr.StartDate <= fromStartDate && dr.EndDate >= toEndDate);

        if (dateRangeToRemove != null)
        {
            var existingDateRangeStart = dateRangeToRemove.StartDate;
            var existingDateRangeEnd = dateRangeToRemove.EndDate;

            context.DateRanges.Remove(dateRangeToRemove);

            if (existingDateRangeStart < fromStartDate)
            {
                context.DateRanges.Add(new DateRange
                {
                    DepositId = depositId,
                    StartDate = existingDateRangeStart,
                    EndDate = fromStartDate
                });
            }

            if (existingDateRangeEnd > toEndDate)
            {
                context.DateRanges.Add(new DateRange
                {
                    DepositId = depositId,
                    StartDate = toEndDate,
                    EndDate = existingDateRangeEnd
                });
            }

            context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("The specified reservation range does not exist.");
        }
    }


    public bool DepositHasDisponibility(int depositId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.DateRanges.Any(d => d.DepositId == depositId);
        
    }

    public List<DateRange> GetDepositDisponibility(int depositId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.DateRanges.Where(d => d.DepositId == depositId).ToList();
    }

    public List<Deposit> GetDeposits()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Deposits.ToList();
    }

    public Deposit GetDeposit(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Deposits.Find(id);
    }

    public Deposit? GetDepositByName(string name)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Deposits.FirstOrDefault(d => d.Name == name);
    }
}
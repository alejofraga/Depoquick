using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.repositories;

public class ReservationRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;
    
    public ReservationRepository(IDbContextFactory<DataAccessContext> dataAccessContext)
    {
        _contextFactory = dataAccessContext;
    }

    public void MarkReservationAdReviewed(int reservationId)
    {
        using var context = _contextFactory.CreateDbContext();
        var reservation = GetReservation(reservationId);
        reservation.IsReviewed = true;
        context.Reservations.Update(reservation);
        context.SaveChanges();
    }
    

    public List<Reservation> GetClientReservations(string email)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Reservations.Where(r => r.ClientEmail == email).ToList();
    }
    
    public void InsertReservation(Reservation newReservation)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Reservations.Add(newReservation);
        context.SaveChanges();
    }

    public void ConfirmReservation(Reservation reservation)
    {
        using var context = _contextFactory.CreateDbContext();
        reservation.IsConfirmed = true;
        reservation.PaymentStatus = PaymentStatus.Paid;
        context.Reservations.Update(reservation);
        context.SaveChanges();
    }
    
    public void RejectReservation(Reservation reservation, string reason)
    {
        using var context = _contextFactory.CreateDbContext();
        reservation.IsRejected = true;
        reservation.PaymentStatus = PaymentStatus.NoPayment;
        reservation.RejectedMessage = reason;
        context.Reservations.Update(reservation);
        context.SaveChanges();
    }
    

    public Reservation GetReservation(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Reservations.Find(id);
    }

    public List<Reservation> GetConfirmedReservations()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Reservations.Where(r => r.IsConfirmed).ToList();
    }
    
    public List<Reservation> GetReservations()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Reservations.ToList();
    }
}
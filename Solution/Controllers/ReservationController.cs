using BusinessLogic;
using BusinessLogic.Domain;
using Controllers.ReservationExporter;
using DataLayer.repositories;

namespace Controllers;

public class ReservationController
{
    public ReservationRepository ReservationRepository { get; }
    public DepositRepository DepositRepository { get; }
    public ClientRepository ClientRepository { get; }

    public PromotionRepository PromotionRepository { get; }

    public ReservationController(ReservationRepository reservationRepository, DepositRepository depositRepository,
        ClientRepository clientRepository, PromotionRepository promotionRepository)
    {
        ReservationRepository = reservationRepository;
        DepositRepository = depositRepository;
        ClientRepository = clientRepository;
        PromotionRepository = promotionRepository;
    }


    public void CreateReservationWithDepositCreateReservationDto(ClientDto client, DepositCreateReservationDto deposit,
        DateTime startDate, DateTime endDate)
    {
        var auxClient = GetClient(client.Email);
        var auxDeposit = GetDepositById(deposit.Id);
        CreateReservation(auxClient, auxDeposit, startDate, endDate);
    }

    public Deposit GetDepositById(int expectedId)
    {
        var depositFromDb = DepositRepository.GetDeposit(expectedId);
        if (depositFromDb == null)
        {
            throw new NullReferenceException();
        }
        return depositFromDb;
    }

    public int GetAmountOfConfirmedReservationsByDepositArea(char area)
    {
        var reservations = GetConfirmedReservations();
        var total = 0;
        foreach (var reservation in reservations)
        {
            var deposit = DepositRepository.GetDeposit(reservation.DepositId);
            if (deposit.Area == area)
            {
                total++;
            }
        }

        return total;
    }

    public float GetMoneyGeneratedBetweenDatesByDepositArea(char area, DateTime startDate, DateTime endDate)
    {
        var reservations = GetConfirmedReservations();
        var total = 0f;
        foreach (var reservation in reservations)
        {
            var deposit = DepositRepository.GetDeposit(reservation.DepositId);

            if (deposit.Area == area && reservation.StartDate >= startDate &&
                reservation.StartDate <= endDate)
            {
                total += CalculateReservationCost(deposit, reservation.StartDate, reservation.EndDate);
            }
        }

        return total;
    }
    
    public void ExportReservation(string format)
    {
        var reservations = GetReservations();
        Exporter exporter = Exporter.CreateExporter(format, DepositRepository);
        exporter.Export(reservations);
    }
    
    public Client GetClient(string email)
    {
        return ClientRepository.GetClient(email);
    }

    public bool ClientIsRegistered(Client client)
    {
        return GetClients().Contains(client);
    }

    public List<Deposit> GetDeposits()
    {
        return DepositRepository.GetDeposits();
    }

    private bool DepositIsRegistered(Deposit? deposit)
    {
        return GetDeposits().Contains(deposit);
    }

    public List<Client> GetClients()
    {
        return ClientRepository.GetClients();
    }

    public void CreateReservation(Client client, Deposit deposit, DateTime startDate, DateTime endDate)
    {
        if (!ClientIsRegistered(client))
        {
            throw new ArgumentException("El cliente no esta registrado");
        }

        if (!DepositIsRegistered(deposit))
        {
            throw new ArgumentException("El depósito no esta registrado");
        }


        if (DepositAlreadyReservedByClientInDate(deposit, startDate, client))
        {
            throw new ArgumentException("Ya has realizado una reserva en esa fecha.");
        }


        var newReservation = new Reservation(startDate, endDate, false, false)
        {
            DepositId = deposit.Id,
            ClientEmail = client.Email,
            PaymentStatus = PaymentStatus.Pending
        };
        ReservationRepository.InsertReservation(newReservation);
    }

    private bool DepositAlreadyReservedByClientInDate(Deposit deposit, DateTime startDate, Client client)
    {
        var reservations = GetClientReservations(client);
        foreach (var reservation in reservations)
        {
            if (reservation.DepositId == deposit.Id &&
                (startDate >= reservation.StartDate && startDate <= reservation.EndDate))
            {
                return true;
            }
        }

        return false;
    }

    public List<Reservation> GetClientReservations(Client client)
    {
        return ReservationRepository.GetClientReservations(client.Email);
    }

    private bool IsPromotionAplicable(Promotion promotion, DateTime startDate)
    {
        return startDate >= promotion.StartDate && startDate <= promotion.EndDate;
    }

    private int CalculateExtraCostByConditioning(Deposit deposit)
    {
        return deposit.Conditioning ? 20 : 0;
    }

    private int CalculateCostBySize(Deposit deposit)
    {
        return deposit.Size switch
        {
            "Pequeño" => 50,
            "Mediano" => 75,
            "Grande" => 100,
        };
    }

    private int CalculateDiscountByDays(DateTime startDate, DateTime endDate)
    {
        var duration = (endDate - startDate).Days;
        return duration switch
        {
            > 14 => 10,
            >= 7 => 5,
            _ => 0
        };
    }


    public float CalculateReservationCostWithDepositCreateReservationDto(DepositCreateReservationDto depositDto,
        DateTime startDate, DateTime endDate)
    {
        var auxDeposit = GetDepositById(depositDto.Id);
        return CalculateReservationCost(auxDeposit, startDate, endDate);
    }

    public float CalculateReservationCost(Deposit deposit, DateTime startDate, DateTime endDate)
    {
        var duration = (endDate - startDate).Days;
        var promotions = PromotionRepository.GetPromotionsInDeposit(deposit.Id);
        var totalDiscount = 0;
        foreach (var promotion in promotions)
        {
            if (IsPromotionAplicable(promotion, startDate))
            {
                totalDiscount += promotion.DiscountPercentage;
            }
        }

        totalDiscount += CalculateDiscountByDays(startDate, endDate);
        if (totalDiscount > 100)
        {
            totalDiscount = 100;
        }

        var multiplicationFactor = (100f - totalDiscount) / 100f;
        return ((CalculateCostBySize(deposit) + CalculateExtraCostByConditioning(deposit)) * duration) *
               multiplicationFactor;
    }


    public List<Reservation> GetReservations()
    {
        return ReservationRepository.GetReservations();
    }


    public void ConfirmReservation(Reservation reservation)
    {
        if (reservation.IsRejected)
        {
            throw new ArgumentException("La reserva fue rechazada, no se puede confirmar");
        }

        try
        {
            DepositRepository.RemoveDisponibilityDateRangeToDeposit(reservation.StartDate, reservation.EndDate,
                reservation.DepositId);
            ReservationRepository.ConfirmReservation(reservation);
        }
        catch (ArgumentException)
        {
            var automaticMessagge = "Se rechazo la reserva automaticamente por superposicion de fechas.";
            RejectReservation(reservation, automaticMessagge);
            throw new ArgumentException("Ese rango de reserva esta ocupado. ");
        }
    }

    public void ConfirmReservationByReservationManagementDto(ReservationManagementDTO reservationManagementDto)
    {
        var reservation = GetReservationById(reservationManagementDto.ReservationId);
        ConfirmReservation(reservation);
    }

    public Reservation GetReservationById(int reservationId)
    {
        return ReservationRepository.GetReservation(reservationId);
    }

    public List<Reservation> GetConfirmedReservations()
    {
        return ReservationRepository.GetConfirmedReservations();
    }

    public List<Reservation> GetRejectedReservations()
    {
        var reservations = GetReservations();
        return reservations.FindAll(reservation => reservation is { IsConfirmed: false, IsRejected: true });
    }


    public void RejectReservation(Reservation reservation, string message)
    {
        if (reservation.IsConfirmed)
        {
            throw new ArgumentException("La reserva fue confirmada, no se puede rechazar");
        }

        ReservationRepository.RejectReservation(reservation, message);
    }

    public void RejectReservationByReservationManagementDto(ReservationManagementDTO reservationManagementDto,
        string rejectionMessage)
    {
        var reservation = GetReservationById(reservationManagementDto.ReservationId);
        RejectReservation(reservation, rejectionMessage);
    }

    public List<Reservation> GetPendingReservations()
    {
        var reservations = GetReservations();
        return reservations.FindAll(reservation =>
            !reservation.IsConfirmed && !reservation.IsRejected && reservation.EndDate >= DateTime.Now);
    }

    public List<ReservationManagementDTO> GetReservationManagementDtos()
    {
        var pendingReservations = GetPendingReservations();
        var reservationManagementDtos = new List<ReservationManagementDTO>();
        foreach (var reservation in pendingReservations)
        {
            var client = GetClientByReservation(reservation);
            var deposit = GetDepositById(reservation.DepositId);
            var reservationManagementDto = new ReservationManagementDTO(reservation.DepositId, reservation.Id,
                deposit.Name,
                client.Name, client.Email, reservation.StartDate, reservation.EndDate);
            reservationManagementDtos.Add(reservationManagementDto);
        }

        return reservationManagementDtos;
    }

    public Client GetClientByReservation(Reservation reservation)
    {
        return ClientRepository.GetClient(reservation.ClientEmail);
    }
}
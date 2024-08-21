using System.Security;
using BusinessLogic;
using BusinessLogic.Domain;
using DataLayer.repositories;

namespace Controllers;

public class ClientController
{
    public ClientRepository ClientRepository { get; set; }
    public DepositRepository DepositRepository { get; set; }
    public PromotionRepository PromotionRepository { get; set; }
    public NotificationsRepository NotificationsRepository { get; set; }
    public LogRepository LogRepository { get; set; }
    public ReservationRepository ReservationRepository { get; set; }


    public ClientController(ClientRepository clientRepository, DepositRepository depositRepository,
        PromotionRepository promotionRepository, NotificationsRepository notificationsRepository,
        LogRepository logRepository, ReservationRepository reservationRepository)
    {
        ClientRepository = clientRepository;
        DepositRepository = depositRepository;
        PromotionRepository = promotionRepository;
        NotificationsRepository = notificationsRepository;
        LogRepository = logRepository;
        ReservationRepository = reservationRepository;
    }
    public Client? GetAdmin()
    {
        return ClientRepository.GetAdmin();
    }

    public ClientDto? GetAdminByClientDto()
    {
        var admin = GetAdmin();
        if (admin == null) return null;
        var adminDto = new ClientDto(admin.Name, admin.Email, admin.Password, admin.IsAdmin);
        return adminDto;
    }

    public bool AdminRegistered()
    {
        return ClientRepository.IsAdminRegistered();
    }

    public void RegisterAdmin(string adminExpectedName, string adminExpectedEmail, string adminExpectedPassword1,
        string adminExpectedPassword2)
    {
        var admin = new Client(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2,
            true);
        if (GetAdmin() == null)
        {
            ClientRepository.InsertClient(admin);
        }
        else
        {
            throw new ArgumentException("Ya hay un administrador registrado");
        }
    }

    private Client? ActiveUser { get; set; }

    public void SetActiveUser(string email)
    {
        var client = GetClient(email);
        ActiveUser = client;
    }

    public Client? GetActiveUser()
    {
        return ActiveUser;
    }

    public ClientDto? GetActiveUserByClientDto()
    {
        var client = GetActiveUser();
        if (client == null) return null;
        var clientDto = new ClientDto(client.Name, client.Email, client.Password, client.IsAdmin);
        return clientDto;
    }

    public void LogOut()
    {
        ActiveUser = null;
    }

    public void Login(string email, string password)
    {
        var client = GetClient(email);
        if (client.Password != password)
        {
            throw new SecurityException("Contraseña incorrecta");
        }

        ActiveUser = client;
    }

    public void RegisterClient(string clientName, string clientEmail, string clientPassword1,
        string clientPassword2)
    {
        var newClient = new Client(clientName, clientEmail, clientPassword1, clientPassword2, false);
        ClientRepository.InsertClient(newClient);
    }

    public List<Reservation> GetClientReservations(Client client)
    {
        return ReservationRepository.GetClientReservations(client.Email);
    }

    public List<Reservation> GetClientExpiredReservations(Client client)
    {
        var reservations = GetClientReservations(client);
        var expiredReservations = reservations
            .Where(reservation =>
                (reservation.EndDate < DateTimeProvider.GetCurrentDateTime()) && reservation.IsConfirmed).ToList();
        return expiredReservations;
    }

    public List<ReviewReservationDto> GetClientExpiredAndNotReviewedReservationInDepositReviewDtos(Client client)
    {
        var expiredReservations = GetClientExpiredReservations(client);
        var reservationInDepositReviewDtos = new List<ReviewReservationDto>();
        foreach (var reservation in expiredReservations)
        {
            if (!reservation.IsReviewed)
            {
                var deposit = DepositRepository.GetDeposit(reservation.DepositId);
                var reservationInDepositReviewDto = new ReviewReservationDto(reservation.Id, deposit.Id,
                    deposit.Name,
                    reservation.StartDate, reservation.EndDate, CalculateReservationCost(deposit, reservation.StartDate,
                        reservation.EndDate));
                reservationInDepositReviewDtos.Add(reservationInDepositReviewDto);
            }
        }

        return reservationInDepositReviewDtos;
    }

    public void RemoveNotification(Notification notification)
    {
        NotificationsRepository.DeleteNotification(notification);
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

    private bool IsPromotionAplicable(Promotion promotion, DateTime startDate)
    {
        return startDate >= promotion.StartDate && startDate <= promotion.EndDate;
    }


    public Client GetClient(string email)
    {
        return ClientRepository.GetClient(email);
    }

    public void AddActionToClient(string email, string description, DateTime date)
    {
        var action = new Log();
        action.AddAction(description, date);
        action.ClientEmail = email;
        LogRepository.InsertAction(action);
    }

    public Client GetClientByReservation(Reservation reservation)
    {
        return ClientRepository.GetClient(reservation.ClientEmail);
    }


    public List<ViewReservationDto> GetClientViewReservationDtOs(ClientDto clientDto)
    {
        var client = GetClient(clientDto.Email);
        var clientReservations = GetClientReservations(client);
        List<ViewReservationDto> clientReservationsDtosList = new List<ViewReservationDto>();

        foreach (var reservation in clientReservations)
        {
            ViewReservationDto reservationDto = new ViewReservationDto();
            reservationDto.StartDate = reservation.StartDate;
            reservationDto.EndDate = reservation.EndDate;
            reservationDto.IsRejected = reservation.IsRejected;
            reservationDto.IsConfirmed = reservation.IsConfirmed;
            reservationDto.Id = reservation.Id;
            reservationDto.DepositId = reservation.DepositId;
            reservationDto.IsReviewed = reservation.IsReviewed;
            reservationDto.RejectedMessage = reservation.RejectedMessage;
            reservationDto.PaymentStatus = (int)reservation.PaymentStatus;
            var deposit = GetDepositById(reservation.DepositId);
            reservationDto.DepositName = deposit.Name;
            clientReservationsDtosList.Add(reservationDto);
        }

        return clientReservationsDtosList;
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

    public List<ClientDto> GetClientDtos()
    {
        var clients = GetClients();
        var clientDtos = new List<ClientDto>();
        foreach (var client in clients)
        {
            var dto = new ClientDto
            {
                Name = client.Name,
                Email = client.Email,
                Password = client.Password,
                IsAdmin = client.IsAdmin
            };
            clientDtos.Add(dto);
        }

        return clientDtos;
    }


    public List<Notification> GetClientNotifications(string email)
    {
        return NotificationsRepository.GetClientNotifications(email);
    }

    public List<NotificationsDto> GetClientNotificationsDtos(String email)
    {
        List<NotificationsDto> notificationsDtos = new List<NotificationsDto>();
        var notifications = NotificationsRepository.GetClientNotifications(email);
        foreach (var notification in notifications)
        {
            NotificationsDto notificationDto = new NotificationsDto
            {
                ClientEmail = email,
                NotificationType = (int)notification.NotificationType,
                DepositName = notification.DepositName,
                StartDate = notification.StartDate,
                EndDate = notification.EndDate,
                Id = notification.Id
            };
            notificationsDtos.Add(notificationDto);
        }

        return notificationsDtos;
    }


    public List<Log> GetClientLog(string email)
    {
        return LogRepository.GetClientActionInLog(email);
    }


    public LogDto GetClientLogDtoByClientDto(ClientDto clientDto)
    {
        var clientLogDto = new LogDto();
        foreach (var log in GetClientLog(clientDto.Email))
        {
            clientLogDto.ActionDescriptions.Add(log.Description);
        }
        return clientLogDto;
    }

    public List<Log> GetClientLogByClientDto(ClientDto clientDto)
    {
        return GetClientLog(clientDto.Email);
    }

    public void AddNotificationToClient(string email, int notificationType, string depositName, DateTime startDate,
        DateTime endDate)
    {
        var detailedNotification = new Notification(notificationType, depositName, startDate, endDate)
        {
            ClientEmail = email
        };
        NotificationsRepository.InsertNotification(detailedNotification);
    }

    public void RemoveNotificationDto(NotificationsDto notificationDto)
    {
        var notification = new Notification
        {
            NotificationType = notificationDto.NotificationType,
            DepositName = notificationDto.DepositName,
            StartDate = notificationDto.StartDate,
            EndDate = notificationDto.EndDate,
            Id = notificationDto.Id
        };
        NotificationsRepository.DeleteNotification(notification);
    }

    public List<ReviewReservationDto> GetClientExpiredAndNotReviewedReservationInDepositReviewDtosByClientDto(
        ClientDto clientDto)
    {
        var client = GetClient(clientDto.Email);
        var expiredAndNotReviewdReservations = GetClientExpiredAndNotReviewedReservationInDepositReviewDtos(client);
        return expiredAndNotReviewdReservations;
    }

    public List<Client> GetClients()
    {
        return ClientRepository.GetClients();
    }

    public bool ClientIsRegistered(Client client)
    {
        return GetClients().Contains(client);
    }
}
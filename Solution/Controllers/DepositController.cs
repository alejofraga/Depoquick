using BusinessLogic;
using BusinessLogic.Domain;
using DataLayer.repositories;

namespace Controllers;

public class DepositController
{
    public DepositRepository DepositRepository { get; set; }
    public PromotionRepository PromotionRepository { get; set; }
    public ReservationRepository ReservationRepository { get; set; }
    public ReviewRepository ReviewRepository { get; set; }

    public DepositController(DepositRepository depositRepository, PromotionRepository promotionRepository,
        ReservationRepository reservationRepository, ReviewRepository reviewRepository)
    {
        DepositRepository = depositRepository;
        PromotionRepository = promotionRepository;
        ReservationRepository = reservationRepository;
        ReviewRepository = reviewRepository;
    }


    public List<Deposit> GetDeposits()
    {
        return DepositRepository.GetDeposits();
    }

    public Promotion GetPromotionById(int promotionId)
    {
        return PromotionRepository.GetPromotion(promotionId);
    }

    public void AddDeposit(string name, char area, string? size, bool conditioning,
        List<PromotionInDepositManagementDto?> promotionsDto)
    {
        var promotions = new List<Promotion?>();
        foreach (var promotionDto in promotionsDto)
        {
            if (promotionDto != null)
            {
                var promotion = GetPromotionById(promotionDto.Id);
                promotions.Add(promotion);
            }
        }

        var deposit = new Deposit
        {
            Name = name,
            Area = area,
            Size = size,
            Conditioning = conditioning,
        };
        if (GetDepositByName(name) == null)
        {
            var depositAdded = DepositRepository.InsertDeposit(deposit);
            foreach (var promotion in promotions)
            {
                PromotionRepository.AddPromotionToDeposit(depositAdded.Id, promotion.Id);
            }
        }
        else
        {
            throw new ArgumentException("Ya hay un deposito con ese nombre");
        }
    }
    
    public List<Reservation> GetReservations()
    {
        return ReservationRepository.GetReservations();
    }

    public bool DepositHasGotReservations(int depositId)
    {
        foreach (var reservation in GetReservations())
            if (reservation.DepositId == depositId)
            {
                return true;
            }

        return false;
    }


    public void RemoveDeposit(int depositId)
    {
        if (!DepositHasGotReservations(depositId))
        {
            var deposit = DepositRepository.GetDeposit(depositId);
            DepositRepository.DeleteDeposit(deposit);
        }
        else
        {
            throw new ArgumentException("El depósito está siendo utilizado por una reserva, no se puede eliminar");
        }
    }

    public bool AreDepositsRegistered()
    {
        if (GetDeposits().Any())
        {
            return true;
        }

        return false;
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

    public List<DepositManagementDto> GetDepositManagementDtos()
    {
        var deposits = GetDeposits();
        var depositFullDataDtosList = new List<DepositManagementDto>();
        foreach (var deposit in deposits)
        {
            var dto = new DepositManagementDto
            {
                Id = deposit.Id,
                Area = deposit.Area,
                Size = deposit.Size,
                Conditioning = deposit.Conditioning,
                Promotions = ConvertPromotionsOnPromotionInDepositManagementDtos(GetDepositPromotions(deposit.Id)),
                Name = deposit.Name
            };
            depositFullDataDtosList.Add(dto);
        }

        return depositFullDataDtosList;
    }

    public List<PromotionInDepositManagementDto> ConvertPromotionsOnPromotionInDepositManagementDtos(
        List<Promotion> promotions)
    {
        var promotionInDepositManagementDtos =
            new List<PromotionInDepositManagementDto>();
        foreach (var promotion in promotions)
        {
            PromotionInDepositManagementDto dto = new PromotionInDepositManagementDto
            {
                Id = promotion.Id,
                Tag = promotion.Tag
            };
            promotionInDepositManagementDtos.Add(dto);
        }

        return promotionInDepositManagementDtos;
    }


    public DepositCreateReservationDto ConvertDepositToDepositCreateReservationDto(Deposit incommingDeposit)
    {
        DepositCreateReservationDto newDepositCreateReservationDto = new DepositCreateReservationDto
        {
            Id = incommingDeposit.Id,
            Area = incommingDeposit.Area,
            Size = incommingDeposit.Size,
            Conditionig = incommingDeposit.Conditioning,
            PromotionDtoList = ConvertPromotionsToPromotionCreateReservationDto(
                PromotionRepository.GetPromotionsInDeposit(incommingDeposit.Id)),
            Name = incommingDeposit.Name,
            ReviewDtoList = ConvertReviewsToReviewCreateReservationDto(GetDepositReviews(incommingDeposit.Id)),
            HasPromotions = PromotionRepository.GetPromotionsInDeposit(incommingDeposit.Id).Any(),
            ContainsAnyReviews = GetDepositReviews(incommingDeposit.Id).Any()
        };

        return newDepositCreateReservationDto;
    }


    public List<ReviewInDepositCreateReservationDto> ConvertReviewsToReviewCreateReservationDto(
        List<Review> incommingReviews)
    {
        var ReviewCreateReservationDtoList =
            new List<ReviewInDepositCreateReservationDto>();

        foreach (var review in incommingReviews)
        {
            ReviewInDepositCreateReservationDto newReviewInDepositDto = new ReviewInDepositCreateReservationDto
            {
                Valoration = review.Valoration,
                Comment = review.Comment
            };
            ReviewCreateReservationDtoList.Add(newReviewInDepositDto);
        }

        return ReviewCreateReservationDtoList;
    }

    public List<PromotionInDepositCreateReservationDto> ConvertPromotionsToPromotionCreateReservationDto(
        List<Promotion> incommingPromotions)
    {
        var promotionCreateReservationDtoList =
            new List<PromotionInDepositCreateReservationDto>();

        foreach (var promotion in incommingPromotions)
        {
            var newPromotionInDepositDto =
                new PromotionInDepositCreateReservationDto
                {
                    Tag = promotion.Tag,
                    DiscountPercentage = promotion.DiscountPercentage
                };
            promotionCreateReservationDtoList.Add(newPromotionInDepositDto);
        }

        return promotionCreateReservationDtoList;
    }


    public List<Deposit> GetDepositsAvailableBetweentDates(DateTime fromStartDate, DateTime toEndDate)
    {
        return DepositRepository.GetDepositsAvailableBetweenDates(fromStartDate, toEndDate);
    }

    public List<DepositCreateReservationDto> GetAvailableDepositListInCreateReservationDto(DateTime startDate,
        DateTime endDate)
    {
        List<DepositCreateReservationDto> newDepositCreateReservationDtoList = new List<DepositCreateReservationDto>();

        foreach (var deposit in GetDepositsAvailableBetweentDates(startDate, endDate))
        {
            DepositCreateReservationDto newDepositDto =
                ConvertDepositToDepositCreateReservationDto(deposit);
            newDepositCreateReservationDtoList.Add(newDepositDto);
        }

        return newDepositCreateReservationDtoList;
    }

    private List<DateRange> GetDepositDisponibility(int depositId)
    {
        return DepositRepository.GetDepositDisponibility(depositId);
    }


    public bool DepositHasDisponibility(int depositId)
    {
        return DepositRepository.DepositHasDisponibility(depositId);
    }

    public DepositDisponibilityDto ConvertDepositToDepositDisponibilityDto(Deposit deposit)
    {
        var newDepositDisponibilityDto = new DepositDisponibilityDto
        {
            Name = deposit.Name,
            Size = deposit.Size,
            Area = deposit.Area,
            Conditioning = deposit.Conditioning,
            HasPromotions = GetDepositPromotions(deposit.Id).Any(),
            AreDisponibilityRangesRegistered = DepositHasDisponibility(deposit.Id)
        };

        foreach (var promotion in GetDepositPromotions(deposit.Id))
        {
            var auxPromotionDto =
                new PromotionDisponibilityDto(promotion.Tag, promotion.DiscountPercentage);
            newDepositDisponibilityDto.PromotionDtoList.Add(auxPromotionDto);
        }

        foreach (var dateRange in GetDepositDisponibility(deposit.Id))
        {
            var auxDateRange = new DateRangeDto(dateRange.StartDate, dateRange.EndDate);
            newDepositDisponibilityDto.DateRangesList.Add(auxDateRange);
        }


        return newDepositDisponibilityDto;
    }

    public List<DepositDisponibilityDto> GetDepositsInDepositDisponibilityDto()
    {
        var newDepositDisponibilityDtoList = new List<DepositDisponibilityDto>();

        foreach (var deposit in GetDeposits())
        {
            var auxDepositDisponibilityDto = ConvertDepositToDepositDisponibilityDto(deposit);
            newDepositDisponibilityDtoList.Add(auxDepositDisponibilityDto);
        }

        return newDepositDisponibilityDtoList;
    }

    public Deposit? GetDepositByName(string depositName)
    {
        return DepositRepository.GetDepositByName(depositName.ToUpper());
    }

    public List<PromotionManagementDto> GetPromotionsInDepositInPromotionManagementDto(int depositId)
    {
        List<Promotion> depositPromotions = GetDepositPromotions(depositId);
        List<PromotionManagementDto> promotionsInDto = new List<PromotionManagementDto>();
        foreach (var promotion in depositPromotions)
        {
            PromotionManagementDto newPromotionDto = new PromotionManagementDto();
            newPromotionDto.Id = promotion.Id;
            newPromotionDto.Tag = promotion.Tag;
            newPromotionDto.DiscountPercentage = promotion.DiscountPercentage;
            newPromotionDto.StartDate = promotion.StartDate;
            newPromotionDto.EndDate = promotion.EndDate;
            promotionsInDto.Add(newPromotionDto);
        }

        return promotionsInDto;
    }

    public List<Promotion> GetDepositPromotions(int depositId)
    {
        return PromotionRepository.GetPromotionsInDeposit(depositId);
    }

    public void AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(DateTime expectedDateRangeStartDate,
        DateTime expectedDateRangeEndDate,
        DepositDisponibilityDto selectedDepositDto)
    {
        var auxDeposit = GetDepositByName(selectedDepositDto.Name);
        AddDateRangeDisponibilityToDeposit(expectedDateRangeStartDate,
            expectedDateRangeEndDate, auxDeposit);
    }

    public void AddDateRangeDisponibilityToDeposit(DateTime expectedDateRangeStartDate,
        DateTime expectedDateRangeEndDate,
        Deposit selectedDeposit)
    {
        DepositRepository.AddDisponibilityDateRangeToDeposit(selectedDeposit.Id, expectedDateRangeStartDate,
            expectedDateRangeEndDate);
    }

    public Reservation GetReservationById(int reservationId)
    {
        return ReservationRepository.GetReservation(reservationId);
    }

    public List<Review> GetDepositReviews(int depositId)
    {
        return ReviewRepository.GetDepositReviews(depositId);
    }

    public void AddReviewToDeposit(int depositId, int valoration, string comment,
        ReviewReservationDto reviewReservationDto)
    {
        var reservation = GetReservationById(reviewReservationDto.ReservationId);
        MarkReservationAdReviewed(reservation);
        var review = new Review(valoration, comment)
        {
            DepositId = depositId
        };
        ReviewRepository.InsertReview(review);
    }

    private void MarkReservationAdReviewed(Reservation reservation)
    {
        ReservationRepository.MarkReservationAdReviewed(reservation.Id);
    }
}
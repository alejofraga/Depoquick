using BusinessLogic;
using BusinessLogic.Domain;
using DataLayer.repositories;

namespace Controllers;

public class PromotionController
{
    public PromotionRepository PromotionRepository { get; set; }

    public PromotionController(PromotionRepository promotionRepository)
    {
        PromotionRepository = promotionRepository;
    }

    public List<Promotion> GetPromotions()
    {
        return PromotionRepository.GetPromotions();
    }

    public Promotion GetPromotionById(int promotionId)
    {
        return PromotionRepository.GetPromotion(promotionId);
    }

    public void AddPromotion(string? tag, int discountPercentage, DateTime startDate, DateTime endDate)
    {
        var promotion = new Promotion(tag, discountPercentage, startDate, endDate);
        PromotionRepository.InsertPromotion(promotion);
    }


    public bool PromotionIsAplyingToAnyDeposit(int promotionId)
    {
        return PromotionRepository.PromotionIsAplyingToAnyDeposit(promotionId);
    }

    public void RemovePromotion(int promotionId)
    {
        if (!PromotionIsAplyingToAnyDeposit(promotionId))
        {
            var promotion = PromotionRepository.GetPromotion(promotionId);
            PromotionRepository.DeletePromotion(promotion);
        }
        else
        {
            throw new ArgumentException("La promoción está siendo utilizada por un depósito, no se puede eliminar");
        }
    }

    public void UpdatePromotion(string? tag, int discountPercentage, DateTime startDate, DateTime endDate,
        int promotionId)
    {
        if (!PromotionIsAplyingToAnyDeposit(promotionId))
        {
            PromotionRepository.UpdatePromotion(tag, discountPercentage, startDate, endDate, promotionId);
        }
        else
        {
            throw new ArgumentException("La promoción está siendo utilizada por un depósito, no se puede editar");
        }
    }

    
    public List<PromotionInDepositManagementDto> GetPromotionInDepositManagementDtos()
    {
        var promotions = GetPromotions();
        var promotionIdAndTagDtosList = new List<PromotionInDepositManagementDto>();
        foreach (var promotion in promotions)
        {
            PromotionInDepositManagementDto dto = new PromotionInDepositManagementDto
            {
                Id = promotion.Id,
                Tag = promotion.Tag
            };
            promotionIdAndTagDtosList.Add(dto);
        }

        return promotionIdAndTagDtosList;
    }

    public List<PromotionManagementDto> GetPromotionInPromotionManagementDtos()
    {
        var promotions = GetPromotions();
        List<PromotionManagementDto> promotionFullDataDtosList = new List<PromotionManagementDto>();
        foreach (var promotion in promotions)
        {
            PromotionManagementDto dto = new PromotionManagementDto
            {
                Id = promotion.Id,
                Tag = promotion.Tag,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                DiscountPercentage = promotion.DiscountPercentage
            };
            promotionFullDataDtosList.Add(dto);
        }

        return promotionFullDataDtosList;
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
}
using WebFrontEnd.Services.BusinessPartners.Dto;
using WebFrontEnd.Services.Items.DTO;

namespace WebFrontEnd.Services.BusinessPartners
{
    public interface IBusinessPartnersService
    {
        Task<BusinessPartnersDto> GetBusinessPartnersAsync(string CardCode, string CardType, bool forceRefresh);
        Action BusinessPartnersAction { get; set; }
    }
}



using Application.Dtos;

namespace Application.UseCases.Orders.DetailsOrders
{
    public record DetailsOrdersResponse : SalesHeader
    {
        public double DocRate { get; set; }
        public int? SalesPersonCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int? UserSign { get; set; }


        public string? Confirmed { get; set; }


        public string? ControlAccount { get; set; }

        public int? DataVersion { get; set; }
        public string? AgentCode { get; set; }
        public int? TransportationCode { get; set; }
        public string? PartialSupply { get; set; }

        public List<GetAllOrdersLineResponse>? DocumentLines { get; set; }
    }
}

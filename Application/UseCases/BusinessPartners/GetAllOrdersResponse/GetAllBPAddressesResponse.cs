using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.BusinessPartners.GetAllOrdersResponse
{
    public record GetAllBPAddressesResponse
    {
        public string? AddressName { get; set; }
        public string? Street { get; set; }
        public string? Block { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Country { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.GetLastOrderCartPend
{
    public sealed record GetLastOrdersCartPendResponse
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public bool Status { get; set; } = false;
        public string UserId { get; set; }
        public List<GetLastOrderLineCartPendResponse> OrderLineCarts { get; set; }
    }
}

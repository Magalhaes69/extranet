using MediatR;

namespace Application.UseCases.ContactEmployees.GetAllContactEmployees
{
    public sealed record GetAllContactEmployeesRequest : IRequest<GetAllContactEmployeesResponse>
    {
        public string SqlCode { get; set; }
        public int CntctCode { get; set; }
    }
}

using Application.Dtos;
using MediatR;

namespace Application.UseCases.SQLQueries
{
    public sealed record SapQueryRequest : IRequest<List<SapQueryResponse>>
    {
        public string  Query { get; set; }
        public string ObjectCode { get; set; }
    }
}

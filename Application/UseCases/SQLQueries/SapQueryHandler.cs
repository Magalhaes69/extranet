

using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.SQLQueries
{
    public sealed class SapQueryHandler : IRequestHandler<SapQueryRequest, List<SapQueryResponse>>
    {
        private readonly ISapClient _sapClient;
        public SapQueryHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<List<SapQueryResponse>> Handle(SapQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sapClient.ExecuteQueryAsync<SapQueryResponse>(
                    request.Query,
                    request.ObjectCode,
                    "ObjectCode",
                    cancellationToken);

                return result ?? new List<SapQueryResponse>();
            }
            catch(Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("An error occurred while executing the SAP query.", ex);
            }
        }
    }
}

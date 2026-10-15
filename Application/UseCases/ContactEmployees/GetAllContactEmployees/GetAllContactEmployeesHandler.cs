using Application.Shared.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.ContactEmployees.GetAllContactEmployees
{

    public sealed class GetAllContactEmployeesHandler : IRequestHandler<GetAllContactEmployeesRequest, GetAllContactEmployeesResponse>
    {
        private readonly ISapClient _sapClient;

        public GetAllContactEmployeesHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<GetAllContactEmployeesResponse> Handle(GetAllContactEmployeesRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var parameters = ToDicionary.ConvertDicionary(request);
                var result = await _sapClient.ExecuteQueryObjAsync<GetAllContactEmployeesResponse>(
                    request.SqlCode,
                    parameters,
                    cancellationToken);

                return result ?? new GetAllContactEmployeesResponse();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("An error occurred while retrieving contact employees.", ex);
            }
        }
    }
}

using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Attachments2.GetAttachment
{
    public class GetAttachments2Handler : IRequestHandler<GetAttachments2Request, GetAttachments2>
    {
        private readonly ISapClient _sapClient;
        public GetAttachments2Handler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<GetAttachments2> Handle(GetAttachments2Request request, CancellationToken cancellationToken)
        {
            var result = await _sapClient.GetAttachments2Async($"Attachments2({request.AbsoluteEntry})/$value",
                    cancellationToken);


            return result;
        }
    }
}

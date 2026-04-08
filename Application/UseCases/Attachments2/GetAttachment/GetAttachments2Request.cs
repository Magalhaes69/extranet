
using Domain.Models;
using MediatR;

namespace Application.UseCases.Attachments2.GetAttachment
{
    public class GetAttachments2Request : IRequest<GetAttachments2>
    {
        public int AbsoluteEntry { get; set; }
    }
}

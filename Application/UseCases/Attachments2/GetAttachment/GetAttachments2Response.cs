

namespace Application.UseCases.Attachments2.GetAttachment
{
    public class GetAttachments2Response
    {
        public Stream Stream { get; set; } = default!;
        public string? ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}

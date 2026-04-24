
using Application.Dtos;
using MediatR;

namespace Application.UseCases.ItemExtensions.EditItemExtension
{
    public sealed record EditItemExtensionRequest : ItemExtensionsBase, IRequest<EditItemExtensionResponse>
    {
        public Stream? ImageStream { get; set; }
        public string? FileName { get; set; }
    }
}

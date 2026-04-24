using Application.UseCases.ItemExtensions.CheckItemExtensions;
using Application.UseCases.ItemExtensions.CreateItemExtension;
using Application.UseCases.ItemExtensions.DeleteItemExtensions;
using Application.UseCases.ItemExtensions.EditItemExtension;
using Application.UseCases.ItemExtensions.GetAllItemExtensions;
using Application.UseCases.ItemExtensions.GetByItemIdItemExtensions;
using Application.UseCases.Items;
using Application.UseCases.OrdersCarts.UpdateOrderCartLine;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemExtensionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemExtensionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrdersCart(
            [FromForm] CreateItemExtensionRequest request,
            [FromForm] IFormFile? file,
            CancellationToken cancellationToken)
        {
            // Save image first
            if (file != null)
            {
                var imageUrl = await SaveImage(file);
                request.ImageUrl = imageUrl;
            }

            var result = await _mediator.Send(request, cancellationToken);
            if(result == null)
            {
                return NotFound(new ServiceGeneralResponse<CreateItemExtensionResponse>
                {
                    Flag = false,
                    Message = "Item extension not found",
                    Data = null
                });
            }

            return Ok(new ServiceGeneralResponse<CreateItemExtensionResponse>
            {
                Flag = true,
                Message = "Item extension created successfully.",
                Data = result
            });

        }

        // GET
        [HttpGet("list")]
        public async Task<IActionResult> GetAllItemExtensions(CancellationToken cancellationToken)
        {
            var request = new GetAllItemExtensionsRequest();
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }


        [HttpGet("GetByItemCode")]
        public async Task<ActionResult<ServiceGeneralResponse<CheckItemExtensionsResponse>>> GetByItemCode(
            [FromQuery] string itemCode,
            CancellationToken cancellationToken)
        {
            var request = new CheckItemExtensionsRequest { ItemCode = itemCode };
            var result = await _mediator.Send(request, cancellationToken);

            if(result == null)
            {
                return NotFound(new ServiceGeneralResponse<CheckItemExtensionsResponse>
                {
                    Flag = false,
                    Message = "Item extension not found",
                    Data = null
                });
            }

            return Ok(new ServiceGeneralResponse<CheckItemExtensionsResponse>
            {
                Flag = true,
                Message = "Item exist.",
                Data = result
            });
        }

        [HttpGet("GetByItemId")]
        public async Task<IActionResult> GetByItemId(
          [FromQuery] string ItemId,
          CancellationToken cancellationToken)
        {
            var request = new GetByItemIdItemExtensionsRequest { ItemId = ItemId };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("delete-many")]
        public async Task<IActionResult> DeleteItemExtension([FromBody] List<string> ItemCode, CancellationToken cancellationToken)
        {
            var request = new DeleteItemExtensionRequest { ItemCode = ItemCode };
            var result = await _mediator.Send(request, cancellationToken);

            if (!result.Flag)
            {
                return NotFound(new ServiceGeneralResponse<bool>
                {
                    Flag = false,
                    Message = "Item extension not found",
                    Data = false
                });
            }


            return Ok(new ServiceGeneralResponse<bool>
            {
                Flag = true,
                Message = "Item extension deleted successfully.",
                Data = true
            });
        }


        [HttpPut]
        public async Task<IActionResult> EditOrdersCart(
            [FromForm] EditItemExtensionRequest request,
            [FromForm] IFormFile? file,
            CancellationToken cancellationToken)
        {
           if(file != null)
            {
                request.FileName = file.FileName;
                request.ImageStream = file.OpenReadStream();
            }

            var result = await _mediator.Send(request, cancellationToken);
            if (result == null)
            {
                return NotFound(new ServiceGeneralResponse<EditItemExtensionResponse>
                {
                    Flag = false,
                    Message = "Item extension not found",
                    Data = null
                });
            }

            return Ok(new ServiceGeneralResponse<EditItemExtensionResponse>
            {
                Flag = true,
                Message = "Item extension updated successfully.",
                Data = result
            });

        }

        private async Task<string> SaveImage(IFormFile file)
        {
            
            if (!file.ContentType.StartsWith("image/"))
                throw new Exception("Invalid file type");

            var folderPath = Path.Combine("wwwroot", "images", "items");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return URL to store in DB
            return $"/images/items/{fileName}";
        }
    }
}

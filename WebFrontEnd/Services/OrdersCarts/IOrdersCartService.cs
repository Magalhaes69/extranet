using WebFrontEnd.Services.Orders.Models;
using WebFrontEnd.Services.OrdersCarts.Dto;

namespace WebFrontEnd.Services.OrdersCarts
{
    public interface IOrdersCartService
    {
        Task<ServiceResponseGeneralPost<CreateOrdersCardResponseDto>> CreateAsync(CreateOrdersCartDto order);
        Task<ServiceResponseGeneralPost<CreateOrderLineCartResponseDto>> AddLineAsync(CreateOrderLineCartExistOrderDto order);
        Task<ServiceResponseGeneralPost<UpdateOrdersCartLineDto>> UpdateLineAsync(UpdateOrdersCartLineDto order);
        
        /// <summary>
        /// Marks an existing entity to be updated in the next commit.
        /// </summary>
        /// <param name="order">The entity containing the updated values.</param>
        /// <remarks>This will send data for backend to persist.</remarks>
        Task<ServiceResponseGeneralPost<UpdateOrdersCartLineDto>> UpdateOrdersAsync(UpdateOrdersCartsRequest order);
        Task<ServiceResponseGeneralPost<ViewOrderLineCartDto>> DeleteLineAsync(string OrderLineCartId, string OrderId);

        /// <summary>
        /// Deletes an order based on the specified order ID
        /// </summary>
        /// <param name="OrderId">The unique identifier of the order to be deleted.</param>
        /// <returns>
        /// A <see cref="ServiceResponseGeneralPost{ViewOrdersCartDto}"/> containing the result of the operation
        /// and the deleted order data if successful.
        /// </returns>
        Task<ServiceResponseGeneralPost<ViewOrdersCartDto>> DeleteOrderIdAsync(string OrderId);
        Task<ViewOrdersCartDto> GetLastOrdersCartPend(string CardCode, string UserId, bool forceRefresh = false);

        /// <summary>
        /// Clear the current cart by resetting all items quantities
        /// </summary>
        /// <remarks>
        /// Resets the cart to an empty state and triggers the <see cref="OrderAction"/> event
        /// so that UI elements (e.g., cart counters) are updated.
        /// </remarks>
        void ClearCart();

        /// <summary>
        /// Event triggered when the cart state changes
        /// </summary>
        /// <remarks>
        /// Components should sibcribe to this event to refresh their UI when the cart is updated.
        /// </remarks>
        Action OrderAction { get; set; }
    }
}

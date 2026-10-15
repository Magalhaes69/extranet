using Microsoft.AspNetCore.Components;
using WebFrontEnd.Helps;
using WebFrontEnd.Services.OrdersCarts;
using WebFrontEnd.Services.OrdersCarts.Dto;

namespace WebFrontEnd.Pages.Components.Geral
{
    public partial class CartDropdownComponent : ComponentBase, IDisposable
    {
        [Inject] public OrdersCartService CartServices { get; set; }
        private string userId = string.Empty;
        
        protected override async Task OnInitializedAsync()
        {
            CartServices.OnChange += async () => await InvokeAsync(StateHasChanged);

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var cardCode = user.GetCardCode();
                userId = user.GetUserGuid();

                await CartServices.GetLastOrdersCartPend(cardCode, userId);
            }
        }

        public void Dispose()
        {
            CartServices.OnChange -= StateHasChanged;
        }

        private decimal ItemCount =>
            CartServices.CurrentCart?.OrderLineCarts?.Sum(x => x.Quantity) ?? 0;

        private decimal Total =>
            CartServices.CurrentCart?.OrderLineCarts?.Sum(x => x.Quantity * x.UnitPrice) ?? 0;

        private async Task Increase(ViewOrderLineCartDto item)
        {
            var list = CartServices.CurrentCart.OrderLineCarts;

            var row = list!.FirstOrDefault(x => x.OrderLineCartId == item.OrderLineCartId);
            if (row == null)
                return;
            row.Quantity += 1;
            row.LineTotal = row.Quantity * row.UnitPrice;


            var OrderLine = new UpdateOrdersCartLineDto()
            {
                OrderLineCartId = row.OrderLineCartId,
                OrderId = row.OrderId,
                ItemCode = row.ItemCode,
                Quantity = row.Quantity,
                UnitPrice = row.UnitPrice,
                ItemDescription = item.ItemDescription,
                LineTotal = row.LineTotal,
                DiscPrcnt = row.DiscPrcnt,
                UserId = row.UserId,
                WarehouseCode = row.WarehouseCode,
            };
            var result = await CartServices.UpdateLineAsync(OrderLine);
            if (result.Flag)
                ToastService.ShowSuccess(LanGeneral["successMessage"], ThemeService.CurrentTheme);
            else
                ToastService.ShowError(LanGeneral["errorMessage"], ThemeService.CurrentTheme);
            return;
        }

        private async Task Decrease(dynamic item)
        {
            var list = CartServices.CurrentCart.OrderLineCarts;

            var row = list!.FirstOrDefault(x => x.OrderLineCartId == item.OrderLineCartId);
            if (row == null)
                return;
            row.Quantity -= 1;
            row.LineTotal = row.Quantity * row.UnitPrice;


            var OrderLine = new UpdateOrdersCartLineDto()
            {
                OrderLineCartId = row.OrderLineCartId,
                OrderId = row.OrderId,
                ItemCode = row.ItemCode,
                Quantity = row.Quantity,
                UnitPrice = row.UnitPrice,
                ItemDescription = item.ItemDescription,
                LineTotal = row.LineTotal,
                DiscPrcnt = row.DiscPrcnt,
                UserId = row.UserId,
                WarehouseCode = row.WarehouseCode,
            };
            await CartServices.UpdateLineAsync(OrderLine);
        }

        private async Task Remove(dynamic item)
        {
            if (CartServices.CurrentCart?.OrderLineCarts?.Count == 1)
            {
                var resultR = await CartServices.DeleteOrderIdAsync(item.OrderId.ToString());
                if (resultR.Flag)
                {
                    ToastService.ShowSuccess(LanGeneral["successMessage"], ThemeService.CurrentTheme);
                    CartServices.CurrentCart = new ViewOrdersCartDto
                    {
                        OrderLineCarts = new List<ViewOrderLineCartDto>()
                    };
                    StateHasChanged();
                }
                else
                    ToastService.ShowError(LanGeneral["errorMessage"], ThemeService.CurrentTheme);
            }
            else
            {
                var result = await CartServices.DeleteLineAsync(item.OrderLineCartId.ToString(), item.OrderId.ToString());

                if (result.Flag)
                {
                    ToastService.ShowSuccess(LanGeneral["successMessage"], ThemeService.CurrentTheme);
                }
                else
                    ToastService.ShowError(LanGeneral["errorMessage"], ThemeService.CurrentTheme);
            }
            return;
        }
        private async Task ViewOrder()
        {

        }
        private async Task FinishOrder()
        {
            var getCart = CartServices.CurrentCart;
            var orders = new UpdateOrdersCartsRequest()
            {
                OrderId = getCart.OrderId,
                Status = true,
                UserId = userId
            };

            var response = await OrdersCartService.UpdateOrdersAsync(orders);
            if (response.Flag)
            {
                ToastService.ShowSuccess(LanGeneral["successMessage"], ThemeService.CurrentTheme);

               CartServices.CurrentCart = new ViewOrdersCartDto
                {
                    OrderLineCarts = new List<ViewOrderLineCartDto>()
                };
                StateHasChanged();
            }
            else
                ToastService.ShowError(LanGeneral["errorMessage"], ThemeService.CurrentTheme);
        }

        
    }
}

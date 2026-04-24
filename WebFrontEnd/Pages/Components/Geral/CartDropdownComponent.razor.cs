using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WebFrontEnd.Helps;
using WebFrontEnd.Services.OrdersCarts;
using WebFrontEnd.Services.OrdersCarts.Dto;

namespace WebFrontEnd.Pages.Components.Geral
{
    public partial class CartDropdownComponent : ComponentBase, IDisposable
    {
        [Inject] public OrdersCartService CartServices { get; set; }
       
        protected override async Task OnInitializedAsync()
        {
            CartServices.OnChange += StateHasChanged;

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var cardCode = user.GetCardCode();
                var userId = user.GetUserGuid();

                await CartService.GetLastOrdersCartPend(cardCode, userId);
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
                WarehouseCode = "PGMT"
            };
            var result = await CartService.UpdateLineAsync(OrderLine);
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
                WarehouseCode = "PGMT"
            };
            await CartService.UpdateLineAsync(OrderLine);
        }

        private async Task Remove(dynamic item)
        {
            var result = await CartService.DeleteLineAsync(item.OrderLineCartId.ToString(), item.OrderId.ToString());

            if (result.Flag)
                ToastService.ShowSuccess(LanGeneral["successMessage"], ThemeService.CurrentTheme);
            else
                ToastService.ShowError(LanGeneral["errorMessage"], ThemeService.CurrentTheme);
            return;
        }
        private async Task ViewOrder()
        {

        }
        private async Task FinishOrder()
        {
            Console.WriteLine("Finish order clicked");
        }
    }
}

namespace WebFrontEnd.Services.Toast
{
    public class ToastService
    {
        public List<ToastMessage> Toasts { get; set; } = new();

        public event Action? OnChange;
        public bool ShowBusyButton { get; set; }
        public bool ShowSaveButton { get; set; } = true;

        public void ShowSuccess(string message, string backround)
            => Add(message, backround, "bi bi-check-circle");

        public void ShowError(string message, string backround)
            => Add(message, backround, "bi bi-x-circle");
        public void ShowWarning(string message, string backround)
           => Add(message, backround, "bi bi-exclamation-triangle text-warning");
        public void ShowInfo(string message, string backround)
            => Add(message, backround, "bi bi-info-circle");

        private async void Add(string message, string bgClass, string icon)
        {
            var toast = new ToastMessage
            {
                Message = message,
                BgClass = bgClass,
                Icon = icon
            };

            Toasts.Add(toast);
            OnChange?.Invoke();

            ShowBusyButton = false;
            ShowSaveButton = true;

            await Task.Delay(5000);

            Toasts.Remove(toast);
            OnChange?.Invoke();
        }

        public void Remove(ToastMessage toast)
        {
            Toasts.Remove(toast);
            OnChange?.Invoke();
        }
    }
}

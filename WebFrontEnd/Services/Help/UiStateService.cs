namespace WebFrontEnd.Services.Help
{
    public class UiStateService
    {
        public string Favicon { get; private set; } = "";

        public event Func<Task>? OnChange;

        public void SetFavicon(string url)
        {
            Favicon = url;
            OnChange?.Invoke();
        }
    }
}

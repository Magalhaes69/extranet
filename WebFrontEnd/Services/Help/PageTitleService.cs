namespace WebFrontEnd.Services.Help
{
    public class PageTitleService
    {
        public string Title { get; private set; } = "Home";

        public event Action? OnChange;

        public void SetTitle(string title)
        {
            Title = title;
            OnChange?.Invoke();
        }
    }
}

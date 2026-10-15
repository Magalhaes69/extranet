namespace WebFrontEnd.Services
{
    public class PageStateService
    {
        public string PageTitle { get; set; } = string.Empty;
        public event Action? OnPageTitleChanged;

        public void SetTitle(string title)
        {
            PageTitle = title;
            OnPageTitleChanged?.Invoke();
        }
    }
}

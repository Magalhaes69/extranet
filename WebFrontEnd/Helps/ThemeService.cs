namespace WebFrontEnd.Helps
{
    public class ThemeService
    {
        public string CurrentTheme { get; private set; } = default!; 
        public event Action? OnThemeChanged;
        public void SetTheme(string theme)
        {
            CurrentTheme = theme;
            OnThemeChanged?.Invoke();
        }
    }
}

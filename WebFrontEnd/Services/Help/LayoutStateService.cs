namespace WebFrontEnd.Services.Help
{
    public class LayoutStateService
    {
        public bool IsSidebarCollapsed { get; private set; }

        public event Func<Task>? OnChange;

        public async Task ToggleSidebar()
        {
            IsSidebarCollapsed = !IsSidebarCollapsed;
            if (OnChange != null)
                await OnChange.Invoke();

        }

        public async Task SetSidebar(bool collapsed)
        {
            IsSidebarCollapsed = collapsed;
            if (OnChange != null)
                await OnChange.Invoke();
        }
    }
}

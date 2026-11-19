namespace WebFrontEnd.Services.Help
{
    public class LayoutStateService
    {
        public bool IsSidebarCollapsed { get; private set; }

        public event Func<Task>? OnChange;

        /// <summary>Disparado quando o perfil (nome, fotografia) muda, para o cabeçalho se actualizar.</summary>
        public event Func<Task>? OnProfileChanged;

        public async Task NotifyProfileChanged()
        {
            if (OnProfileChanged != null)
                await OnProfileChanged.Invoke();
        }

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

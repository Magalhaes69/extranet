using Microsoft.AspNetCore.Components;

namespace WebFrontEnd.Services.Help
{
    public class NavigationHistoryService
    {
        private readonly Stack<string> _history = new();

        public event Action? OnChange;

        public int Count => _history.Count;

        public void Init(NavigationManager nav)
        {
            Add(nav.Uri);

            nav.LocationChanged += (sender, args) =>
            {
                Add(args.Location);
            };
        }

        private void Add(string uri)
        {
            if (_history.Count == 0 || _history.Peek() != uri)
            {
                _history.Push(uri);
                OnChange?.Invoke();
            }
        }

        public string? Back()
        {
            if (_history.Count <= 1)
                return null;

            _history.Pop();
            OnChange?.Invoke();

            return _history.Peek();
        }
    }
}

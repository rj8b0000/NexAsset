using System;

namespace NexAsset.Web.State
{
    public class ThemeState
    {
        public bool DarkMode { get; private set; } = false;
        
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void SetDarkMode(bool darkMode)
        {
            DarkMode = darkMode;
            NotifyStateChanged();
        }
    }
}

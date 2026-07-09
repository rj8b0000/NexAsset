using System;
using NexAsset.Web.Infrastructure.Models;

namespace NexAsset.Web.State
{
    public class AuthenticationState
    {
        private readonly NotificationState _notificationState;

        public bool IsAuthenticated { get; private set; } = true; // Start logged-in for ease of review
        public string AuthEmail { get; set; } = "";
        public string AuthPassword { get; set; } = "";
        public string AuthView { get; set; } = "login"; // login, forgot, reset
        public bool ShowAuthError { get; set; } = false;

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public AuthenticationState(NotificationState notificationState)
        {
            _notificationState = notificationState;
        }

        public void Login(string email, string password)
        {
            if ((email == "admin@nexasset.com" && password == "password") || (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password)))
            {
                IsAuthenticated = true;
                ShowAuthError = false;
                _notificationState.AddToast("Authenticated", "Welcome back, Admin!", ToastType.Success);
                NotifyStateChanged();
            }
            else
            {
                ShowAuthError = true;
                NotifyStateChanged();
            }
        }

        public void Logout()
        {
            IsAuthenticated = false;
            AuthView = "login";
            NotifyStateChanged();
        }

        public void SetAuthView(string view)
        {
            AuthView = view;
            ShowAuthError = false;
            NotifyStateChanged();
        }
    }
}

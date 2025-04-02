using MedHelpAuthorizations.Client.Infrastructure.Interfaces;
using System;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers
{
    public class UserSessionState : IState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user is auto-logged out.
        /// </summary>
        private bool _isAutoLogout { get; set; } = false;

        /// <summary>
        /// Gets or sets the auto logout status. Changes are only allowed if the value is different from the current state.
        /// </summary>
        public bool IsAutoLogout
        {
            get => _isAutoLogout;
            private set
            {

                if (_isAutoLogout == false || !_isAutoLogout.Equals(value))
                {
                    _isAutoLogout = value;
                }
                Console.WriteLine("Property changed to:", _isAutoLogout, Environment.StackTrace);
            }
        }

        /// <summary>
        /// Gets or sets the last visited page location (URL).
        /// </summary>
        private string _pageLocation { get; set; }

        /// <summary>
        /// Gets or sets the page location (URL). Changes are only allowed if the value is different from the current state.
        /// </summary>
        public string PageLocation
        {
            get => _pageLocation;
            private set
            {
                if (_pageLocation == null || !_pageLocation.Equals(value))
                {
                    _pageLocation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the email address of the last logged-in user.
        /// </summary>
        private string _lastLoggedInUser { get; set; }

        /// <summary>
        /// Gets or sets the last logged-in user. Changes are only allowed if the value is different from the current state.
        /// </summary>
        public string LastLoggedInUser
        {
            get => _lastLoggedInUser;
            private set
            {
                if (_lastLoggedInUser == null || !_lastLoggedInUser.Equals(value))
                {
                    _lastLoggedInUser = value;
                }
            }
        }

        /// <summary>
        /// Clears all state data, including the last logged-in user, auto logout status, and page location.
        /// </summary>
        public void ClearAllState()
        {
            LastLoggedInUser = null;
            IsAutoLogout = false;
            PageLocation = null;
        }

        /// <summary>
        /// Sets the auto logout status.
        /// </summary>
        /// <param name="value">The new auto logout status.</param>
        public void SetIsAutologout(bool value)
        {
            IsAutoLogout = value;
        }

        /// <summary>
        /// Sets the last visited page location (URL).
        /// </summary>
        /// <param name="url">The URL of the last visited page.</param>
        public void SetPageLocation(string url)
        {
            PageLocation = url;
        }

        /// <summary>
        /// Sets the email address of the last logged-in user.
        /// </summary>
        /// <param name="User">The email address of the last logged-in user.</param>
        public void SetLastLoggedInUser(string User)
        {
            LastLoggedInUser = User;
        }



    }

}

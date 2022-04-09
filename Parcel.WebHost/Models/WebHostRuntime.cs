using System;
using System.Diagnostics;

namespace Parcel.WebHost.Models
{
    public class WebHostRuntime
    {
        #region Constructor
        public WebHostRuntime()
        {
            if (Singleton == null)
                Singleton = this;
            else
            {
                throw new InvalidOperationException("WebHostRuntime is already initialized! Singleton is not null.");
            }
        }
        public static WebHostRuntime Singleton { get; set; }
        #endregion

        #region Properties
        public int Port { get; set; }
        public string Address { get; set; }
        public bool ShouldLog { get; set; }

        #endregion

        #region Accessor - Endpoints
        public string BaseUrl => Address;
        public void Open()
        {
            new Process
            {
                StartInfo = new ProcessStartInfo(Address)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
        #endregion
    }
}
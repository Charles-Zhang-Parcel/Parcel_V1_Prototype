using System;
using System.Diagnostics;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared
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
        public void Open(string adress)
        {
            new Process
            {
                StartInfo = new ProcessStartInfo(adress)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
        public void OpenTarget(string target)
        {
            new Process
            {
                StartInfo = new ProcessStartInfo($"{Address}/{target}")
                {
                    UseShellExecute = true
                }
            }.Start();
        }
        #endregion

        #region Interoperation
        public ProcessorNode LastNode { get; set; }
        #endregion
    }
}
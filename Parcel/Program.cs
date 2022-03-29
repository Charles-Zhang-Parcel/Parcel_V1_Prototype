using System;
using Parcel.Shared;

namespace Parcel
{
    internal static class Program
    {
        [STAThreadAttribute]
        private static void Main(string[] args)
        {
            // Initialize application data
            ParcelRuntime runtimeContext = new ParcelRuntime()
            {
            };
        }
    }
}
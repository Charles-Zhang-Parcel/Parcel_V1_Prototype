using System;
using System.IO;
using Parcel.Macros;
using Parcel.Shared.DataTypes;
using Parcel.Toolbox.FileSystem;

namespace Parcel.SetupTest.Test001
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        private static void Program1()
        {
            RegularRenameParameters parameters = new RegularRenameParameters()
            {
            };
            FileSystemMacros.RegularRename(parameters);
        }
    }
}
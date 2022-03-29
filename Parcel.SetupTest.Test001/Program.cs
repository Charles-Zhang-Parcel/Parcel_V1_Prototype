using System;
using System.IO;
using Parcel.Macros;
using Parcel.Toolbox.FileSystem;

namespace Parcel.SetupTest.Test001
{
    class Program
    {
        static void Main(string[] args)
        {
            RegularRenameParameters parameters = new RegularRenameParameters()
            {

            };
            FileSystemMacros.RegularRename(parameters);
        }
    }
}
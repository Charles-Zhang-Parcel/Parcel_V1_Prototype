using System;
using Parcel.Toolbox.FileSystem;

namespace Parcel.Macros
{
    public class RegularRenameParameters
    {
        public string InputFolder { get; set; }
        public string InputPattern { get; set; }
        public string InputReplacement { get; set; }
    }
    
    public static class FileSystemMacros
    {
        public static void RegularRename(RegularRenameParameters parameters)
        {
            FetchParameters fetchParameters = new FetchParameters()
            {
                InputFolder = parameters.InputFolder 
            };
            FileSystemHelper.Fetch(fetchParameters);

            RenameParameters renameParameters = new RenameParameters()
            {
                InputPaths = fetchParameters.OutputFiles,
                InputNamePattern = parameters.InputPattern,
                InputReplacementPattern = parameters.InputReplacement
            };
            FileSystemHelper.Rename(renameParameters);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Csv;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Toolbox.DataProcessing.Nodes;

namespace Parcel.Toolbox.DataProcessing
{
    #region Parameters
    public class CSVParameter
    {
        public string InputPath { get; set; }
        public bool InputContainsHeader { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    #endregion

    public static class DataProcessingHelper
    {
        public static void CSV(CSVParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.InputPath) || !File.Exists(parameter.InputPath))
                throw new ArgumentException("Invalid inputs");

            IEnumerable<ICsvLine> csv = Csv.CsvReader.ReadFromText(File.ReadAllText(parameter.InputPath), new CsvOptions()
            {
                HeaderMode = parameter.InputContainsHeader ? HeaderMode.HeaderPresent : HeaderMode.HeaderAbsent
            });
            parameter.OutputTable = new DataGrid(csv);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class TakeParameter
    {
        public DataGrid InputTable { get; set; }
        public string InputColumnName { get; set; }
        public int InputRowCount { get; set; }
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
        
        public static void Take(TakeParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");

            var column = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName);
            DataGrid newDataGrid = new DataGrid();
            newDataGrid.AddColumnFrom(column, parameter.InputRowCount);
            parameter.OutputTable = newDataGrid;
        }
    }
}
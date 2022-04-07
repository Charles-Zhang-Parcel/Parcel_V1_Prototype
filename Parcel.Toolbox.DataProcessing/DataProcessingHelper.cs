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
    public class ExtractParameter
    {
        public DataGrid InputTable { get; set; }
        public string[] InputColumnNames { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    public class ExcludeParameter
    {
        public DataGrid InputTable { get; set; }
        public string[] InputColumnNames { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    public class SortParameter
    {
        public DataGrid InputTable { get; set; }
        public string InputColumnName { get; set; }
        public bool InputReverseOrder { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    public class AppendParameter
    {
        public DataGrid InputTable1 { get; set; }
        public DataGrid InputTable2 { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    public class TransposeParameter
    {
        public DataGrid InputTable { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    public class SQLParameter
    {
        public DataGrid[] InputTables { get; set; }
        public DataGrid OutputTable { get; set; }
        public ServerConfig OutputConfig { get; set; }
    }
    #endregion

    public static class DataProcessingHelper
    {
        public static void CSV(CSVParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.InputPath) || !File.Exists(parameter.InputPath))
                throw new ArgumentException("Invalid inputs");
            // TODO: Currently if the CSV File is opened by excel then it's not readable by us, and the File.ReadAllText will throw an exception

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
        
        public static void Extract(ExtractParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && parameter.InputColumnNames.Length == 0)
                throw new ArgumentException("No columns are given for the table.");
            if (parameter.InputTable != null && parameter.InputColumnNames.Length != 0
                                             && parameter.InputTable.Columns.Any(c => parameter.InputColumnNames.Contains(c.Header)))
                throw new ArgumentException("Cannot find column with specified name on data table.");
            
            parameter.OutputTable = parameter.InputTable.Extract(parameter.InputColumnNames);
        }
        
        public static void Exclude(ExcludeParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && parameter.InputColumnNames.Length == 0)
                throw new ArgumentException("No columns are given for the table.");
            if (parameter.InputTable != null && parameter.InputColumnNames.Length != 0
                                             && parameter.InputTable.Columns.Any(c => parameter.InputColumnNames.Contains(c.Header)))
                throw new ArgumentException("Cannot find column with specified name on data table.");
            
            parameter.OutputTable = parameter.InputTable.Exclude(parameter.InputColumnNames);
        }
        
        public static void Sort(SortParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");

            var newTable = parameter.InputTable.MakeCopy();
            newTable.Sort(parameter.InputColumnName, parameter.InputReverseOrder);
            parameter.OutputTable = newTable;
        }
        
        public static void Append(AppendParameter parameter)
        {
            if (parameter.InputTable1 == null || parameter.InputTable2 == null)
                throw new ArgumentException("Missing Data Table input.");
            
            parameter.OutputTable = parameter.InputTable1.MakeCopy().Append(parameter.InputTable2);
        }
        
        public static void Transpose(TransposeParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            
            parameter.OutputTable = parameter.InputTable.Transpose();
        }
        
        public static void SQL(SQLParameter parameter)
        {
            if (parameter.InputTables.Length == 0 || parameter.InputTables == null)
                throw new ArgumentException("Missing Data Table input.");

            throw new NotImplementedException();
        }
    }
}
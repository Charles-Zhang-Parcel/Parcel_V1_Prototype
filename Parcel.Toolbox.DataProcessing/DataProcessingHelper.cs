using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Csv;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Toolbox.DataProcessing.Nodes;
using DataTable = System.Data.DataTable;

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
    public class MatrixMultiplyParameter
    {
        public DataGrid[] InputTables { get; set; }
        public bool[] InputTableShouldTransposes { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    public class SQLParameter
    {
        public DataGrid[] InputTables { get; set; }
        public string[] InputTableNames { get; set; }
        public string InputCommand { get; set; }
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
        
        public static void MatrixMultiply(MatrixMultiplyParameter parameter)
        {
            if (parameter.InputTables.Length != parameter.InputTableShouldTransposes.Length)
                throw new ArgumentException("Wrong number of inputs.");
            if (parameter.InputTables.Any(t => t == null))
                throw new ArgumentException("Invalid table inputs.");

            throw new NotImplementedException();
            // parameter.OutputTable = ;
        }
        
        public static void SQL(SQLParameter parameter)
        {
            void PopulateTable(DataGrid table, string tableName, SQLiteConnection connection)
            {
                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"CREATE TABLE '{tableName}'({string.Join(',', table.Columns.Select(c => $"'{c.Header}'"))})";
                cmd.ExecuteNonQuery();
                
                // Remark: The API is as shitty as it can get
                
                SQLiteTransaction transaction = connection.BeginTransaction();
                
                string sql = $"select * from '{tableName}' limit 1";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql,connection);
                adapter.InsertCommand = new SQLiteCommandBuilder(adapter).GetInsertCommand();
                
                DataSet dataSet = new DataSet();
                adapter.FillSchema(dataSet, SchemaType.Source, tableName); 
                adapter.Fill(dataSet, tableName);     // Load exiting table data (will be empty) 

                // Insert data
                DataTable dataTable = dataSet.Tables[tableName];
                foreach (ExpandoObject row in table.Rows)
                {
                    DataRow dataTableRow = dataTable.NewRow();
                    foreach (KeyValuePair<string,dynamic> pair in (IDictionary<string, dynamic>)row)
                        dataTableRow[pair.Key]=pair.Value;
                    dataTable.Rows.Add(dataTableRow);
                }
                int result = adapter.Update(dataTable); 
                
                transaction.Commit(); 
                dataSet.AcceptChanges();
                // Release resources 
                adapter.Dispose();
                dataSet.Clear();
            }
            
            if (parameter.InputTables.Length == 0 || parameter.InputTables == null)
                throw new ArgumentException("Missing Data Table input.");
            
            using (var connection = new SQLiteConnection("Data Source=:memory:"))
            {
                connection.Open();
                
                // Initialize
                for (int i = 0; i < parameter.InputTables.Length; i++)
                {
                    PopulateTable(parameter.InputTables[i], parameter.InputTableNames[i], connection);
                }

                // Execute
                string formattedText = parameter.InputCommand.EndsWith(';')
                    ? parameter.InputCommand
                    : parameter.InputCommand + ';';
                for (int i = 0; i < parameter.InputTables.Length; i++)
                    formattedText = formattedText.Replace($"@Table{i + 1}", $"'{parameter.InputTableNames[i]}'"); // Table names can't use parameters, so do it manually
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(formattedText, connection);
                DataSet result = new DataSet();
                adapter.Fill(result);
                // using (var reader = command.ExecuteReader())
                // {
                //     while (reader.Read())
                //     {
                //         var name = reader.GetString(0);
                //
                //         Console.WriteLine($"Hello, {name}!");
                //     }
                // }
                
                parameter.OutputTable = new DataGrid(result);
                connection.Close();
            }
        }
    }
}
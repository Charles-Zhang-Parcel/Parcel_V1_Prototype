using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Csv;

namespace Parcel.Shared.DataTypes
{
    public class DataColumn
    {
        public string Header { get; set; }
        public List<dynamic> ColumnData { get; set; } = new List<dynamic>();
        public Type ColumnType { get; private set; }

        public void Add<T>(T value)
        {
            if (ColumnData.Count == 0)
                ColumnType = value.GetType();

            if (value.GetType() == ColumnType)
                ColumnData.Add(value);
            else throw new ArgumentException("Wrong type.");
        }

        #region Column Operations
        public double Mean()
        {
            if (ColumnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            IEnumerable<double> list = ColumnData.Cast<double>();
            return list.Average();
        }
        #endregion
    }
    
    public class DataGrid
    {
        #region Constructors
        public DataGrid(){}
        public DataGrid(IEnumerable<ICsvLine> csvLines)
        {
            string[] headers = null;
            foreach (ICsvLine line in csvLines)
            {
                // Initialize columns
                if (headers == null)
                {
                    headers = line.Headers;
                    foreach (string header in headers)
                        Columns.Add(new DataColumn() {Header = header});
                }
                
                // Add data to columns
                for (var i = 0; i < headers.Length; i++)
                    Columns[i].ColumnData.Add(line[i]);
            }
        }
        #endregion

        public List<DataColumn> Columns { get; set; } = new List<DataColumn>();

        #region Accessors
        public List<dynamic> Rows
        {
            get
            {
                int colCount = Columns.Count;
                int rowCount = Columns.First().ColumnData.Count;
                List<dynamic> rows = new List<dynamic>();
                for (int row = 0; row < rowCount; row++)
                {
                    dynamic temp = new ExpandoObject();
                    for (int col = 0; col < colCount; col++)
                        ((IDictionary<String, Object>)temp)[Columns[col].Header] = Columns[col].ColumnData[row];
                    rows.Add(temp);
                }
                return rows;
            }
        }
        #endregion

        #region Editors
        public void AddRow(params object[] values)
        {
            if (values.Length > Columns.Count)
                throw new ArgumentException("Wrong number of row elements.");

            for (int i = 0; i < values.Length; i++)
            {
                Columns[i].Add(values[i]);
            }
        }
        public void AddColumn(string columnName)
        {
            Columns.Add(new DataColumn(){ Header = columnName});
        }
        #endregion
    }
}
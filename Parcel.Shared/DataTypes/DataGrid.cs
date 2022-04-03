using System;
using System.Collections.Generic;
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
            foreach (ICsvLine line in csvLines)
            {
                
            }
        }
        #endregion

        public List<DataColumn> Columns { get; set; } = new List<DataColumn>();

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
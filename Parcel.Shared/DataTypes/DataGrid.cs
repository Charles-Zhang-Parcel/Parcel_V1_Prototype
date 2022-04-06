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
        private List<dynamic> _columnData { get; } = new List<dynamic>();
        private Type _columnType { get; set; }

        #region Accesor
        public void Add<T>(T value)
        {
            if (_columnData.Count == 0)
                _columnType = value.GetType();

            if (value.GetType() != _columnType)
                _columnType = null; // throw new ArgumentException("Wrong type.");
            _columnData.Add(value);
        }
        public int Length => _columnData.Count;
        public dynamic this[int index] => _columnData[index];
        public string TypeName
        {
            get
            {
                if (_columnType == null) return "Mixed";
                else if (_columnType == typeof(double)) return "Number";
                return _columnType.Name;
            }
        }
        #endregion

        #region Column Operations (Math)
        public double Mean()
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            IEnumerable<double> list = _columnData.Cast<double>();
            return list.Average();
        }
        public double Variance(bool population)
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            double[] values = _columnData.Cast<double>().ToArray();
            double variance = 0.0;
            if (values.Count() > 1)
            {
                double avg = values.Average();
                variance += values.Sum(value => Math.Pow(value - avg, 2.0));
            }
            return variance / (population ? values.Length - 1 : values.Length); // For population, use n-1, for sample, use n
        }
        public double STD(bool population)
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            IEnumerable<double> list = _columnData.Cast<double>();
            return Math.Sqrt(Variance(population));
        }
        public double Min()
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            IEnumerable<double> list = _columnData.Cast<double>();
            return list.Min();
        }
        public double Max()
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            IEnumerable<double> list = _columnData.Cast<double>();
            return list.Max();
        }
        public double Sum()
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");

            IEnumerable<double> list = _columnData.Cast<double>();
            return list.Sum();
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
                {
                    if (double.TryParse(line[i], out double value))
                        Columns[i].Add(value);
                    else Columns[i].Add(line[i]);
                }
            }
        }
        #endregion

        public List<DataColumn> Columns { get; set; } = new List<DataColumn>();

        #region Accessors
        public List<dynamic> Rows
        {
            get
            {
                string FormatHeader(int index)
                {
                    var col = Columns[index];
                    return $"{col.Header} ({col.TypeName})";
                }

                int colCount = Columns.Count;
                int rowCount = Columns.First().Length;
                List<dynamic> rows = new List<dynamic>();
                for (int row = 0; row < rowCount; row++)
                {
                    dynamic temp = new ExpandoObject();
                    for (int col = 0; col < colCount; col++)
                        ((IDictionary<String, Object>)temp)[FormatHeader(col)] = Columns[col][row];
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
        public void AddColumnFrom(DataColumn refColumn, int rowCount)
        {
            var column = new DataColumn() {Header = refColumn.Header};
            var count = rowCount == 0 ? refColumn.Length : rowCount;
            for (int i = 0; i < count; i++)
                column.Add(refColumn[i]);
            Columns.Add(column);
        }
        #endregion
    }
}
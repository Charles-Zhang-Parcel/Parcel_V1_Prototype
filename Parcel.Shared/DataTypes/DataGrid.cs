using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Csv;

namespace Parcel.Shared.DataTypes
{
    public class DataColumn
    {
        #region Construction
        public DataColumn(){}
        public DataColumn(string header) => Header = header.Trim().Trim('"');
        public DataColumn(DataColumn other)
        {
            Header = other.Header;
            _columnData = other._columnData.ToList();
            _columnType = other._columnType;
        }
        public DataColumn MakeCopy()
            => new DataColumn(this);
        #endregion

        #region Properties
        public string Header { get; private set; }
        private List<dynamic> _columnData { get; } = new List<dynamic>();
        private Type _columnType { get; set; }
        #endregion

        #region Accessor
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
        public Type Type => _columnType;
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
        public double Correlation(DataColumn other)
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");
            if (this.Length != other.Length)
                throw new InvalidOperationException("Columns are not of same length.");

            double covariance = this.Covariance(other);
            double std1 = this.STD(true);   // Always use n-1 for population
            double std2 = other.STD(true);
            return covariance / (std1 * std2);
        }
        public double Covariance(DataColumn other)
        {
            if (_columnType != typeof(double))
                throw new InvalidOperationException("Column is not of numerical type.");
            if (this.Length != other.Length)
                throw new InvalidOperationException("Columns are not of same length.");

            double[] values1 = _columnData.Cast<double>().ToArray();
            double[] values2 = other._columnData.Cast<double>().ToArray();
            double variance = 0.0;
            if (values1.Count() > 1)
            {
                double avg1 = values1.Average();
                double avg2 = values1.Average();
                for (int i = 0; i < values1.Length; i++)
                    variance += (values1[i] - avg1) * (values2[i] - avg2);
            }
            return variance / (values1.Length - 1); // Always use n-1 for population
        }
        #endregion
    }
    
    public class DataGrid
    {
        #region Constructors
        public DataGrid(){}
        public DataGrid(ExpandoObject expando)
        {
            var dict = (IDictionary<string, object>)expando;
            foreach (string key in dict.Keys)
            {
                var col = new DataColumn(key);
                col.Add(dict[key]);
                Columns.Add(col);
            }
        }
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
                        Columns.Add(new DataColumn(header));
                }
                
                // Add data to columns
                for (var i = 0; i < headers.Length; i++)
                {
                    // Perform pre-formatting
                    if (double.TryParse(line[i], out double number))
                        Columns[i].Add(number);
                    else if (DateTime.TryParse(line[i], out DateTime dateTime))
                        Columns[i].Add(dateTime);
                    else Columns[i].Add(line[i]);
                }
            }
        }
        #endregion

        public List<DataColumn> Columns { get; set; } = new List<DataColumn>();
        public DataColumn OptionalRowHeaderColumn { get; set; }

        #region Accessors
        public int ColumnCount => Columns.Count;
        public int RowCount => Columns.First().Length;
        public List<dynamic> Rows
        {
            get
            {
                int colCount = Columns.Count;
                int rowCount = Columns.First().Length;
                List<dynamic> rows = new List<dynamic>();
                for (int row = 0; row < rowCount; row++)
                {
                    dynamic temp = new ExpandoObject();
                    if (OptionalRowHeaderColumn != null)
                        ((IDictionary<String, Object>)temp)[OptionalRowHeaderColumn.Header] = OptionalRowHeaderColumn[row];
                    for (int col = 0; col < colCount; col++)
                        ((IDictionary<String, Object>)temp)[Columns[col].Header] = Columns[col][row];
                    rows.Add(temp);
                }
                return rows;
            }
        }
        #endregion

        #region Editors (In-Place Operations)
        public void AddRow(params object[] values)
        {
            if (values.Length > Columns.Count)
                throw new ArgumentException("Wrong number of row elements.");

            for (int i = 0; i < values.Length; i++)
            {
                Columns[i].Add(values[i]);
            }
        }
        public DataColumn AddColumn(string columnName)
        {
            var newColumn = new DataColumn(columnName);
            Columns.Add(newColumn);
            return newColumn;
        }
        public void AddOptionalRowHeaderColumn(string columnName)
        {
            OptionalRowHeaderColumn = new DataColumn(columnName);
        }
        public DataColumn AddColumnFrom(DataColumn refColumn, int rowCount)
        {
            var column = new DataColumn(refColumn.Header);
            var count = rowCount == 0 ? refColumn.Length : rowCount;
            for (int i = 0; i < count; i++)
                column.Add(refColumn[i]);
            Columns.Add(column);
            return column;
        }
        public void Sort(string anchorColumnName, bool reverseOrder)
        {
            var result = reverseOrder 
                ? Rows.OrderByDescending(r => ((IDictionary<String, Object>) r)[anchorColumnName]).ToArray()
                : Rows.OrderBy(r => ((IDictionary<String, Object>) r)[anchorColumnName]).ToArray();
            var names = Columns.Select(c => c.Header);
            Columns = names.Select(name =>
            {
                var col = new DataColumn(name);
                foreach (dynamic expando in result)
                    col.Add(((IDictionary<String, Object>) expando)[name]);
                return col;
            }).ToList();
        }
        #endregion

        #region Copy Operations
        public DataGrid MakeCopy()
        {
            DataGrid result = new DataGrid();
            IEnumerable<DataColumn> columnCopies = this.Columns
                .Select(c => c.MakeCopy());
            result.Columns = columnCopies.ToList();
            return result;
        }
        public DataGrid Append(DataGrid other)
        {
            DataGrid result = MakeCopy();
            result.Columns.AddRange(other.Columns.Select(c => c.MakeCopy()));
            return result;
        }
        public DataGrid Extract(string[] names)
        {
            DataGrid result = new DataGrid();
            IEnumerable<DataColumn> columnCopies = this.Columns
                .Where(c => names.Contains(c.Header))
                .Select(c => c.MakeCopy());
            result.Columns = columnCopies.ToList();
            return result;
        }
        public DataGrid Exclude(string[] names)
            => this.Extract(Columns.Select(c => c.Header).Except(names).ToArray());
        public DataGrid Transpose()
        {
            DataGrid result = new DataGrid();
            // Create optional column to hold existing headers
            result.AddOptionalRowHeaderColumn("Header");
            foreach (DataColumn column in this.Columns)
                result.OptionalRowHeaderColumn.Add(column.Header);
            
            // Create data columns
            for (int i = 0; i < this.Columns.First().Length; i++)
                result.AddColumn($"Value {i+1}");
                
            // Copy values over
            foreach (DataColumn column in this.Columns)
                for (int row = 0; row < column.Length; row++)
                    result.Columns[row].Add(column[row]);

            return result;
        }
        #endregion

        #region Numerical Computation
        public DataGrid CorrelationMatrix()
        {
            DataGrid result = new DataGrid();
            
            // Define columns
            result.AddColumn("(Relation)"); // Add 
            foreach (DataColumn column in Columns)
                result.AddColumn(column.Header);
            // Compute data
            AddOptionalRowHeaderColumn(string.Empty);
            foreach (DataColumn column in Columns)
            {
                OptionalRowHeaderColumn.Add(column.Header);
                result.AddRow(Columns.Select(other => other.Covariance(column)));
            }
            
            return result;
        }
        #endregion
    }
}
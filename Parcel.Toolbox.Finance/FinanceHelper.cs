using System;
using System.Linq;
using Parcel.Shared.DataTypes;

namespace Parcel.Toolbox.Finance
{
    public class RobustMeanParameter
    {
        public double[] InputList { get; set; }
        public DataGrid InputTable { get; set; }
        public string InputColumnName { get; set; }

        /// <remarks>GUI can provide more specific dynamic behavior for this parameter based on Table Input,
        /// e.g. hide it when InputTable is not hooked, and provide enumeration if input table contains headers</remarks>
        public int InputColumnSelection { get; set; } = -1;
        
        public double[] OutputList { get; set; }
        public DataGrid OutputTable { get; set; }
        public double OutputValue { get; set; }
    }

    public class BaseColumnOperationParameter
    {
        public DataGrid InputTable { get; set; }
        public string InputColumnName { get; set; }
        public double OutputValue { get; set; }
    }
    public class MeanParameter : BaseColumnOperationParameter {}
    public class VarianceParameter: BaseColumnOperationParameter {}
    public class StandardDeviationParameter: BaseColumnOperationParameter {}
    public class MinParameter : BaseColumnOperationParameter {}
    public class MaxParameter : BaseColumnOperationParameter {}
    public class SumParameter : BaseColumnOperationParameter {}
    
    public static class FinanceHelper
    {
        public static void RobustMean(RobustMeanParameter parameter)
        {
            if (parameter.InputList == null && parameter.InputTable == null)
                throw new ArgumentException("Invalid inputs");
            if (parameter.InputList != null && parameter.InputTable != null)
                throw new ArgumentException("Invalid inputs");
            if (parameter.InputTable != null && parameter.InputColumnSelection == -1 && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && parameter.InputColumnSelection != -1 
                                             && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            if (parameter.InputList != null)
                parameter.OutputValue = parameter.InputList.Sum();
            if (parameter.InputTable != null)
            {
                if (parameter.InputColumnSelection != -1)
                    parameter.OutputValue = parameter.InputTable.Columns[parameter.InputColumnSelection].Mean();
                else if (!string.IsNullOrWhiteSpace(parameter.InputColumnName))
                    parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).Mean();
            }

            parameter.OutputList = parameter.InputList;
            parameter.OutputTable = parameter.InputTable;
        }
        
        public static void Mean(MeanParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).Mean();
        }
        public static void Variance(VarianceParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).Variance();
        }
        
        public static void StandardDeviation(StandardDeviationParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).STD();
        }
        
        public static void Min(MinParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).Min();
        }
        
        public static void Max(MaxParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).Max();
        }
        
        public static void Sum(SumParameter parameter)
        {
            if (parameter.InputTable == null)
                throw new ArgumentException("Missing Data Table input.");
            if (parameter.InputTable != null && string.IsNullOrWhiteSpace(parameter.InputColumnName))
                throw new ArgumentException("No column selection is given for the table");
            if (parameter.InputTable != null && !string.IsNullOrWhiteSpace(parameter.InputColumnName)
                                             && parameter.InputTable.Columns.All(c => c.Header != parameter.InputColumnName))
                throw new ArgumentException("Cannot find column with specified name on data table");
            
            parameter.OutputValue = parameter.InputTable.Columns.Single(c => c.Header == parameter.InputColumnName).Sum();
        }

        public static void Correlation()
        {
            
        }

        public static void Covariance()
        {
            
        }
        
        public static void CovarianceMatrix()
        {}
        
    }
}
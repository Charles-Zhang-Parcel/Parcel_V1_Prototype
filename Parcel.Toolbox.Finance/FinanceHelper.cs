using System;
using System.Linq;
using Parcel.Shared.DataTypes;

namespace Parcel.Toolbox.Finance
{
    public class MeanParameter
    {
        public double[] InputList { get; set; }
        public DataGrid InputTable { get; set; }

        /// <remarks>GUI can provide more specific dynamic behavior for this parameter based on Table Input,
        /// e.g. hide it when InputTable is not hooked, and provide enumeration if input table contains headers</remarks>
        public int InputColumnSelection { get; set; } = -1;
        
        public double[] OutputList { get; set; }
        public DataGrid OutputTable { get; set; }
        public double OutputValue { get; set; }
    }
    public class VarianceParameter
    {
        
    }

    public class StandardDeviationParameter
    {
        
    }
    
    public static class FinanceHelper
    {
        public static void Mean(MeanParameter parameter)
        {
            if (parameter.InputList == null && parameter.InputTable == null)
                throw new ArgumentException("Invalid inputs");
            if (parameter.InputList != null && parameter.InputTable != null)
                throw new ArgumentException("Invalid inputs");
            if (parameter.InputTable != null && parameter.InputColumnSelection == -1)
                throw new ArgumentException("No column selection is given for the table");
            
            if (parameter.InputList != null)
                parameter.OutputValue = parameter.InputList.Sum();
            if (parameter.InputTable != null)
                parameter.OutputValue = parameter.InputTable.Columns[parameter.InputColumnSelection].Mean();

            parameter.OutputList = parameter.InputList;
            parameter.OutputTable = parameter.InputTable;
        }
        
        public static void Variance(VarianceParameter parameter)
        {
            
        }
        
        public static void StandardDeviation(StandardDeviationParameter parameter)
        {
            
        }
    }
}
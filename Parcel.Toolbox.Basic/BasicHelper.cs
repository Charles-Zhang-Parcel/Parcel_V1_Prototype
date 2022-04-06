using System;
using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Basic
{
    #region Parameters
    public class BaseNumericalParameter
    {
        public double InputNumber1 { get; set; }
        public double InputNumber2 { get; set; }
        public double OutputNumber { get; set; }
    }
    public class AddParameter: BaseNumericalParameter
    {
    }
    public class SubtractParameter: BaseNumericalParameter
    {
    }
    public class MultiplyParameter: BaseNumericalParameter
    {
    }
    public class DivideParameter: BaseNumericalParameter
    {
    }
    public class ModulusParameter: BaseNumericalParameter
    {
    }
    public class PowerParameter: BaseNumericalParameter
    {
    }
    #endregion
    
    public static class BasicHelper
    {
        public static void Add(AddParameter parameter)
        {
            parameter.OutputNumber = parameter.InputNumber1 + parameter.InputNumber2;
        }
        
        public static void Subtract(SubtractParameter parameter)
        {
            parameter.OutputNumber = parameter.InputNumber1 - parameter.InputNumber2;
        }
        
        public static void Multiply(MultiplyParameter parameter)
        {
            parameter.OutputNumber = parameter.InputNumber1 * parameter.InputNumber2;
        }
        
        public static void Divide(DivideParameter parameter)
        {
            if (parameter.InputNumber2 == 0)
                throw new ArgumentException("Parameter 2 cannot be zero.");

            parameter.OutputNumber = parameter.InputNumber1 / parameter.InputNumber2;
        }
        
        public static void Modulus(ModulusParameter parameter)
        {
            parameter.OutputNumber = parameter.InputNumber1 % parameter.InputNumber2;
        }
        
        public static void Power(PowerParameter parameter)
        {
            parameter.OutputNumber = Math.Pow(parameter.InputNumber1, parameter.InputNumber2);
        }
    }
}
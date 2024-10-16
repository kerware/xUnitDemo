using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitDemo
{
    public class Calculator : ICalculator
    {
        private double lastResult;
        public Calculator()
        {
        }

        public virtual double getLastResult()
        {
            return lastResult;
        }

        public virtual double add(double x, double y)
        {
            lastResult =  x + y;
            return lastResult;
        }

        public virtual void clear()
        {
            lastResult = 0;
        }

        public virtual double divide(double x, double y)
        {
            if (y == 0) throw new DivideByZeroException();
            lastResult = x / y;
            return (lastResult);
        }

       

        public virtual double multiply(double x, double y)
        {
            lastResult = x * y;
            return lastResult;
        }

        public virtual double subtract(double x, double y)
        {
            lastResult =  x - y;
            return lastResult;
        }
    }
}

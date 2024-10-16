using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitDemo
{
    public interface ICalculator
    {
        public double add( double x,double y );
        public double subtract( double x,double y );

        public double multiply( double x,double y );
        public double divide( double x,double y );

        public double getLastResult();

        public void clear();

    }
}

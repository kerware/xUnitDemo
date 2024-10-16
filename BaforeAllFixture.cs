using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace xUnitDemo
{
    public class BeforeAllFixture : IDisposable
    {
        private ICalculator _calculator;
        public BeforeAllFixture( )
        {
            _calculator = new Calculator();
            
        }

        
      
        public void Dispose()
        {
            _calculator.clear();
        }

        public ICalculator Calculator { get { return _calculator; } }


       

    }
}

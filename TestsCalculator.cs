using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System.Reflection;
using System.Transactions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitDemo
{
    public class MonBeforeAfterTestAttribute : BeforeAfterTestAttribute
    {
        
        public override void Before(MethodInfo methodUnderTest)
        {  
            Console.WriteLine($"Avant le test: {methodUnderTest.Name}");
            // Ajouter ici toute logique à exécuter avant chaque test
        }

        public override void After(MethodInfo methodUnderTest)
        {
            Console.WriteLine($"Après le test: {methodUnderTest.Name}");
            // Ajouter ici toute logique à exécuter après chaque test
        }
    }

    public class CalculatorEngine
    {
        private ICalculator _calculator;

        public CalculatorEngine( ICalculator calculator )
        {
            _calculator = calculator;
        }

        public double computeSum( int p )
        {
            double sum = 0;
            for( int i=1; i<=p; i++ ) { 
                 sum += _calculator.add( 0 , i );
            }
            return sum;
        }
    }

    public class TestsCalculator : IClassFixture<BeforeAllFixture>{

        private readonly BeforeAllFixture _fixture;


        public TestsCalculator( BeforeAllFixture fixture )
        {
            _fixture = fixture;
        }
        public static IEnumerable<object[]> GetDataFromCsv()
        {
            var csvFilePath = "C:\\Users\\Administrateur\\source\\repos\\xUnitDemo\\testdata.csv";
            var csvLines = File.ReadAllLines(csvFilePath);
            foreach (var line in csvLines)
            {
                var values = line.Split(',');
                yield return new object[] { double.Parse(values[0]), double.Parse(values[1]), double.Parse(values[2]) };
            }
        }

    
        [Fact]
        public void TestNominalCalculator()
        {
            // Arrange
            ICalculator c = _fixture.Calculator;
            // Act 
            double r = c.add(5, 5);
            // Assert
            Assert.Equal(10.0, r);
            Assert.Equal(10.0, c.getLastResult());
        }

        [Theory]
        [InlineData(5,5,10)]
        [InlineData(10,-5,5)]
        public void TestCalculatorAdd( double x, double y , double expectedRes) {
            // Arrange
            ICalculator c = _fixture.Calculator;
            // Act 
            double actualRes = c.add(x,y);
            // Assert
            Assert.Equal(expectedRes, actualRes);
            Assert.Equal(expectedRes, c.getLastResult());
        }

        [Theory]
        [MemberData(nameof(GetDataFromCsv))]
        [MonBeforeAfterTest]
        public void TestCalculatorAddWithExternalData(double x, double y, double expectedRes)
        {
            // Arrange
            ICalculator c = _fixture.Calculator;
            // Act 
            double actualRes = c.add(x, y);
            // Assert
            Assert.Equal(expectedRes, actualRes);
            Assert.Equal(expectedRes, c.getLastResult());

        }

        [Fact]
        [Trait("Category","TU")]
        [Trait("Priority","High")]
        // On peut lancer en ligne de commande :
        // dotnet test --filter "Trait=Category=TU&Trait=Priority=High"
        public void TestDivisionByZero()
        {
            // Arrange
            ICalculator c = _fixture.Calculator; 
            // Assert with Act
            Action action = () => { c.divide(5, 0); };
            Assert.Throws<DivideByZeroException>(action);
        }

        // Démonstration Stub Spy Mock

        [Fact]
        public void ShouldReturnWithStubbedResponseOnAMock()
        {
            // Arrange
            // On mock une Interface et on programme le mock
            var mockCalculator = new Mock<ICalculator>();
            mockCalculator.Setup( calc => calc.add( It.IsAny<double>(), It.IsAny<double>())).Returns(5.0);
            // Act
            var result = mockCalculator.Object.add( 10, -5);
            // Assert
            Assert.Equal(5, result);
        }


        [Fact]
        public void ShouldSpyAMock()
        {
            // Arrange
            // On mock une Interface et on programme le mock
            var mockCalculator = new Mock<ICalculator>();
            mockCalculator.Setup(calc => calc.add(It.IsAny<double>(), It.IsAny<double>())).Returns(5.0);
          
            CalculatorEngine ce = new CalculatorEngine(mockCalculator.Object);
            // Act
            var result = ce.computeSum(5);
            // Assert
            Assert.Equal(25, result);
            mockCalculator.Verify(
                calc => calc.add(It.IsAny<double>(), It.IsAny<double>()), 
                Times.Exactly(5), "Add method should be called exactly 5 times");
        }


        [Fact]
        public void ShouldStubAndSpyARealInstance()
        {
            // Arrange
            // On mock une Interface et on programme le mock
            var mockCalculator = new Mock<Calculator> { CallBase = true };
            
            mockCalculator.Setup(calc => calc.add(0.0, 4.0)).Returns(3.0);
            CalculatorEngine ce = new CalculatorEngine(mockCalculator.Object);

            // Act
            var result = ce.computeSum(5);
            // Assert
            // n * (N+1) /2   5*6/2 = 15  ( avec le 0 + 4 = 3 stubbé
            // on a 14 au lieu de 15)
            Assert.Equal(14, result);
            mockCalculator.Verify(
                calc => calc.add(It.IsAny<double>(), It.IsAny<double>()),
                Times.Exactly(5), "Add method should be called exactly 5 times");
        }

    }
    
}
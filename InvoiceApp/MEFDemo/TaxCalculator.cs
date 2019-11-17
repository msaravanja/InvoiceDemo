using System.ComponentModel.Composition;

namespace MEFDemo
{
    public interface ITaxCalculator
    {
        decimal CalculateTax(decimal amount, decimal taxPercentage);
    }

    [Export(typeof(ITaxCalculator))]
    public class TaxCalculator : ITaxCalculator
    {
        public decimal CalculateTax(decimal amount, decimal taxPercentage)
        {
            return (amount * (1 + (taxPercentage / 100)));
        }
    }
}

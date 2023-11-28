using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace OO_programming
{
    public class TaxRate
    {
        private List<TaxRate> taxRatesWithThreshold;
        private List<TaxRate> taxRatesNoThreshold;

        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }
        public decimal RateA { get; set; }
        public decimal RateB { get; set; }
        public decimal grosspaylocal {  get; set; }
        public decimal result { get; set; }


        public decimal CalculateTax(decimal grossPay, bool hasThreshold)
        {
            TaxRate taxRateCalculator = new TaxRate();
            grosspaylocal = grossPay;
            if (hasThreshold == true)
            {
                taxRateCalculator.LoadTaxRatesWithThreshold(
                @"C:\Users\cc\Downloads\Cl_OOProgramming_AE_Pro_Appx (1)\Part 3 application files\taxrate-withthreshold.csv");
                }
            else
            {
                taxRateCalculator.LoadTaxRatesNoThreshold(

                @"C:\Users\cc\Downloads\Cl_OOProgramming_AE_Pro_Appx (1)\Part 3 application files\taxrate-nothreshold.csv");
            }

            if (hasThreshold == true)
            {
                foreach (var taxRate in taxRateCalculator.taxRatesWithThreshold)
                {
                    if (grosspaylocal >= taxRate.LowerBound && grosspaylocal < taxRate.UpperBound)
                    {
                        result = Math.Round(taxRate.RateA * (grosspaylocal + 0.99m) - taxRate.RateB);
                        return result;
                    }

                }
            }
            else
            {
                foreach (var taxRate in taxRateCalculator.taxRatesNoThreshold)
                {
                    if (grosspaylocal >= taxRate.LowerBound && grosspaylocal < taxRate.UpperBound)
                    {
                        result = Math.Round(taxRate.RateA * (grosspaylocal + 0.99m) - taxRate.RateB);
                        return result;
                    }
                }
            }
           return result;

        }

        public void LoadTaxRatesWithThreshold(string filePath)
        {
            taxRatesWithThreshold = ReadTaxRates(filePath);
            
        }
        public void LoadTaxRatesNoThreshold(string filePath)
        {
            taxRatesNoThreshold = ReadTaxRates(filePath);
        }

        private List<TaxRate> ReadTaxRates(string filePath)
        {
            List<TaxRate> taxRates = new List<TaxRate>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] values = line.Split(',');

                        if (values.Length >= 4 &&
                            decimal.TryParse(values[0], out decimal minIncome) &&
                            decimal.TryParse(values[1], out decimal maxIncome) &&
                            decimal.TryParse(values[2], out decimal rateA) &&
                            decimal.TryParse(values[3], out decimal rateB)) 
                        {
                            TaxRate taxRate = new TaxRate
                            {
                                LowerBound = minIncome,
                                UpperBound = maxIncome,
                                RateA = rateA,
                                RateB = rateB

                            };
                            taxRates.Add(taxRate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading tax rate file: " + ex.Message);
            }

            return taxRates;
        }
    }
}

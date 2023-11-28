﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace OO_programming
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    /// <summary>
    /// Class to capture details associated with an employee's pay slip record
    /// </summary>
    public class PaySlip
    {
        public PaySlip() { }
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string HourlyRate { get; set; }
        public string TaxThreshold { get; set; }

        public PaySlip(string employeeID, string firstName, string lastName, string department, string hourlyrate, string taxthreshold)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Department = department;
            HourlyRate = hourlyrate;
            TaxThreshold = taxthreshold;

            // Initialize other properties
            InitializeTaxRates(employeeID); // Pass the employee ID
        }

        private void InitializeTaxRates(string employeeID)
        {
            // Read and process tax threshold data for this employee from employee.csv file
            string employeeFile = @"C:/Users/cc/Downloads/Cl_OOProgramming_AE_Pro_Appx (1)/Part 3 application files/employee.csv";

            // Read the employee CSV file
            List<string[]> employeeData = ReadCsvFile(employeeFile);

            // Find the employee's tax threshold status based on the employee ID
            foreach (var record in employeeData)
            {
                if (record[0] == employeeID) // Assuming employee ID is at index 0 in the CSV
                {
                    TaxThreshold = record[5]; // Change index to the appropriate column where tax threshold status is located in your CSV
                    break;
                }
            }
        }

        private List<string[]> ReadCsvFile(string filePath)
        {
            List<string[]> data = new List<string[]>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] values = line.Split(','); // this assumes the .csv file is comma-separated

                        data.Add(values);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading CSV file: " + ex.Message);
            }

            return data;
        }

        public string FullEmployeeInfo
        {
            get { return $"{EmployeeID} - {FirstName} {LastName}"; }
        }
    }

    /// <summary>
    /// Base class to hold all Pay calculation functions
    /// Default class behavior is tax calculated with tax threshold applied
    /// </summary>
    public class PayCalculator
    {
        public static Dictionary<string, decimal> TaxThresholds = new Dictionary<string, decimal>();

        public static object WithThreshold { get; internal set; }

        // Method to calculate tax amount based on employee ID
        public virtual decimal CalculateTax(decimal grossPay, string employeeID)
        {
            if (TaxThresholds.ContainsKey(employeeID))
            {
                decimal taxThreshold = TaxThresholds[employeeID];

            }
            return 0; // Default to no tax if threshold not found
        }

        // Method to calculate gross pay
        public virtual decimal CalculateGrossPay(decimal hourlyRate, decimal hoursWorked)
        {
            return hourlyRate * hoursWorked;
        }

        // Method to calculate net pay
        public virtual decimal CalculateNetPay(decimal grossPay, decimal tax)
        {
            return grossPay - tax;
        }

        // Method to calculate superannuation
        public virtual decimal CalculateSuperannuation(decimal grossPay, decimal superannuationRate)
        {
            return grossPay * (superannuationRate / 100);
        }
        public virtual decimal CalculateTax(decimal grossPay)
        {
            return 0; // No tax applied since there's no tax threshold
        }
    }

    public class PayCalculatorNoThreshold : PayCalculator
    {
        // Class to hold the pay calculator nothreshold classes
    }
    public class PayCalculatorWithThreshold : PayCalculator
    {
        // Override CalculateTax method to apply tax threshold
        public override decimal CalculateTax(decimal grossPay)
        {
            return base.CalculateTax(grossPay); // Use base class tax calculation with threshold applied
        }
    }
}


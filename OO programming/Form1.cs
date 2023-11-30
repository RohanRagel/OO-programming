using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace OO_programming
{
    /// <summary>
    /// form 1 class
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// tax to parse, called at the bottom of the page in "CalculateTax"
        /// </summary>
        public decimal taxtoparse {  get; set; }
        public Form1()
        {
            InitializeComponent();
            // Initialize a list to store PaySlip objects

            List<PaySlip> paySlips = new List<PaySlip>();

            // Read the employee.csv file and populate the list
            string[] lines = File.ReadAllLines(@"C:/Users/cc/Downloads/Cl_OOProgramming_AE_Pro_Appx (1)/Part 3 application files/employee.csv");
            foreach (string line in lines)
            {
                string[] data = line.Split(','); //If the file is csv this will work
                if (data.Length >= 6) //number of columns
                {
                    
                    // Defining where the below values are located within the CSV
                    PaySlip paySlip = new PaySlip
                    {
                        EmployeeID = data[0].Trim(),
                        FirstName = data[1].Trim(),
                        LastName = data[2].Trim(),
                        Department = data[3].Trim(),
                        HourlyRate = data[4].Trim(),
                        TaxThreshold = data[5].Trim()

                    };
                    paySlips.Add(paySlip);
                }
            }

            // Bind the list to listBox1
            listBox1.DataSource = paySlips;
            listBox1.DisplayMember = "FullEmployeeInfo";


            // Add code below to complete the implementation to populate the listBox
            // by reading the employee.csv file into a List of PaySlip objects, then binding this to the ListBox.
            // CSV file format: <employee ID>, <first name>, <last name>, <hourly rate>,<taxthreshold>

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Ensure an item is selected in the listBox1
            if (listBox1.SelectedItem != null && listBox1.SelectedItem is PaySlip)
            {
                // Cast the selected item back to PaySlip to access its properties
                PaySlip selectedPaySlip = (PaySlip)listBox1.SelectedItem;

                // Retrieve necessary data from the form controls
                //retrieve line below from payslip
                decimal hourlyRate = Convert.ToDecimal(selectedPaySlip.HourlyRate); ;
                // retrieve from UI
                decimal hoursWorked = Convert.ToDecimal(textBox1.Text);
                string employeeID = selectedPaySlip.EmployeeID;

                string firstName = selectedPaySlip.FirstName;
                string lastName = selectedPaySlip.LastName;
                string department = selectedPaySlip.Department;
                string hourlyrate = selectedPaySlip.HourlyRate;
                string taxthreshold = selectedPaySlip.TaxThreshold;

                bool Flag = false;
                if (taxthreshold == "Y")
                {
                    Flag = true;
                }


                
                PaySlip selectedEmployee = new PaySlip(employeeID, firstName, lastName, department, hourlyrate, taxthreshold);
                PayCalculatorNoThreshold calculatorNoThreshold = new PayCalculatorNoThreshold();
                PayCalculatorWithThreshold calculatorWithThreshold = new PayCalculatorWithThreshold();
                TaxRate TCalc = new TaxRate();


                // Calculate gross pay, tax, net pay, and superannuation using the selected calculator
                decimal grossPay = calculatorWithThreshold.CalculateGrossPay(hourlyRate, hoursWorked);
                decimal tax = TCalc.CalculateTax(grossPay, Flag);
                taxtoparse = tax;
                decimal netPay = calculatorWithThreshold.CalculateNetPay(grossPay, tax);
                decimal superannuation = calculatorWithThreshold.CalculateSuperannuation(grossPay, 11);

                // Display the calculated values in respective TextBoxes
                textBox2.Text = $"Employee details: {employeeID} - {firstName} {lastName} {department}\r\n" +
                                             $"Hours Worked: {hoursWorked}\r\n" +
                                             $"Hourly Rate: {hourlyRate}\r\n" +
                                             $"Tax Threshold: {taxthreshold}\r\n" +
                                             $"Gross Pay: {grossPay}\r\n" +
                                             $"Tax: {tax}\r\n" +
                                             $"Net Pay: {netPay}\r\n" +
                                             $"Superannuation: {superannuation}";
            }


        }

        private void button2_Click(object sender, EventArgs e) //save button
        {
            if (listBox1.SelectedItem != null && listBox1.SelectedItem is PaySlip selectedPaySlip)
            {
                string employeeID = selectedPaySlip.EmployeeID;
                string fullName = $"{selectedPaySlip.FirstName} {selectedPaySlip.LastName}";
                decimal hoursWorked;
                decimal.TryParse(textBox1.Text, out hoursWorked); // Parse hours worked

                decimal hourlyRate;
                decimal.TryParse(selectedPaySlip.HourlyRate, out hourlyRate); // Parse hourly rate

                string taxThreshold = selectedPaySlip.TaxThreshold;


                decimal grossPay = CalculateGrossPay(hourlyRate, hoursWorked);
                decimal tax = CalculateTax(grossPay);
                decimal netPay = CalculateNetPay(grossPay, tax);
                decimal superannuation = CalculateSuperannuation(grossPay, 11); // 11 is the super rate

                string fileName = $"C:/Users/cc/Downloads/Cl_OOProgramming_AE_Pro_Appx (1)/Part 3 application files/Pay-EmployeeID-Fullname-28.11.csv";

                using (var writer = new StreamWriter(fileName, true)) //true appends to the file, so it doesnt replace what is already there
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(new[]
                    {
                new //adding the necessary values
                {
                    EmployeeId = employeeID,
                    FullName = fullName,
                    HoursWorked = hoursWorked,
                    HourlyRate = hourlyRate,
                    TaxThreshold = taxThreshold,
                    GrossPay = grossPay,
                    Tax = tax,
                    NetPay = netPay,
                    Superannuation = superannuation
                }
            });
                }

                MessageBox.Show("Payment details saved successfully!");
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        /// <summary>
        /// Method to calculate gross pay
        /// </summary>
        /// <param name="hourlyRate"></param>
        /// <param name="hoursWorked"></param>
        /// <returns>hourly rate x hours worked</returns>
        public virtual decimal CalculateGrossPay(decimal hourlyRate, decimal hoursWorked)
        {
            return hourlyRate * hoursWorked;
        }

        /// <summary>
        /// Method to calculate net pay
        /// </summary>
        /// <param name="grossPay"></param>
        /// <param name="tax"></param>
        /// <returns>gross pay - tax</returns>
        public virtual decimal CalculateNetPay(decimal grossPay, decimal tax)
        {
            return grossPay - tax;
        }

        /// <summary>
        /// Method to calculate superannuation
        /// </summary>
        /// <param name="grossPay"></param>
        /// <param name="superannuationRate"></param>
        /// <returns> -11% of the gross pay</returns>
        public virtual decimal CalculateSuperannuation(decimal grossPay, decimal superannuationRate)
        {
            return grossPay * (superannuationRate / 100);
        }
        /// <summary>
        /// submits the tax calculation into the .csv file 
        /// </summary>
        /// <param name="grossPay"></param>
        /// <returns></returns>
        public virtual decimal CalculateTax(decimal grossPay)
        {
            return taxtoparse; 
        }

        // Add code below to complete the implementation for saving the
        // calculated payment data into a csv file.
        // File naming convention: Pay_<full name>_<datetimenow>.csv
        // Data fields expected - EmployeeId, Full Name, Hours Worked, Hourly Rate, Tax Threshold, Gross Pay, Tax, Net Pay, Superannuation

    }

}
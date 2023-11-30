using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OO_programming
{
    public class CSVHelper
    {
        /// <summary>
        /// this is to parse the csv file through the file path
        /// </summary>
        /// <param name="filePath"></param>
        public void ParseCsvFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {


                IEnumerable<dynamic> records = csv.GetRecords<dynamic>();

                foreach (var record in records)
                {
                    foreach (var property in record)
                    {
                        var columnName = property.Key;
                        var columnValue = property.Value;

                    }
                }
            }
        }
    }
}
        
    


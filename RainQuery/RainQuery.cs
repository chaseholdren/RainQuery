using System;
using System.Collections.Generic;
using System.Linq;

namespace RainQuery
{
    class RainQuery
    {
        List<RainMeasurement> rainMeasurementList;

        public RainQuery()
        {
            rainMeasurementList = LoadPrecipitationData();
        }

        public void Run()
        {
            do
            {
                Console.WriteLine("You can query the database.\n1. Daily Value\n2. Range statistic");

                var input = Console.ReadKey().KeyChar;

                switch (input)
                {
                    case ('1'):
                        var dateInput = GetYearMonthDayInput("\nEnter year, month, day");
                        Console.WriteLine(ViewDailyValue(dateInput));
                        break;
                    case ('2'):
                        var startDate = GetYearMonthDayInput("\nEnter start year, month, day");
                        var endDate = GetYearMonthDayInput("\nEnter end year, month, day");
                        Console.WriteLine(ViewRangeStatistic(startDate, endDate));
                        break;
                    default:
                        Console.WriteLine("\nThat is not a valid option.");
                        break;
                }
            } while (true);
        }

        private static List<RainMeasurement> LoadPrecipitationData()
        {
            string[] monthDataList = System.IO.File.ReadAllLines("precipitation.txt");
            List<RainMeasurement> rainMeasurements = new List<RainMeasurement>();

            foreach (string monthData in monthDataList)
            {
                var splitData = monthData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var year = int.Parse(splitData.First());
                splitData.RemoveAt(0);
                var month = int.Parse(splitData.First());
                splitData.RemoveAt(0);
                var measurementList = splitData.Select(measurement => double.Parse(measurement)).TakeWhile(measurement => measurement >= 0).ToList();

                for (int day = 1; day < measurementList.Count; day++)
                {
                    rainMeasurements.Add(new RainMeasurement(year, month, day, measurementList[day - 1]));
                }
            }

            return rainMeasurements;
        }

        private string ViewDailyValue(DateTime dateInput)
        {
            var rainMeasurement = rainMeasurementList.Single(measurement => measurement.date == dateInput);
            var rainMeasurementString = rainMeasurement.measurement.ToString();

            return $"The value is {rainMeasurementString}";
        }

        private string ViewRangeStatistic(DateTime startDate, DateTime endDate)
        {
            var rainMeasurementRange = rainMeasurementList
                .Where(measurement => startDate < measurement.date && measurement.date < endDate)
                .Select(measurement => measurement.measurement);

            var total = rainMeasurementRange.Count();
            var average = rainMeasurementRange.Average();
            var maximum = rainMeasurementRange.Max();
            var minimum = rainMeasurementRange.Min();

            return $"Total {total} points, average={average}, maximum={maximum}, minimum={minimum}.";
        }

        private static DateTime GetYearMonthDayInput(string message, string invalidInputMessage = "That input wasn't formatted correctly, try again.")
        {
            DateTime? result = null;

            do
            {
                var receivedValidInputs = false;

                Console.WriteLine(message);
                var inputs = Console.ReadLine();
                var inputArray = inputs.Split(' ');

                if (inputArray.Length != 3)
                {
                    Console.WriteLine(invalidInputMessage);
                    continue;
                }

                var receivedYear = int.TryParse(inputArray[0], out var yearInt);
                var receivedMonth = int.TryParse(inputArray[1], out var monthInt);
                var receivedDay = int.TryParse(inputArray[2], out var dayInt);

                receivedValidInputs = receivedYear && receivedMonth && receivedDay;

                if (!receivedValidInputs)
                {
                    Console.WriteLine(invalidInputMessage);
                    continue;
                }

                result = new DateTime(yearInt, monthInt, dayInt);

            } while (result == null);

            return (DateTime)result;
        }
    }
}

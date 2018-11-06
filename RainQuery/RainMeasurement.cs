using System;

namespace RainQuery
{
    class RainMeasurement
    {
        public DateTime date;
        public int Year => date.Year;
        public int Month => date.Month;
        public int Day => date.Day;
        public double measurement { get; private set; }

        public RainMeasurement(int year, int month, int day, double measurement)
        {
            date = new DateTime(year, month, day);
            this.measurement = measurement;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherArchive.Models
{
    public class WeatherCondition
    {
        
        [Key]
        public DateTime Date { get; set; }
        public float Temperature { get; set; }
        public float RelativeHumidity { get; set; }
        public float DewPoint { get; set; }
        public ushort AtmosphericPressure { get; set; }
        public string WindDirection { get; set; }
        public System.Nullable<ushort> WindSpeed { get; set; }
        public System.Nullable<ushort> CloudCover { get; set; }
        public System.Nullable<ushort> DownBorderCloudCover { get; set; }
        public System.Nullable<ushort> HorizontalView { get; set; }
        public string WeatherPhenomena { get; set; }
    }
}
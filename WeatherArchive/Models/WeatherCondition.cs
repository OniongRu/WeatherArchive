using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherArchive.Models
{
    public class WeatherCondition
    {
        
        [Key]
        public DateTime date { get; set; }
        public float temperature { get; set; }
        public float relativeHumidity { get; set; }
        public float dewPoint { get; set; }
        public ushort atmosphericPressure { get; set; }
        public string windDirection { get; set; }
        public System.Nullable<ushort> windSpeed { get; set; }
        public System.Nullable<ushort> cloudCover { get; set; }
        public System.Nullable<ushort> downBorderCloudCover { get; set; }
        public System.Nullable<ushort> horizontalView { get; set; }
        public string weatherPhenomena { get; set; }
    }
}
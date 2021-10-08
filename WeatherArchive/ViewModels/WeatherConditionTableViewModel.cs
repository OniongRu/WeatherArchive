using System.Collections.Generic;
using WeatherArchive.Models;

namespace WeatherArchive.ViewModels
{
    public class WeatherConditionTableViewModel
    {
        public ushort Year { get; set; }
        public ushort Month { get; set; }
        public IEnumerable<WeatherCondition> AllWeatherCondition { get; set; }
    }
}
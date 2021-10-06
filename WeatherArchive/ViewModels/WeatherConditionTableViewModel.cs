using System;
using System.Collections;
using System.Collections.Generic;
using WeatherArchive.Models;

namespace WeatherArchive.ViewModels
{
    public class WeatherConditionTableViewModel
    {
        public ushort year { get; set; }
        public ushort month { get; set; }
        public IEnumerable<WeatherCondition> allWeatherCondition { get; set; }
    }
}
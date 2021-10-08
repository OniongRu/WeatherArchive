using System.Collections.Generic;
using WeatherArchive.Models;

namespace WeatherArchive.interfaces
{
    public interface IAllWeatherConditions
    {
        void AddWeatherConditions(List<WeatherCondition> conditions);
        IEnumerable<WeatherCondition> WeatherConditions { get; }

    }
}
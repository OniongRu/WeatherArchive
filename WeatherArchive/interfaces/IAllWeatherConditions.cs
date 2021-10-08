using System.Collections.Generic;
using WeatherArchive.Models;

namespace WeatherArchive.interfaces
{
    public interface IAllWeatherConditions
    {
        void addWeatherConditions(List<WeatherCondition> conditions);
        IEnumerable<WeatherCondition> weatherConditions { get; }

    }
}
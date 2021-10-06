using System;
using System.Collections.Generic;
using System.Linq;
using WeatherArchive.DBInteraction;
using WeatherArchive.interfaces;
using WeatherArchive.Models;

namespace WeatherArchive.Repository
{
    public class WeatherConditionRepository : IAllWeatherConditions
    {
        private readonly AppDBContent appDbContent;

        public WeatherConditionRepository(AppDBContent appDbContent)
        {
            this.appDbContent = appDbContent;
        }

        public void addWeatherConditions(List<WeatherCondition> conditions)
        {
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    if (appDbContent.WeatherCondition.Any(o => o.date == condition.date))
                    {
                        appDbContent.WeatherCondition.Update(condition);
                    }
                    else
                    {
                        appDbContent.WeatherCondition.Add(condition);
                    }
                } 
                appDbContent.SaveChanges();
            }
            
        }

        public IEnumerable<WeatherCondition> weatherConditions
        {
            get => appDbContent.WeatherCondition; 
            set => throw new System.NotImplementedException();
        }
        public WeatherCondition getObjectWeatherCondition(int weatherCondition)
        {
            throw new System.NotImplementedException();
        }
    }
}
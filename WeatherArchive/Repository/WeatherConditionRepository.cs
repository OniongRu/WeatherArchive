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
            List<WeatherCondition> addConditions = new List<WeatherCondition>();
            List<WeatherCondition> updateConditions = new List<WeatherCondition>();
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    if (appDbContent.WeatherCondition.Any(o => o.date == condition.date))
                    {
                        updateConditions.Add(condition);
                        //appDbContent.WeatherCondition.Update(condition);
                    }
                    else
                    {
                        addConditions.Add(condition);
                        //appDbContent.WeatherCondition.Add(condition);
                    }
                } 
                if(addConditions.Any())
                    appDbContent.WeatherCondition.AddRange(addConditions);
                if(updateConditions.Any())
                    appDbContent.WeatherCondition.UpdateRange(updateConditions);
                
                appDbContent.SaveChanges();
            }
            
        }

        public IEnumerable<WeatherCondition> weatherConditions
        {
            get => appDbContent.WeatherCondition;
        }
    }
}
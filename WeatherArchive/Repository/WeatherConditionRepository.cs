using System.Collections.Generic;
using System.Linq;
using WeatherArchive.DBInteraction;
using WeatherArchive.interfaces;
using WeatherArchive.Models;

namespace WeatherArchive.Repository
{
    public class WeatherConditionRepository : IAllWeatherConditions
    {
        private readonly AppDBContent _appDbContent;

        public WeatherConditionRepository(AppDBContent appDbContent)
        {
            this._appDbContent = appDbContent;
        }

        public void AddWeatherConditions(List<WeatherCondition> conditions)
        {
            List<WeatherCondition> addConditions = new List<WeatherCondition>();
            List<WeatherCondition> updateConditions = new List<WeatherCondition>();
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    if (_appDbContent.WeatherCondition.Any(o => o.Date == condition.Date))
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
                    _appDbContent.WeatherCondition.AddRange(addConditions);
                if(updateConditions.Any())
                    _appDbContent.WeatherCondition.UpdateRange(updateConditions);
                
                _appDbContent.SaveChanges();
            }
            
        }

        public IEnumerable<WeatherCondition> WeatherConditions
        {
            get => _appDbContent.WeatherCondition;
        }
    }
}
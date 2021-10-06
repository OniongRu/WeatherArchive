using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherArchive.interfaces;
using WeatherArchive.Models;
using WeatherArchive.ViewModels;

namespace WeatherArchive.Controllers
{
    public class Month
    {
        public int num { get; set; }
        public string name { get; set; }
    }

    public class Year
    {
        public int yearNum { get; set; }
        public string yearStr { get; set; }
    }
        public class WeatherConditionsTableController : Controller
    {
        private IEnumerable<Month> months = new List<Month>
        {
            new Month { num = 0, name = "Месяц не выбран" },
            new Month { num = 1, name = "Январь" },
            new Month { num = 2, name = "Февраль" },
            new Month { num = 3, name = "Март" },
            new Month { num = 4, name = "Апрель" },
            new Month { num = 5, name = "Май" },
            new Month { num = 6, name = "Июнь" },
            new Month { num = 7, name = "Июль" },
            new Month { num = 8, name = "Август" },
            new Month { num = 9, name = "Сентябрь" },
            new Month { num = 10, name = "Октябрь" },
            new Month { num = 11, name = "Ноябрь" },
            new Month { num = 12, name = "Декабрь" }
        };

        private IEnumerable<Year> availableYears()
        {
            var avalible = _allWeatherConditions.weatherConditions.GroupBy(g => g.date.Year);
            List<Year> avalible_years = new List<Year>();
            avalible_years.Add(new Year
            {
                yearNum = 0,
                yearStr = "Год не выбран"
            });
            foreach (var year in avalible)
            {
                avalible_years.Add(new Year
                {
                    yearNum = year.Key,
                    yearStr = year.Key.ToString()
                });
            }
            return avalible_years.OrderBy(i=>i.yearNum);
        }

        private readonly IAllWeatherConditions _allWeatherConditions;
        
        public WeatherConditionsTableController(IAllWeatherConditions iallWeatherConditions)
        {
            _allWeatherConditions = iallWeatherConditions;
        }
        
        public IActionResult Checkout()
        {
            ViewBag.Years = new SelectList(availableYears(), "yearNum", "yearStr");
            ViewBag.Months = new SelectList(months, "num", "name");
            return View();
        }
        
        [HttpPost]
        public IActionResult Checkout(WeatherConditionTableViewModel weather)
        {
            ViewBag.Years = new SelectList(availableYears(), "yearNum", "yearStr");
            ViewBag.Months = new SelectList(months, "num", "name");
            WeatherConditionTableViewModel conditionObject = null;
            if (weather.year!=0)
            {
                if (weather.month != 0)
                {
                    conditionObject = new WeatherConditionTableViewModel()
                    {
                        year = weather.year,
                        month = weather.month,
                        allWeatherCondition = (from weatherCondition in _allWeatherConditions.weatherConditions 
                            where weatherCondition.date.Year==weather.year && weatherCondition.date.Month == weather.month 
                            select weatherCondition).OrderBy(i=>i.date).ToList()
                    };
                }
                else
                {
                    conditionObject = new WeatherConditionTableViewModel()
                    {
                        year = weather.year,
                        month = weather.month,
                        allWeatherCondition = (from weatherCondition in _allWeatherConditions.weatherConditions 
                            where weatherCondition.date.Year==weather.year 
                            select weatherCondition).OrderBy(i=>i.date).ToList()
                    };
                }
            }
            else
            {
                conditionObject = new WeatherConditionTableViewModel()
                {
                    year = weather.year,
                    month = weather.month,
                    allWeatherCondition = _allWeatherConditions.weatherConditions.OrderBy(i=>i.date)
                };
            }

            return View(conditionObject);
        }
        
    }
}
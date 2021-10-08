using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherArchive.interfaces;
using WeatherArchive.ViewModels;

namespace WeatherArchive.Controllers
{
    public class Month
    {
        public int Num { get; set; }
        public string Name { get; set; }
    }

    public class Year
    {
        public int YearNum { get; set; }
        public string YearStr { get; set; }
    }
        public class WeatherConditionsTableController : Controller
    {
        private readonly IEnumerable<Month> _months = new List<Month>
        {
            new Month { Num = 0, Name = "Месяц не выбран" },
            new Month { Num = 1, Name = "Январь" },
            new Month { Num = 2, Name = "Февраль" },
            new Month { Num = 3, Name = "Март" },
            new Month { Num = 4, Name = "Апрель" },
            new Month { Num = 5, Name = "Май" },
            new Month { Num = 6, Name = "Июнь" },
            new Month { Num = 7, Name = "Июль" },
            new Month { Num = 8, Name = "Август" },
            new Month { Num = 9, Name = "Сентябрь" },
            new Month { Num = 10, Name = "Октябрь" },
            new Month { Num = 11, Name = "Ноябрь" },
            new Month { Num = 12, Name = "Декабрь" }
        };

        private IEnumerable<Year> AvailableYears()
        {
            var avalible = _allWeatherConditions.WeatherConditions.GroupBy(g => g.Date.Year);
            List<Year> avalibleYears = new List<Year>();
            avalibleYears.Add(new Year
            {
                YearNum = 0,
                YearStr = "Год не выбран"
            });
            foreach (var year in avalible)
            {
                avalibleYears.Add(new Year
                {
                    YearNum = year.Key,
                    YearStr = year.Key.ToString()
                });
            }
            return avalibleYears.OrderBy(i=>i.YearNum);
        }

        private readonly IAllWeatherConditions _allWeatherConditions;
        
        public WeatherConditionsTableController(IAllWeatherConditions iallWeatherConditions)
        {
            _allWeatherConditions = iallWeatherConditions;
        }
        
        public IActionResult Checkout()
        {
            ViewBag.SelectYears = new SelectList(AvailableYears(), "YearNum", "YearStr");
            ViewBag.SelectMonths = new SelectList(_months, "Num", "Name");
            return View();
        }
        
        [HttpPost]
        public IActionResult Checkout(WeatherConditionTableViewModel weather)
        {
            ViewBag.SelectYears = new SelectList(AvailableYears(), "YearNum", "YearStr");
            ViewBag.SelectMonths = new SelectList(_months, "Num", "Name");
            WeatherConditionTableViewModel conditionObject = null;
            if (weather.Year!=0)
            {
                if (weather.Month != 0)
                {
                    conditionObject = new WeatherConditionTableViewModel()
                    {
                        Year = weather.Year,
                        Month = weather.Month,
                        AllWeatherCondition = (from weatherCondition in _allWeatherConditions.WeatherConditions 
                            where weatherCondition.Date.Year==weather.Year && weatherCondition.Date.Month == weather.Month 
                            select weatherCondition).OrderBy(i=>i.Date).ToList()
                    };
                }
                else
                {
                    conditionObject = new WeatherConditionTableViewModel()
                    {
                        Year = weather.Year,
                        Month = weather.Month,
                        AllWeatherCondition = (from weatherCondition in _allWeatherConditions.WeatherConditions 
                            where weatherCondition.Date.Year==weather.Year 
                            select weatherCondition).OrderBy(i=>i.Date).ToList()
                    };
                }
            }
            else
            {
                conditionObject = new WeatherConditionTableViewModel()
                {
                    Year = weather.Year,
                    Month = weather.Month,
                    AllWeatherCondition = _allWeatherConditions.WeatherConditions.OrderBy(i=>i.Date)
                };
            }

            return View(conditionObject);
        }
        
    }
}
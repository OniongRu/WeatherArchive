using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherArchive.interfaces;
using WeatherArchive.ViewModels;

namespace WeatherArchive.Controllers
{
    public class DropDownList
    {
        public int Num { get; set; }
        public string Name { get; set; }
    }

    public class WeatherConditionsTableController : Controller
    {
        private readonly IEnumerable<DropDownList> _months = new List<DropDownList>
        {
            new DropDownList { Num = 0, Name = "Месяц не выбран" },
            new DropDownList { Num = 1, Name = "Январь" },
            new DropDownList { Num = 2, Name = "Февраль" },
            new DropDownList { Num = 3, Name = "Март" },
            new DropDownList { Num = 4, Name = "Апрель" },
            new DropDownList { Num = 5, Name = "Май" },
            new DropDownList { Num = 6, Name = "Июнь" },
            new DropDownList { Num = 7, Name = "Июль" },
            new DropDownList { Num = 8, Name = "Август" },
            new DropDownList { Num = 9, Name = "Сентябрь" },
            new DropDownList { Num = 10, Name = "Октябрь" },
            new DropDownList { Num = 11, Name = "Ноябрь" },
            new DropDownList { Num = 12, Name = "Декабрь" }
        };
        private readonly IEnumerable<DropDownList> _tableType = new List<DropDownList>
        {
            new DropDownList { Num = 0, Name = "Таблица из cdn.datatables.net" },
            new DropDownList { Num = 1, Name = "Минималистичная таблица" }
        };
        

        private IEnumerable<DropDownList> AvailableYears()
        {
            var avalible = _allWeatherConditions.WeatherConditions.GroupBy(g => g.Date.Year);
            List<DropDownList> avalibleYears = new List<DropDownList>();
            avalibleYears.Add(new DropDownList
            {
                Num = 0,
                Name = "Год не выбран"
            });
            foreach (var year in avalible)
            {
                avalibleYears.Add(new DropDownList
                {
                    Num = year.Key,
                    Name = year.Key.ToString()
                });
            }
            return avalibleYears.OrderBy(i=>i.Num);
        }

        private readonly IAllWeatherConditions _allWeatherConditions;
        
        public WeatherConditionsTableController(IAllWeatherConditions iallWeatherConditions)
        {
            _allWeatherConditions = iallWeatherConditions;
        }
        
        public IActionResult Checkout()
        {
            ViewBag.SelectYears = new SelectList(AvailableYears(), "Num", "Name");
            ViewBag.SelectMonths = new SelectList(_months, "Num", "Name");
            ViewBag.SelectTableType = new SelectList(_tableType, "Num", "Name");
            return View();
        }
        
        [HttpPost]
        public IActionResult Checkout(WeatherConditionTableViewModel weather)
        {
            ViewBag.SelectYears = new SelectList(AvailableYears(), "Num", "Name");
            ViewBag.SelectMonths = new SelectList(_months, "Num", "Name");
            ViewBag.SelectTableType = new SelectList(_tableType, "Num", "Name");
            WeatherConditionTableViewModel conditionObject = null;
            if (weather.Year!=0)
            {
                if (weather.Month != 0)
                {
                    conditionObject = new WeatherConditionTableViewModel()
                    {
                        TableType = weather.TableType,
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
                        TableType = weather.TableType,
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
                    TableType = weather.TableType,
                    Year = weather.Year,
                    Month = weather.Month,
                    AllWeatherCondition = _allWeatherConditions.WeatherConditions.OrderBy(i=>i.Date)
                };
            }

            return View(conditionObject);
        }
        
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherArchive.interfaces;
using WeatherArchive.Models;
using WeatherArchive.Parsers;

namespace WeatherArchive.Controllers
{
    public class LoadArchiveController : Controller
    {
        
        private readonly ILogger<LoadArchiveController> _logger;
        
        private readonly IAllWeatherConditions _allWeatherConditions;

        public LoadArchiveController(IAllWeatherConditions iallWeatherConditions, ILogger<LoadArchiveController> logger)
        {
            _allWeatherConditions = iallWeatherConditions;
            _logger = logger;
        }

        public IActionResult LoadInfo()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            
            foreach(var uploadedFile in uploads)
            {
                // формат обрабатываемого файла
                string fileFormat = Path.GetExtension(uploadedFile.FileName);
                
                using (var fileStream = uploadedFile.OpenReadStream())
                {
                    List<WeatherCondition> report = ExcelParser.OpenExcel(fileStream,fileFormat);
                    if (report == null) continue;
                    if (report.Any())
                    {
                        _allWeatherConditions.AddWeatherConditions(report);
                        _logger.LogInformation("add new "+report.Count+"rows");
                    }
                    else
                    {
                        _logger.LogInformation("Excel file does not contain any matching lines!");
                    }
                }
                
            }
            return RedirectToAction("LoadInfo");
        }
    }
}
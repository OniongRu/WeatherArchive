using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherArchive.interfaces;
using WeatherArchive.Models;
using WeatherArchive.Parsers;
using WeatherArchive.ViewModels;

namespace WeatherArchive.Controllers
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    
    public class LoadArchiveController : Controller
    {

        
        private IWebHostEnvironment _appEnvironment;
        
        private readonly IAllWeatherConditions _allWeatherConditions;

        private int numberAddRows;
            
        public LoadArchiveController(IAllWeatherConditions iallWeatherConditions, IWebHostEnvironment appEnvironment)
        {
            _allWeatherConditions = iallWeatherConditions;
            _appEnvironment = appEnvironment;
        }

        public IActionResult LoadInfo()
        {
           
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult>  AddFile(IFormFileCollection uploads)
        {
            
            foreach(var uploadedFile in uploads)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                    FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                }
                
                List<WeatherCondition> report = ExcelParser.OpenExcel(_appEnvironment.WebRootPath +path);
                if (report == null) continue;
                if (report.Any())
                {
                    _allWeatherConditions.addWeatherConditions(report);
                    Console.WriteLine("add new "+report.Count+"rows");
                }
            }
            return RedirectToAction("LoadInfo");
        }
    }
}
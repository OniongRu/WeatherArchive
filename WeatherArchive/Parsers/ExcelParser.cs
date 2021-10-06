using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherArchive.Models;

namespace WeatherArchive.Parsers
{
    
    
    public class ExcelParser
    {

        public static WeatherCondition getWeatherConditionFromRow(IRow row)
        {
            WeatherCondition rowCondition = new WeatherCondition();
            if (row != null)
            {
                if (row.LastCellNum >= 11)
                {
                    ICell cell;
                    
                    //Date + Time
                    cell = row.GetCell(0);
                    if (cell != null && row.GetCell(1)!=null)
                    {
                        var s = row.GetCell(0).ToString()+" "+row.GetCell(1).ToString();
                        DateTime dt = new DateTime();
                        var rusCulture = CultureInfo.GetCultureInfo("ru-RU");
                        if (DateTime.TryParse(s, rusCulture, DateTimeStyles.None, out dt))
                        {
                            rowCondition.date = dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    
                    
                    //T
                    cell = row.GetCell(2);
                    if (cell != null)
                    {
                        float num=0;
                        if (float.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.temperature = num;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    
                    //влажность
                    cell = row.GetCell(3);
                    if (cell != null)
                    {
                        float num=0;
                        if (float.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.relativeHumidity = num;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    
                    //Td
                    cell = row.GetCell(4);
                    if (cell != null)
                    {
                        float num=0;
                        if (float.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.dewPoint = num;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    
                    //Давление
                    cell = row.GetCell(5);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.atmosphericPressure = num;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    
                    //Ветере направ.
                    cell = row.GetCell(6);
                    if (cell != null)
                    {
                        rowCondition.windDirection = cell.ToString();
                    }
                    else
                    {
                        rowCondition.windDirection = null;
                    }
                    
                    //скорость ветра
                    cell = row.GetCell(7);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.windSpeed = num;
                        }
                        else
                        {
                            rowCondition.windSpeed = null;
                        }
                    }
                    else
                    {
                        rowCondition.windSpeed = null;
                    }
                    
                    //Облачность
                    cell = row.GetCell(8);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.cloudCover = num;
                        }
                        else
                        {
                            rowCondition.cloudCover = null;
                        }
                    }
                    else
                    {
                        rowCondition.cloudCover = null;
                    }
                    
                    //h
                    cell = row.GetCell(9);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.downBorderCloudCover = num;
                        }
                        else
                        {
                            rowCondition.downBorderCloudCover = null;
                        }
                    }
                    else
                    {
                        rowCondition.downBorderCloudCover = null;
                    }
                    
                    //vv
                    cell = row.GetCell(10);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.horizontalView = num;
                        }
                        else
                        {
                            rowCondition.horizontalView = null;
                        }
                    }
                    else
                    {
                        rowCondition.horizontalView = null;
                    }
                    
                    //погодное явление
                    cell = row.GetCell(11);
                    if (cell != null)
                    {
                        rowCondition.weatherPhenomena = cell.ToString();
                    }
                    else
                    {
                        rowCondition.weatherPhenomena = null;
                    }

                    return rowCondition;
                }
                
            }

            return null;
        }
        
        public static List<WeatherCondition> OpenExcel(string path)
        {
            List<WeatherCondition> conditionsFromExcel = new List<WeatherCondition>();
            FileStream fs;
            IWorkbook wk = null;
            try
            {
                using (fs = File.OpenRead (path)) // Открываем файл
                {
                    if(Path.GetExtension(fs.Name).Equals(".xlsx"))
                    {
                        wk = new XSSFWorkbook (fs); // Записать xls -> wk 
                    }
                    else if (Path.GetExtension(fs.Name).Equals(".xls"))
                    {
                        wk = new HSSFWorkbook (fs);
                    }
                    else
                    {
                        return null;
                    }
                        
                       
                    for (int i = 0; i <wk.NumberOfSheets; i ++) // NumberOfSheets - общее количество таблиц
                    {
                        ISheet sheet = wk.GetSheetAt (i); // Считать данные текущего листа
                        if (sheet.LastRowNum > 3)
                        {
                            for (int j = 4; j <= sheet.LastRowNum; j++) // LastRowNum - общее количество строк в текущей таблице
                            {
                                IRow row = sheet.GetRow(j); // Считать данные текущей строки
                                WeatherCondition condition = getWeatherConditionFromRow(row);
                                
                                if (condition != null)
                                {
                                    conditionsFromExcel.Add(new WeatherCondition{
                                        date = condition.date,
                                        temperature = condition.temperature,
                                        relativeHumidity = condition.relativeHumidity,
                                        dewPoint = condition.dewPoint,
                                        atmosphericPressure = condition.atmosphericPressure,
                                        windDirection = condition.windDirection,
                                        windSpeed = condition.windSpeed,
                                        cloudCover = condition.cloudCover,
                                        downBorderCloudCover = condition.downBorderCloudCover,
                                        horizontalView = condition.horizontalView,
                                        weatherPhenomena = condition.weatherPhenomena
                                    });

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Exception!!!");
            }
            return conditionsFromExcel;
         }
        
    }
}
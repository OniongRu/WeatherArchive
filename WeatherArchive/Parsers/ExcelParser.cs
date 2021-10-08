using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherArchive.Models;

namespace WeatherArchive.Parsers
{
    
    
    public class ExcelParser
    {
        private static ILogger logger = LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<ExcelParser>();
        public static WeatherCondition GetWeatherConditionFromRow(IRow row)
        {
            WeatherCondition rowCondition = new WeatherCondition();
            if (row != null)
            {
                if (row.LastCellNum >= 11)
                {
                    ICell cell;
                    
                    //Date + Time
                    cell = row.GetCell((int) WeatherConditionsFields.Date);
                    if (cell != null && row.GetCell((int) WeatherConditionsFields.Time)!=null)
                    {
                        var s = cell.ToString()+" "+row.GetCell((int) WeatherConditionsFields.Time).ToString();
                        DateTime dt = new DateTime();
                        var rusCulture = CultureInfo.GetCultureInfo("ru-RU");
                        if (DateTime.TryParse(s, rusCulture, DateTimeStyles.None, out dt))
                        {
                            rowCondition.Date = dt;
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
                    cell = row.GetCell((int) WeatherConditionsFields.T);
                    if (cell != null)
                    {
                        float num=0;
                        if (float.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.Temperature = num;
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
                    cell = row.GetCell((int) WeatherConditionsFields.Humidity);
                    if (cell != null)
                    {
                        float num=0;
                        if (float.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.RelativeHumidity = num;
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
                    cell = row.GetCell((int) WeatherConditionsFields.Dp);
                    if (cell != null)
                    {
                        float num=0;
                        if (float.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.DewPoint = num;
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
                    cell = row.GetCell((int) WeatherConditionsFields.AtmPress);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.AtmosphericPressure = num;
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
                    cell = row.GetCell((int) WeatherConditionsFields.WindDir);
                    if (cell != null)
                    {
                        rowCondition.WindDirection = cell.ToString();
                    }
                    else
                    {
                        rowCondition.WindDirection = null;
                    }
                    
                    //скорость ветра
                    cell = row.GetCell((int) WeatherConditionsFields.WindS);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.WindSpeed = num;
                        }
                        else
                        {
                            rowCondition.WindSpeed = null;
                        }
                    }
                    else
                    {
                        rowCondition.WindSpeed = null;
                    }
                    
                    //Облачность
                    cell = row.GetCell((int) WeatherConditionsFields.Cloud);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.CloudCover = num;
                        }
                        else
                        {
                            rowCondition.CloudCover = null;
                        }
                    }
                    else
                    {
                        rowCondition.CloudCover = null;
                    }
                    
                    //h
                    cell = row.GetCell((int) WeatherConditionsFields.DownBorder);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.DownBorderCloudCover = num;
                        }
                        else
                        {
                            rowCondition.DownBorderCloudCover = null;
                        }
                    }
                    else
                    {
                        rowCondition.DownBorderCloudCover = null;
                    }
                    
                    //vv
                    cell = row.GetCell((int) WeatherConditionsFields.Horizont);
                    if (cell != null)
                    {
                        ushort num=0;
                        if (ushort.TryParse(cell.ToString(), out num))
                        {
                            rowCondition.HorizontalView = num;
                        }
                        else
                        {
                            rowCondition.HorizontalView = null;
                        }
                    }
                    else
                    {
                        rowCondition.HorizontalView = null;
                    }
                    
                    //погодное явление
                    cell = row.GetCell((int) WeatherConditionsFields.Phenomena);
                    if (cell != null)
                    {
                        rowCondition.WeatherPhenomena = cell.ToString();
                    }
                    else
                    {
                        rowCondition.WeatherPhenomena = null;
                    }

                    return rowCondition;
                }
                
            }

            return null;
        }
        
        public static List<WeatherCondition> OpenExcel(Stream file, string fileFormat)
        {
            List<WeatherCondition> conditionsFromExcel = new List<WeatherCondition>();
            IWorkbook wk = null;
            try
            {
                using (file) // Открываем файл
                {
                    
                    if(fileFormat.Equals(".xlsx"))
                   {
                       wk = new XSSFWorkbook (file); // Записать xls -> wk 
                   }
                   else if (fileFormat.Equals(".xls"))
                   {
                       wk = new HSSFWorkbook (file);
                   }
                   else
                   {
                       logger.LogInformation("Wrong file format");
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
                                WeatherCondition condition = GetWeatherConditionFromRow(row);
                                
                                if (condition != null)
                                {
                                    conditionsFromExcel.Add(new WeatherCondition{
                                        Date = condition.Date,
                                        Temperature = condition.Temperature,
                                        RelativeHumidity = condition.RelativeHumidity,
                                        DewPoint = condition.DewPoint,
                                        AtmosphericPressure = condition.AtmosphericPressure,
                                        WindDirection = condition.WindDirection,
                                        WindSpeed = condition.WindSpeed,
                                        CloudCover = condition.CloudCover,
                                        DownBorderCloudCover = condition.DownBorderCloudCover,
                                        HorizontalView = condition.HorizontalView,
                                        WeatherPhenomena = condition.WeatherPhenomena
                                    });

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                logger.LogInformation("Exception in parsing file");
            }
            logger.LogInformation("File parse successfully");
            return conditionsFromExcel;
         }
        
    }
}
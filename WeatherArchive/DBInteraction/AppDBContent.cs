using Microsoft.EntityFrameworkCore;
using WeatherArchive.Models;

namespace WeatherArchive.DBInteraction
{
    public class AppDBContent : DbContext
    {
        public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)
        {
            
        }
        
        public DbSet<WeatherCondition> WeatherCondition { get; set; }
    }
}
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL.oClasses
{
    public class ConnectionStringManager : IConnectionStringManager
    {

        //---This function is for picking up connectioin string from appsetting.json file in project
        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetSection("ConnectionStrings").GetSection("DBConnect").Value;
        }
    }
}

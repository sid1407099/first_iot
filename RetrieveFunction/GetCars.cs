using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RetrieveFunction
{

    public static class GetCars
    {

        [FunctionName("GetCars")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<CarProperties> lst = new List<CarProperties>();
            string _connection_string = "Server=tcp:carapiproject.database.windows.net,1433;Initial Catalog=Car;Persist Security Info=False;User ID=admin121;Password=sidjain@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string _statement = "select make,model,year,colour from Car";
            SqlConnection _connection = new SqlConnection(_connection_string);

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    CarProperties _cars = new CarProperties()
                    {
                        make = _reader.GetString(0),
                        model = _reader.GetString(1),
                        year = _reader.GetInt32(2),
                        colour = _reader.GetString(3),
                    };
                    lst.Add(_cars);
                }
            }
            _connection.Close();
            return new OkObjectResult(lst);

            

        }
    }
}

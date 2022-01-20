using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace RetrieveFunction
{
    public static class AddCar
    {
        [FunctionName("AddCar")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CarProperties data = JsonConvert.DeserializeObject<CarProperties>(requestBody);

            string _connection_string = "Server=tcp:carapiproject.database.windows.net,1433;Initial Catalog=Car;Persist Security Info=False;User ID=admin121;Password=sidjain@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string _statement = "INSERT INTO Car(make,model,year,colour) VALUES(@make,@model,@year,@colour)";
            SqlConnection _connection = new SqlConnection(_connection_string);

            _connection.Open();

            using (SqlCommand _command = new SqlCommand(_statement, _connection))
            {
                _command.Parameters.Add("@make", SqlDbType.VarChar).Value = data.make;
                _command.Parameters.Add("@model", SqlDbType.VarChar).Value = data.model;
                _command.Parameters.Add("@year", SqlDbType.Int).Value = data.year;
                _command.Parameters.Add("@colour", SqlDbType.VarChar).Value = data.colour;

                _command.CommandType = CommandType.Text;
                _command.ExecuteNonQuery();
            }

            return new OkObjectResult("Car added");
        }
    }
}

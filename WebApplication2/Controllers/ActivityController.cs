using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ActivityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //API Methods

        [HttpGet]
        public JsonResult Get()
        {
            //RAW QUERIES:ORM kullanmadan veritabanı işlemi yapmama yarar.
            string query = @"
               select ActivityId,ActivityName,ActivityDescription,ActivityLocation,ActivityAge from dbo.Activities
             ";
            //Datatable Object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Activity act)
        {

            string query = @"
               insert into dbo.Activities
               values (@ActivityName,@ActivityDescription,@ActivityLocation,@ActivityAge)
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {                    
                    myCommand.Parameters.AddWithValue("ActivityName", act.ActivityName);
                    myCommand.Parameters.AddWithValue("ActivityDescription", act.ActivityDescription);
                    myCommand.Parameters.AddWithValue("ActivityLocation", act.ActivityLocation);
                    myCommand.Parameters.AddWithValue("ActivityAge", act.ActivityAge);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Succesfully");
        }
        [HttpPut]
        public JsonResult Put(Activity act)
        {

            string query = @"
               update dbo.Activity
              set ActivityName=@ActivityName,
              ActivityDescription=@ActivityDescription,
              ActivityLocation=@ActivityLocation,
              ActivityAge=@ActivityAge
               where ActivityId=@ActivityId
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ActivityId", act.ActivityId);
                    myCommand.Parameters.AddWithValue("@ActivityName", act.ActivityName);
                    myCommand.Parameters.AddWithValue("ActivityDescription",act.ActivityDescription);
                    myCommand.Parameters.AddWithValue("@ActivityLocation", act.ActivityLocation);
                    myCommand.Parameters.AddWithValue("@ActivityAge", act.ActivityAge);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update Succesfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {

            string query = @"
               delete from dbo.Activities
               where ActivityId=@ActivityId
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ActivityId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Succesfully");
        }
    }
}

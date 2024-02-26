using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //API Methods

        [HttpGet]
        public JsonResult Get()
        {
            //RAW QUERIES:ORM kullanmadan veritabanı işlemi yapmama yarar.
            string query = @"
               select CategoryId,CategoryName from dbo.Categories
             ";
            //Datatable Object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand=new SqlCommand(query, myCon))
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
        public JsonResult Post(Category cat)
        {
     
            string query = @"
               insert into dbo.Categories
               values (@CategoryName)
             ";
            
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("CategoryName", cat.CategoryName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Succesfully");
        }
        [HttpPut]
        public JsonResult Put(Category cat)
        {
            
            string query = @"
               update dbo.Categories
               set CategoryName = @CategoryName 
               where CategoryId=@CategoryId
             ";
            
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CategoryId", cat.CategoryId);
                    myCommand.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
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
               delete from dbo.Categories
               where CategoryId=@CategoryId
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CategoryId", id);
                   
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

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
    public class BrandsController : ControllerBase
    {
        
            private readonly IConfiguration _configuration;
            public BrandsController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            //API Methods

            [HttpGet]
            public JsonResult Get()
            {
                
                string query = @"
               select BrandId,BrandName, CategoryId from dbo.Brands
             ";
                
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
            public JsonResult Post(Brands brands)
            {

                string query = @"
               insert into dbo.Brands
               values (@BrandName, @CategoryId)
             ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("dbcon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("BrandName", brands.BrandName);
                        myCommand.Parameters.AddWithValue("CategoryId", brands.CategoryId);

                    myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("Added Succesfully");
            }
            [HttpPut]
            public JsonResult Put(Brands brand)
            {

                string query = @"
               update dbo.Brands
               set BrandName = @BrandName, CategoryId=@CategoryId
               where BrandId=@BrandId";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("dbcon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@BrandId", brand.BrandId);
                        myCommand.Parameters.AddWithValue("@BrandName", brand.BrandName);
                        myCommand.Parameters.AddWithValue("@CategoryId", brand.CategoryId);
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
               delete from dbo.Brands
               where BrandId=@BrandId
             ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("dbcon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@BrandId", id);

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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication2.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        //injection to get the application path to photos
        private readonly IWebHostEnvironment _env;
        public ProductsController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        //API Methods

        [HttpGet]
        public JsonResult Get()
        {
            //RAW QUERIES:ORM kullanmadan veritabanı işlemi yapmama yarar.
            string query = @"
               select ProductId,ProductName,DateOfJoining,PhotoFileName,ProductDescription,Price,CategoryId,BrandId from dbo.Products
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
        public JsonResult Post(Products pro)
        {

            string query = @"
               insert into dbo.Products
               values (@ProductName,@DateOfJoining,@PhotoFileName,@ProductDescription,@Price,@CategoryId,@BrandId)
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("ProductName", pro.ProductName);
                    myCommand.Parameters.AddWithValue("DateOfJoining", pro.DateOfJoining);
                    myCommand.Parameters.AddWithValue("PhotoFileName", pro.PhotoFileName);
                    myCommand.Parameters.AddWithValue("ProductDescription", pro.ProductDescription);
                    myCommand.Parameters.AddWithValue("Price", pro.Price);
                    myCommand.Parameters.AddWithValue("CategoryId", pro.CategoryId);
                    myCommand.Parameters.AddWithValue("BrandId", pro.BrandId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Succesfully");
        }

        [HttpPut]
        public JsonResult Put(Products pro)
        {

            string query = @"
               update dbo.Products
                set ProductName = @ProductName,
                DateOfJoining = @DateOfJoining,
                PhotoFileName = @PhotoFileName,
                ProductDescription = @ProductDescription,
                Price = @Price,
                CategoryId = @CategoryId,
                BrandId = @BrandId
                where ProductId=@ProductId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ProductId", pro.ProductId);
                    myCommand.Parameters.AddWithValue("@ProductName", pro.ProductName);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", pro.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", pro.PhotoFileName);
                    myCommand.Parameters.AddWithValue("@ProductDescription", pro.ProductDescription);
                    myCommand.Parameters.AddWithValue("@Price", pro.Price);
                    myCommand.Parameters.AddWithValue("@CategoryId", pro.CategoryId);
                    myCommand.Parameters.AddWithValue("@BrandId", pro.BrandId);
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
               delete from dbo.Products
               where ProductsId=@ProductsId
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ProductsId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Succesfully");
        }

        //Giving custom root name for add photos method
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile() 
        {
            try
            {
                //extract the first file which is attached to form data
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var pyhsicalPath = _env.ContentRootPath + "/Photos" + filename;

                using (var stream=new FileStream(pyhsicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);


            }
            catch (Exception)
            {
                return new JsonResult("anonymous.jpg");
            }
        }

    }
}

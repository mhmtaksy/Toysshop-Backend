using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Data.SqlClient;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _sqlConnection;
        SqlCommand cmd = null;
        SqlDataAdapter da = null;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("dbcon");
            _sqlConnection = new SqlConnection(connectionString);
        }

        //API Methods

        [HttpGet]
        public JsonResult Get()
        {
            //RAW QUERIES:ORM kullanmadan veritabanı işlemi yapmama yarar.
            string query = @"
               select Email,PasswordHash from dbo.Admins
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
        [Route("Login")]
        public string Login(Admin admin)
        {
            string msg = string.Empty;
            try
            {
                _sqlConnection.Open();
                string query = "SELECT COUNT(*) FROM Admins WHERE Email = @Email AND PasswordHash = @PasswordHash";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@Email", admin.Email);
                    command.Parameters.AddWithValue("@PasswordHash", admin.PasswordHash);

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        msg = "Admin is valid";
                    }
                    else
                    {
                        msg = "Admin is Invalid";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                _sqlConnection.Close();
            }
            return msg;
        }


        public class Admin
        {
            public string Email { get; set; }
            public string PasswordHash { get; set; }
        }
    }
}
    


using FinTrack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataAdapter _adapter;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("FinTrackRepo"));
        }

        [HttpPost]
        [Route("Registration")]
        public string registration(User user)
        {
            string message = string.Empty;
            try
            {
                _command = new SqlCommand("usp_signup", _connection);
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@name", user.FirstName);
                _command.Parameters.AddWithValue("@phNumber", user.PhoneNumber);
                _command.Parameters.AddWithValue("@address", user.Address);
                _command.Parameters.AddWithValue("@isActive", user.IsActive);
                _command.Parameters.AddWithValue("@password", user.Password);

                _connection.Open();
                int i = _command.ExecuteNonQuery();
                _connection.Close();

                if (i > 0)
                {
                    message = "Data inserted";
                }
                else
                {
                    message = "Failure";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        [HttpPost]
        [Route("Login")]
        public string login([FromBody] LoginRequest req)
        {
            string message = string.Empty;
            try
            {
                _adapter = new SqlDataAdapter("usp_login",_connection);
                _adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                _adapter.SelectCommand.Parameters.AddWithValue("@name", req.Name);
                _adapter.SelectCommand.Parameters.AddWithValue("@password", req.Password);

                DataTable dt = new DataTable();
                _adapter.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    message = "User is valid";
                }
                else
                {
                    message = "User is invalid";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
    }
}

using ASPNETCoreWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace ASPNETCoreWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public string GetEmployees()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EmployeeAppcon").ToString());
            SqlDataAdapter da = new SqlDataAdapter("select * from employee", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Employee> employeelist = new List<Employee>();
            Response response = new Response();
            if(dt.Rows.Count>0)
            {
                for(int i = 0; i< dt.Rows.Count;i++)
                {
                    Employee employee = new Employee();
                    employee.id = Convert.ToInt32( dt.Rows[i]["EmpId"]);
                    employee.Empname = Convert.ToString(dt.Rows[i]["EmpName"]);
                    employee.Password = Convert.ToString(dt.Rows[i]["Password"]);
                    employeelist.Add(employee);
                }

            }
            if(employeelist.Count>0)
            {
                return JsonConvert.SerializeObject(employeelist);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}

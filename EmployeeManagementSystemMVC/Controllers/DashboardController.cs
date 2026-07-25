using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystemMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string ConnectionString =>
            _configuration.GetConnectionString("DefaultConnection");

        public IActionResult Index()
        {
            // Check Session
            var admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Admin = admin;

            int totalEmployees = 0;
            int totalReports = 0;

            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                con.Open();

                // Total Employees
                string empQuery = "SELECT COUNT(*) FROM Employee";

                using (MySqlCommand cmd = new MySqlCommand(empQuery, con))
                {
                    totalEmployees = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Total Reports
                // Change this query if you have a Reports table
                string reportQuery = "SELECT COUNT(*) FROM Employee";

                using (MySqlCommand cmd = new MySqlCommand(reportQuery, con))
                {
                    totalReports = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalReports = totalReports;
            ViewBag.TodayLogin = 1;

            return View();
        }
    }
}
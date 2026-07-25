using EmployeeManagementSystemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystemMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;


        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private string ConnectionString =>
            _configuration.GetConnectionString("DefaultConnection")!;




        // Login Page

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }





        // Login Check

        [HttpPost]
        public IActionResult Index(Login login)
        {

            try
            {

                using MySqlConnection con =
                    new MySqlConnection(ConnectionString);


                con.Open();



                string query =
                    @"SELECT COUNT(*) 
                      FROM Admin
                      WHERE Username=@Username
                      AND Password=@Password";



                using MySqlCommand cmd =
                    new MySqlCommand(query, con);



                cmd.Parameters.AddWithValue(
                    "@Username",
                    login.Username
                );


                cmd.Parameters.AddWithValue(
                    "@Password",
                    login.Password
                );



                int result =
                    Convert.ToInt32(cmd.ExecuteScalar());



                if (result > 0)
                {

                    // Create Session

                    HttpContext.Session.SetString(
                        "Admin",
                        login.Username
                    );



                    return RedirectToAction(
                        "Index",
                        "Dashboard"
                    );
                }



                ViewBag.Error =
                    "Invalid Username or Password";


                return View(login);


            }
            catch (Exception ex)
            {

                ViewBag.Error =
                    ex.Message;


                return View(login);

            }

        }





        // Logout

        public IActionResult Logout()
        {

            // Remove Session

            HttpContext.Session.Clear();


            return RedirectToAction(
                "Index",
                "Login"
            );

        }


    }
}
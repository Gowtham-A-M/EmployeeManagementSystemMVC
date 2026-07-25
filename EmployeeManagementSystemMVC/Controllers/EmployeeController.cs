using EmployeeManagementSystemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystemMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;


        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private string ConnectionString =>
            _configuration.GetConnectionString("DefaultConnection")!;



        // Employee List

        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();


            using MySqlConnection con =
                new MySqlConnection(ConnectionString);

            con.Open();


            string query =
                "SELECT * FROM Employee";


            using MySqlCommand cmd =
                new MySqlCommand(query, con);


            using MySqlDataReader reader =
                cmd.ExecuteReader();



            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Department = reader["Department"].ToString()!,
                    Salary = Convert.ToDecimal(reader["Salary"])
                });
            }


            return View(employees);

        }




        // CREATE GET

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }




        // CREATE POST

        [HttpPost]
        public IActionResult Create(Employee employee)
        {

            using MySqlConnection con =
                new MySqlConnection(ConnectionString);


            con.Open();



            string check =
                "SELECT COUNT(*) FROM Employee WHERE Email=@Email";


            using MySqlCommand checkCmd =
                new MySqlCommand(check, con);


            checkCmd.Parameters.AddWithValue(
                "@Email",
                employee.Email);



            int count =
                Convert.ToInt32(checkCmd.ExecuteScalar());



            if (count > 0)
            {
                ModelState.AddModelError(
                    "Email",
                    "Email already exists"
                );

                return View(employee);
            }




            string query =
                @"INSERT INTO Employee
                (Name,Email,Department,Salary)
                VALUES
                (@Name,@Email,@Department,@Salary)";


            using MySqlCommand cmd =
                new MySqlCommand(query, con);


            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);



            cmd.ExecuteNonQuery();


            return RedirectToAction("Index");

        }




        // EDIT GET

        [HttpGet]
        public IActionResult Edit(int id)
        {

            Employee employee = null!;


            using MySqlConnection con =
                new MySqlConnection(ConnectionString);


            con.Open();



            string query =
                "SELECT * FROM Employee WHERE Id=@Id";


            using MySqlCommand cmd =
                new MySqlCommand(query, con);


            cmd.Parameters.AddWithValue("@Id", id);



            using MySqlDataReader reader =
                cmd.ExecuteReader();



            if (reader.Read())
            {
                employee = new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Department = reader["Department"].ToString()!,
                    Salary = Convert.ToDecimal(reader["Salary"])
                };
            }


            return View(employee);

        }




        // EDIT POST UPDATE

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {


            using MySqlConnection con =
                new MySqlConnection(ConnectionString);


            con.Open();



            string query =
                @"UPDATE Employee SET
                Name=@Name,
                Email=@Email,
                Department=@Department,
                Salary=@Salary
                WHERE Id=@Id";



            using MySqlCommand cmd =
                new MySqlCommand(query, con);



            cmd.Parameters.AddWithValue("@Id", employee.Id);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);



            cmd.ExecuteNonQuery();



            return RedirectToAction("Index");

        }





        // DELETE

        [HttpGet]
        public IActionResult Delete(int id)
        {


            using MySqlConnection con =
                new MySqlConnection(ConnectionString);


            con.Open();



            string query =
                "DELETE FROM Employee WHERE Id=@Id";


            using MySqlCommand cmd =
                new MySqlCommand(query, con);



            cmd.Parameters.AddWithValue("@Id", id);



            cmd.ExecuteNonQuery();



            return RedirectToAction("Index");

        }


    }
}
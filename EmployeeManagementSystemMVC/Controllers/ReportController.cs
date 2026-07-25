using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using EmployeeManagementSystemMVC.Models;

namespace EmployeeManagementSystemMVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;

        public ReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private string ConnectionString =>
            _configuration.GetConnectionString("DefaultConnection")!;



        // Employee Report Page

        public IActionResult EmployeeReport()
        {
            // Session Check

            var admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }


            ViewBag.Admin = admin;


            List<Employee> employees = GetEmployees();


            return View(employees);
        }




        // Export Employee Report Excel

        public IActionResult ExportExcel()
        {

            // Session Check

            var admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }



            using var workbook = new XLWorkbook();



            var worksheet =
                workbook.Worksheets.Add("Employees");



            // Header

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Department";
            worksheet.Cell(1, 5).Value = "Salary";



            List<Employee> employees =
                GetEmployees();



            int row = 2;



            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.Id;
                worksheet.Cell(row, 2).Value = emp.Name;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.Department;
                worksheet.Cell(row, 5).Value = emp.Salary;


                row++;
            }



            worksheet.Columns()
                     .AdjustToContents();



            using var stream =
                new MemoryStream();



            workbook.SaveAs(stream);



            stream.Position = 0;



            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EmployeeReport.xlsx"
            );

        }





        // Get Employees From Database

        private List<Employee> GetEmployees()
        {

            List<Employee> employees =
                new List<Employee>();



            using MySqlConnection con =
                new MySqlConnection(ConnectionString);



            con.Open();



            string query =
                "SELECT Id, Name, Email, Department, Salary FROM Employee";



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



            return employees;

        }

    }
}
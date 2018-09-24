using LabApplicationBackendLayer.Models;
using LabApplicationBackendLayer.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LabApplicationBackendLayer.DAL
{
    internal class EmployeeSQLDAL : ILabBackendDAL
    {
        string connectionString;

        string[] empNames =
                {
                    "Lenore Futch","Janice Sackett","Arturo Calabro",
                    "Nana Caddell","Leandro Rarick",
                    "Carma Benfer",
                    "Luke Fu",
                    "Chrissy Harlan",
                    "Jayme Prioleau",
                    "Fredricka Ritchey",
                    "Sherita Casteel",
                    "Blaine Masters",
                    "Celestina Ginsburg",
                    "Maragaret Tortora",
                    "Marisela Wroblewski",
                    "Mickie Yost",
                    "Viviana Rister",
                    "Jerry Paterson",
                    "Jule Hoch",
                    "Edward Perrigo",
                    "Coleen Cuenca",
                    "Ahmed Loughman",
                    "Franchesca Roberts",
                    "Desmond Bleich",
                    "Hosea Bizier",
                    "Man Beresford",
                    "Israel Norsworthy",
                    "Graig Dinkins",
                    "Marylin Chiles",
                    "Candyce Muth",
                    "Jalisa Eutsey",
                    "Deandre Utley",
                    "Ruth Pigott",
                    "Francis Vansant",
                    "Nicki Capp",
                    "Gabriela Drewes",
                    "Jeanene Braaten",
                    "Jutta Bunton",
                    "Lashon Sires",
                    "Ila Ancona",
                    "Mohammed Flanagan",
                    "Kary Enloe",
                    "Corina Shumpert",
                    "Kip Sample",
                    "Reagan Aranda",
                    "Ellis Simons",
                    "Guy Manning",
                    "Inge Parrett",
                    "Wilburn Boehman",
                    "Thad Criswell",
                    "Nora Lanser",
                    "Julienne Dorrell",
                    "Bethann Beaver",
                    "Darleen Gaston",
                    "Lisandra Lukes",
                    "Jorge Fredricks",
                    "Dede Deshazo",
                    "Michelle Stocks",
                    "Patria Carrera",
                    "Meridith Sarles",
                    "Elke Barnhart",
                    "Sana Votaw",
                    "Sharmaine Bagwell",
                    "Jammie Kapoor",
                    "Adalberto Mccowan",
                    "Lowell Ruppel",
                    "Lasandra Burk",
                    "Edwardo Prevatte",
                    "Booker Berthelot",
                    "Dion Milbrandt",
                    "Ying Hamblin",
                    "Lashanda Brousseau",
                    "Lucile Gum",
                    "Mari Crompton",
                    "Georgiana Chaparro",
                    "Yong Caron",
                    "Belva Longmire",
                    "Jane Winrow",
                    "Asley Casias",
                    "Nola March",
                    "Tayna Stgermain",
                    "Marie Kestler",
                    "Shawanda Mondy",
                    "Rosann Coop",
                    "Angel Guertin",
                    "Aura Dansereau",
                    "Julietta Eargle",
                    "Clarine Shemwell",
                    "Craig Edmisten",
                    "Yasmin Legrande",
                    "Lakesha Trantham",
                    "Damon Blankenbaker",
                    "Russel Johannes",
                    "Jules Puglia",
                    "Katrina Aiken",
                    "Evangelina Besse",
                    "Callie Knerr",
                    "Octavia Peabody",
                    "Soila Ortmann",
                    "Charissa Kolbe"
                };

        public EmployeeSQLDAL()
        {
            connectionString = ConfigReader.SQLConnectionString;
        }

        //To View all employees details    
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> lstemployee = new List<Employee>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllEmployees", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Employee employee = new Employee();

                    //employee.EmpId = Convert.ToInt32(rdr["EmployeeID"]);
                    employee.EmpId = rdr["EmployeeID"].ToString();
                    employee.Name = rdr["Name"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Department = rdr["Department"].ToString();
                    employee.City = rdr["City"].ToString();

                    lstemployee.Add(employee);
                }
                con.Close();
            }
            return lstemployee;
        }

        //To Add new employee record    
        public void AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@City", employee.City);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //To Update the records of a particluar employee  
        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmpId", employee.EmpId);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@City", employee.City);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //Get the details of a particular employee  
        public Employee GetEmployeeData(string id)
        {
            Employee employee = new Employee();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM tblEmployee WHERE EmployeeID= " + id;
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //employee.EmpId = Convert.ToInt32(rdr["EmployeeID"]);
                    employee.EmpId = rdr["EmployeeID"].ToString();
                    employee.Name = rdr["Name"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Department = rdr["Department"].ToString();
                    employee.City = rdr["City"].ToString();
                }
            }
            return employee;
        }

        //To Delete the record on a particular employee  
        public void DeleteEmployee(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmpId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void PerformDeleteUpdate(string employeeId, int trackNumber)
        {
            using (var empDbConnection = new SqlConnection(connectionString))
            {
                empDbConnection.Open();
                var tran = empDbConnection.BeginTransaction();

                using (var deleteCommand = empDbConnection.CreateCommand())
                {
                    deleteCommand.CommandType = CommandType.StoredProcedure;
                    deleteCommand.CommandText = "spDeleteEmployee";

                    deleteCommand.Parameters.Add(new SqlParameter("@EmpId", SqlDbType.Int));
                    deleteCommand.Parameters["@EmpId"].Value = int.Parse(employeeId);
                    deleteCommand.Transaction = tran;
                    deleteCommand.ExecuteNonQuery();

                    using (var updateCommand = empDbConnection.CreateCommand())
                    {
                        updateCommand.CommandType = CommandType.StoredProcedure;
                        updateCommand.CommandText = "spUpdateEmployee_Optional";

                        updateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 20));
                        updateCommand.Parameters.Add(new SqlParameter("@Gender", SqlDbType.NVarChar, 6));
                        updateCommand.Parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar, 20));
                        updateCommand.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 20));

                        updateCommand.Transaction = tran;

                        updateCommand.Parameters["@Name"].Value = "TaskNumber" + employeeId;
                        updateCommand.Parameters["@Gender"].Value = (trackNumber % 2) == 0 ? "Male" : "Female";
                        updateCommand.Parameters["@Department"].Value = (trackNumber % 2) == 0 ? "HR" : "IT";
                        updateCommand.Parameters["@City"].Value = (trackNumber % 2) == 0 ? "Seattle" : "Chicago";
                        updateCommand.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                empDbConnection.Close();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace StudentDB.Pages.StudentDetails
{
    public class EditModel : PageModel
    {
        public StudentInfo studentInfo1 = new StudentInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"]; 

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentsdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM details WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentInfo1.id = "" + reader.GetInt32(0);
                                studentInfo1.name = reader.GetString(1);
                                studentInfo1.email = reader.GetString(2);
                                studentInfo1.address = reader.GetString(3);
                                studentInfo1.description = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message; 
            }
        }

        public void OnPost()
        {
            
            studentInfo1.id = Request.Form["id"];
            studentInfo1.name = Request.Form["name"];
            studentInfo1.email = Request.Form["email"];
            studentInfo1.address = Request.Form["address"];
            studentInfo1.description = Request.Form["description"];

            if (studentInfo1.id.Length == 0 || studentInfo1.name.Length == 0 ||
                studentInfo1.email.Length == 0 || studentInfo1.address.Length == 0 ||
                studentInfo1.description.Length == 0)
            {
                errorMessage = "All fields are required";
                return;

            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentsdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE details  SET name=@name, email=@email, address=@address, description=@description  WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", studentInfo1.name);
                        command.Parameters.AddWithValue("@email", studentInfo1.email);
                        command.Parameters.AddWithValue("@address", studentInfo1.address);
                        command.Parameters.AddWithValue("@description", studentInfo1.description);
                        command.Parameters.AddWithValue("@id", studentInfo1.id);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/StudentDetails/DetailsIndex");
            
        }
    }
}


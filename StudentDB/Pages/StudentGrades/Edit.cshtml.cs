using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentDB.Pages.StudentDetails;
using System.Data.SqlClient;

namespace StudentDB.Pages.StudentGrades
{
    public class EditModel : PageModel
    {
        public GradeInfo gradeInfo = new GradeInfo();
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
                    String sql = "SELECT * FROM grades WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                gradeInfo.id = "" + reader.GetInt32(0);
                                gradeInfo.name = reader.GetString(1);
                                gradeInfo.subject = reader.GetString(2);
                                gradeInfo.grade = reader.GetString(3);
               
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

            gradeInfo.id = Request.Form["id"];
            gradeInfo.name = Request.Form["name"];
            gradeInfo.subject = Request.Form["subject"];
            gradeInfo.grade = Request.Form["grade"];
      
            if (gradeInfo.id.Length == 0 || gradeInfo.name.Length == 0 ||
                gradeInfo.subject.Length == 0 || gradeInfo.grade.Length == 0)
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
                    String sql = "UPDATE grades  SET name=@name, subject=@subject, grade=@grade  WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", gradeInfo.name);
                        command.Parameters.AddWithValue("@subject", gradeInfo.subject);
                        command.Parameters.AddWithValue("@grade", gradeInfo.grade);                   
                        command.Parameters.AddWithValue("@id", gradeInfo.id);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/StudentGrades/GradesIndex");

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentDB.Pages.StudentDetails;
using System.Data.SqlClient;

namespace StudentDB.Pages.StudentGrades
{
    public class GradesIndexModel : PageModel
    {
        public List<GradeInfo> listGrades = new List<GradeInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=studentsdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM grades";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GradeInfo gradeInfo = new GradeInfo();
                                gradeInfo.id = "" + reader.GetInt32(0);
                                gradeInfo.name = reader.GetString(1);
                                gradeInfo.subject = reader.GetString(2);
                                gradeInfo.grade = reader.GetString(3);

                                listGrades.Add(gradeInfo);

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }


    }

    public class GradeInfo
    {
        public String id;
        public String name;
        public String subject;
        public String grade;
    }
}

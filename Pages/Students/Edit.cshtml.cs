using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebbilirCRUD.Pages.Students
{
    public class EditModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();
        public string errorMsg = "";
        public string scsMsg = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=DESKTOP-VA41PNG;Initial Catalog=clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Student WHERE Id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentInfo = new StudentInfo
                                {
                                    Id = reader.GetInt32(0).ToString(),
                                    Name = reader.GetString(1),
                                    ClassId = reader.GetInt32(2).ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            studentInfo.Name = Request.Form["name"];
            studentInfo.Id = Request.Form["id"];
            studentInfo.ClassId = Request.Form["dropdown"];
            studentInfo.oldId = Request.Form["oldid"];

            if (string.IsNullOrWhiteSpace(studentInfo.Name) || string.IsNullOrWhiteSpace(studentInfo.Id) || string.IsNullOrWhiteSpace(studentInfo.ClassId))
            {
                errorMsg = "All fields must be filled";
                return;
            }

            switch (studentInfo.ClassId)
            {
                case "A":
                    studentInfo.ClassId = "1";
                    break;
                case "B":
                    studentInfo.ClassId = "2";
                    break;
                case "C":
                    studentInfo.ClassId = "3";
                    break;
                default:
                    errorMsg = "Invalid class selected.";
                    return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-VA41PNG;Initial Catalog=clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Student SET Name= @name , ClassId= @classId , Id=@id WHERE Id=@oldid;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", studentInfo.Id);
                        command.Parameters.AddWithValue("@name", studentInfo.Name);
                        command.Parameters.AddWithValue("@classId", studentInfo.ClassId);
                        command.Parameters.AddWithValue("@oldid", studentInfo.oldId);
                        command.ExecuteNonQuery();
                    }
                }
                scsMsg = "Student Updated Successfully";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            Response.Redirect("/Students/Index");
        }

        

    }
}

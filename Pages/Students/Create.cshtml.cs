using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebbilirCRUD.Pages.Students
{
    public class CreateModel : PageModel
    {
        public StudentInfo studentInfo=new StudentInfo();
        public String errorMsg = "";
        public String scsMsg = "";
        public void OnGet()
        {
        
        }


        public void OnPost() {
            studentInfo.Name = Request.Form["name"];
            studentInfo.Id = Request.Form["id"];
            studentInfo.Class=new ClassInfo();
            studentInfo.ClassId =Request.Form["dropdown"];

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



            if (studentInfo.Name.Length==0 || 
                studentInfo.Id.Length == 0)
            {
                errorMsg = "All fields must be filled";
                return;
            }

            try
            {
                String connectionString = "Data Source=DESKTOP-VA41PNG;Initial Catalog=clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection=new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Student (Id,Name,ClassId)" +
                        "VALUES(@id,@name,@classId);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", studentInfo.Id);
                        command.Parameters.AddWithValue("@name", studentInfo.Name);
                        command.Parameters.AddWithValue("@classId", studentInfo.ClassId);
                        command.ExecuteNonQuery();
                    }

                    scsMsg = "New Student Added Successfully";
                    


                }
               
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }
            studentInfo.Name ="";
            studentInfo.Id = "";
            studentInfo.Class.ClassName = "";

            System.Threading.Thread.Sleep(1000);
            Response.Redirect("/Students/Index");




        }
    }
}

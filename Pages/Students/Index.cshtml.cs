using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Claims;

namespace WebbilirCRUD.Pages.Students
{
    public class IndexModel : PageModel
    {
        public List<StudentInfo> listStudents = new List<StudentInfo>();
       
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-VA41PNG;Initial Catalog=clients;Integrated Security=True;Encrypt=False";
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = @"
                        SELECT s.Name,s.Id, c.ClassName
                        FROM Student s
                        INNER JOIN Class c ON s.ClassId = c.Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentInfo studentInfo = new StudentInfo
                                {
                                    Name = reader.GetString(0),
                                    Id = reader.GetInt32(1).ToString(),
                                    Class = new ClassInfo
                                    {
                                        ClassName = reader.GetString(2)
                                    }
                                };

                                listStudents.Add(studentInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ex.ToString);
            }
        }
    }

    public class StudentInfo
    {

        public String oldId {  get; set; }
        public String Id { get; set; }
        public String Name { get; set; }
        public String ClassId { get; set; }

        public ClassInfo Class { get; set; }

    }

    public class ClassInfo
    {
        public String Id { get; set; }
        public String ClassName { get; set; }

    }


}

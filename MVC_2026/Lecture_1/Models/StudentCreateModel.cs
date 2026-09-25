using System.ComponentModel;

namespace Lecture_1.Models
{
    public class StudentCreateModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Grade { get; set; }
    }
}

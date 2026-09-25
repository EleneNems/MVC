using System.ComponentModel;

namespace Lecture_1.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [DisplayName("სახელი")]
        public string FirstName { get; set; }
        [DisplayName("გვარი")]
        public string LastName{ get; set; }
        [DisplayName("კლასი")]
        public int Grade { get; set; }

    }
}

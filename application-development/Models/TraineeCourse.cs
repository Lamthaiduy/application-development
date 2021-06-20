using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace application_development.Models
{
    public class TraineeCourse
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string TraineeId { get; set; }
        public Trainee Trainee  { get; set; }
    }
}
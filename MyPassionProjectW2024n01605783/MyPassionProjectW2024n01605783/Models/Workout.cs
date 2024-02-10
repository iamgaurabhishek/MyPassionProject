using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyPassionProjectW2024n01605783.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public string WorkoutDescription { get; set;}
        public string WorkoutDay { get; set; }

        public string WorkoutStatus { get; set; }

        // a workout has many exercises
        public ICollection<Exercise> Exercises { get; set; }

        // a user can do many workouts
        // UserId in Workout DB
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
    public class WorkoutDto
    {
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set;}
        public string WorkoutDescription { get; set;}
        public string WorkoutDay { get; set; }
        public string WorkoutStatus { get; set; }
        public int UserId { get; set; }
    }
}
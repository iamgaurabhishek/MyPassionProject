using MyPassionProjectW2024n01605783.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MyPassionProjectW2024n01605783.Controllers
{
    public class ExerciseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/ExerciseData/ListExercises
        // output a list of exercise in system.
        [HttpGet]
        [Route("api/exercisedata/listexercises")]
        public List<ExerciseDto> ListExercises() 
        {
           List<Exercise> Exercises =  db.Exercises.ToList();

            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();
            Exercises.ForEach(
                exercise => ExerciseDtos.Add(new ExerciseDto() 
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,
                WorkoutName = exercise.Workout.WorkoutName,
                WorkoutDay = exercise.Workout.WorkoutDay,
                NumberOfSets = exercise.NumberOfSets,
                ExerciseDescription = exercise.ExerciseDescription
                }));

            return ExerciseDtos;
        }
        // GET: api/ExerciseData/FindExercise/5
        [HttpGet]
        [ResponseType(typeof(Exercise))]
        [Route("api/exercisedata/findexercise/{id}")]
        public IHttpActionResult FindExercise(int id)
        {
            Exercise exercise = db.Exercises.Find(id);
            ExerciseDto ExerciseDto = new ExerciseDto()
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,
                ExerciseDescription = exercise.ExerciseDescription,
                WorkoutName = exercise.Workout.WorkoutName,
                WorkoutDay = exercise.Workout.WorkoutDay,
                NumberOfSets = exercise.NumberOfSets

            };
            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(ExerciseDto);
        }

        // POST: api/ExerciseData/UpdateExercise/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/ExerciseData/UpdateExercise/{id}")]
        public IHttpActionResult UpdateExercise(int id, Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exercise.ExerciseId)
            {

                return BadRequest();
            }

            db.Entry(exercise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ExerciseData/AddExercise
        [ResponseType(typeof(Exercise))]
        [HttpPost]
        [Route("api/ExerciseData/AddExercise")]
        public IHttpActionResult AddExercise(Exercise Exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exercises.Add(Exercise);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/ExerciseData/DeleteExercise/5
        [ResponseType(typeof(Exercise))]
        [HttpPost]
        [Route("api/ExerciseData/DeleteExercise/{id}")]
        public IHttpActionResult DeleteExercise(int id)
        {
            Exercise Exercise = db.Exercises.Find(id);
            if (Exercise == null)
            {
                return NotFound();
            }

            db.Exercises.Remove(Exercise);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseExists(int id)
        {
            return db.Exercises.Count(e => e.ExerciseId == id) > 0;
        }
    }
}

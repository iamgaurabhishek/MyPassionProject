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
            // Retrive all exercises from the database
           List<Exercise> Exercises =  db.Exercises.ToList();

            // Create a list of store ExerciseDTO objects
            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();
            // Transform each Exercise object into ExerciseDtos and add to the list
            Exercises.ForEach(
                exercise => ExerciseDtos.Add(new ExerciseDto() 
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,

                WorkoutName = exercise.Workout.WorkoutName,
                WorkoutDay = exercise.Workout.WorkoutDay,
                WorkoutId = exercise.Workout.WorkoutId,

                NumberOfSets = exercise.NumberOfSets,
                ExerciseDescription = exercise.ExerciseDescription,
               
                }));

            // return the list of ExerciseDto
            return ExerciseDtos;
        }
        

        /// <summary>
        /// Returns Exercise base on id
        /// </summary>
        /// <param name="ExerciseId">The ID to find workout</param>
        /// <returns></returns>
        // GET: api/ExerciseData/FindExercise/5
        [ResponseType(typeof(Exercise))]
        [HttpGet]        
        [Route("api/exercisedata/findexercise/{id}")]
        public IHttpActionResult FindExercise(int id)
        {
            Exercise exercise = db.Exercises.Find(id);
            // gets all the elements of Exercise into ExerciseDto
            ExerciseDto ExerciseDto = new ExerciseDto()
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,

                ExerciseDescription = exercise.ExerciseDescription,

                WorkoutName = exercise.Workout.WorkoutName,
                WorkoutDay = exercise.Workout.WorkoutDay,
                WorkoutId= exercise.Workout.WorkoutId,

                NumberOfSets = exercise.NumberOfSets,
                
            };

            if (exercise == null)
            {
                return NotFound();
            }
            
            return Ok(ExerciseDto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exercise"></param>
        /// <returns></returns>
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

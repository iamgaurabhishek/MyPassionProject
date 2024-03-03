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
    public class WorkoutDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/WorkoutData/ListWorkouts
        // output a list of Workout in system.
        [HttpGet]
        [Route("api/Workoutdata/listWorkouts")]
        public List<WorkoutDto> ListWorkouts()
        {
            List<Workout> Workouts = db.Workouts.ToList();

            List<WorkoutDto> WorkoutDtos = new List<WorkoutDto>();
            Workouts.ForEach(
                Workout => WorkoutDtos.Add(new WorkoutDto()
                {
                    WorkoutId = Workout.WorkoutId,
                    WorkoutName = Workout.WorkoutName,
                    WorkoutDay = Workout.WorkoutDay,

                    WorkoutStatus = Workout.WorkoutStatus,
                    WorkoutDescription = Workout.WorkoutDescription,

                    UserName = Workout.User.UserName,
                    UserId = Workout.User.UserId
                }));

            return WorkoutDtos;
        }
        // GET: api/WorkoutData/FindWorkout/5
        [ResponseType(typeof(Workout))]
        [HttpGet]
        [Route("api/Workoutdata/findWorkout/{id}")]
        public IHttpActionResult FindWorkout(int id)
        {
            Workout Workout = db.Workouts.Find(id);
            WorkoutDto WorkoutDto = new WorkoutDto()
            {
                WorkoutId = Workout.WorkoutId,
                WorkoutName = Workout.WorkoutName,
                WorkoutDescription = Workout.WorkoutDescription,

                WorkoutDay = Workout.WorkoutDay,
                WorkoutStatus = Workout.WorkoutStatus,

                UserName = Workout.User.UserName,
                UserId = Workout.User.UserId
            };
            if (Workout == null)
            {
                return NotFound();
            }

            return Ok(WorkoutDto);
        }

        // POST: api/WorkoutData/UpdateWorkout/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/WorkoutData/UpdateWorkout/{id}")]
        public IHttpActionResult UpdateWorkout(int id, Workout Workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Workout.WorkoutId)
            {

                return BadRequest();
            }

            db.Entry(Workout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
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

        // POST: api/WorkoutData/AddWorkout
        [ResponseType(typeof(Workout))]
        [HttpPost]
        [Route("api/WorkoutData/AddWorkout")]
        public IHttpActionResult AddWorkout(Workout Workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workouts.Add(Workout);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/WorkoutData/DeleteWorkout/5
        [ResponseType(typeof(Workout))]
        [HttpPost]
        [Route("api/WorkoutData/DeleteWorkout/{id}")]
        public IHttpActionResult DeleteWorkout(int id)
        {
            Workout Workout = db.Workouts.Find(id);
            if (Workout == null)
            {
                return NotFound();
            }

            db.Workouts.Remove(Workout);
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

        private bool WorkoutExists(int id)
        {
            return db.Workouts.Count(e => e.WorkoutId == id) > 0;
        }
    }
}

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
    public class UserDataController : ApiController
    {
            private ApplicationDbContext db = new ApplicationDbContext();
            // GET: api/UserData/ListUsers
            // output a list of User in system.
            [HttpGet]
            [Route("api/Userdata/listUsers")]
            public List<UserDto> ListUsers()
            {
                List<User> Users = db.Users.ToList();

                List<UserDto> UserDtos = new List<UserDto>();
                Users.ForEach(
                    User => UserDtos.Add(new UserDto()
                    {
                        UserId = User.UserId,
                        UserName = User.UserName,
                        UserAge = User.UserAge,
                        UserWeight = User.UserWeight
                    }));

                return UserDtos;
            }
            // GET: api/UserData/FindUser/5
            [HttpGet]
            [ResponseType(typeof(User))]
            [Route("api/Userdata/findUser/{id}")]
            public IHttpActionResult FindUser(int id)
            {
                User User = db.Users.Find(id);
                UserDto UserDto = new UserDto()
                {
                    UserId = User.UserId,
                    UserName = User.UserName,
                    UserDescription = User.UserDescription, 
                    UserAge = User.UserAge,
                    UserWeight = User.UserWeight

                };
                if (User == null)
                {
                    return NotFound();
                }

                return Ok(UserDto);
            }

            // POST: api/UserData/UpdateUser/5
            [ResponseType(typeof(void))]
            [HttpPost]
            [Route("api/Userdata/UpdateUser/{id}")]
            public IHttpActionResult UpdateUser(int id, User User)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != User.UserId)
                {

                    return BadRequest();
                }

                db.Entry(User).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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

            // POST: api/UserData/AddUser
            [ResponseType(typeof(User))]
            [HttpPost]
            [Route("api/UserData/AddUser")]
            public IHttpActionResult AddUser(User User)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Users.Add(User);
                db.SaveChanges();

                return Ok();
            }

            // POST: api/UserData/DeleteUser/5
            [ResponseType(typeof(User))]
            [HttpPost]
            [Route("api/UserData/DeleteUser/{id}")]
            public IHttpActionResult DeleteUser(int id)
            {
                User User = db.Users.Find(id);
                if (User == null)
                {
                    return NotFound();
                }

                db.Users.Remove(User);
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

            private bool UserExists(int id)
            {
                return db.Users.Count(e => e.UserId == id) > 0;
            }
    }
}
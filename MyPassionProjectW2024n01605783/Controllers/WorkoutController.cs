using MyPassionProjectW2024n01605783.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyPassionProjectW2024n01605783.Controllers
{
    public class WorkoutController : Controller
    {
        // GET: Workout/List
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
        }

        public ActionResult List()
        {
            // semester 2
            // assume we only can talk to the API through an HTTP request using an HTTP client in C# to gather the Workout data.
            //

            // we have our http client object


            //set the path to the resource
            string url = "workoutdata/listworkouts";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // we should try to digest this response into something we can use
            // digest it into an Workout data transfer object
            List<WorkoutDto> Workouts = response.Content.ReadAsAsync<List<WorkoutDto>>().Result;

            return View(Workouts);
        }

        public ActionResult Details(int id)
        {
            //objective: communicate with our animal data api to retrieve one animal
            //curl https://localhost:44301/api/Workoutdata/findWorkout/{id}

            string url = "Workoutdata/findWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            Debug.WriteLine("Workout to do : ");
            Debug.WriteLine(selectedWorkout.WorkoutName);


            return View(selectedWorkout);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Workout/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Workout/Create
        [HttpPost]
        public ActionResult Create(Workout Workout)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(workout.WorkoutName);
            //objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44324/api/animaldata/addanimal 
            string url = "Workoutdata/addWorkout";


            string jsonpayload = jss.Serialize(Workout);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Workout/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the Workout information

            string url = "Workoutdata/findWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            WorkoutDto selectedworkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            //Debug.WriteLine("workout received : ");
            //Debug.WriteLine(selectedworkout.WorkoutName);

            return View(selectedworkout);
        }

        // POST: Workout/Update/5
        [HttpPost]
        [Route("Workout/Update/{id}")]
        public ActionResult Update(int id, Workout workout)
        {
            try
            {
                Debug.WriteLine("The new animal info is:");
                Debug.WriteLine(workout.WorkoutStatus);
                Debug.WriteLine(workout.UserId);
                Debug.WriteLine(workout.WorkoutDay);

                //serialize into JSON
                //Send the request to the API

                string url = "WorkoutData/UpdateWorkout/" + id;


                string jsonpayload = jss.Serialize(workout);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/AnimalData/UpdateAnimal/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;




                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }
        // GET: workout/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "workoutdata/findworkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto selectedworkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            return View(selectedworkout);
        }

        // POST: workout/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "workoutdata/deleteworkout/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
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
    public class UserController : Controller
    {
        // GET: User
        // GET: User/List
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static UserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
        }

        public ActionResult List()
        {
            // semester 2
            // assume we only can talk to the API through an HTTP request using an HTTP client in C# to gather the User data.
            //

            // we have our http client object


            //set the path to the resource
            string url = "Userdata/listUsers";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // we should try to digest this response into something we can use
            // digest it into an User data transfer object
            List<UserDto> Users = response.Content.ReadAsAsync<List<UserDto>>().Result;

            return View(Users);
        }

        public ActionResult Details(int id)
        {
            //objective: communicate with our User data api to retrieve one User
            //curl https://localhost:44301/api/Userdata/findUser/{id}

            string url = "Userdata/findUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            Debug.WriteLine("User to do : ");
            Debug.WriteLine(selectedUser.UserName);


            return View(selectedUser);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: User/New
        public ActionResult New()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User User)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(User.UserName);
            //objective: add a new User into our system using the API
            //curl -H "Content-Type:application/json" -d @User.json https://localhost:44324/api/Userdata/addUser 
            string url = "Userdata/addUser";


            string jsonpayload = jss.Serialize(User);

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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the User information

            //objective: communicate with our User data api to retrieve one User
            //curl https://localhost:44324/api/Userdata/findUser/{id}

            string url = "userdata/findUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            //Debug.WriteLine("User received : ");
            //Debug.WriteLine(selectedUser.UserName);

            return View(selectedUser);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, User User)
        {
            try
            {
                Debug.WriteLine("The new User info is:");
                Debug.WriteLine(User.UserName);
                Debug.WriteLine(User.UserDescription);
       
                //serialize into JSON
                //Send the request to the API

                string url = "userdata/UpdateUser/" + id;


                string jsonpayload = jss.Serialize(User);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/UserData/UpdateUser/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selectedUser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "userdata/deleteuser/" + id;
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
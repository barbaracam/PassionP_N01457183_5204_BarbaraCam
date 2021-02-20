using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using amigopet.Models;
using amigopet.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace amigopet.Controllers
{
    public class AppointmentController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static AppointmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44324/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: Appointment/List
        public ActionResult List()
        {
            string url = "AppointmentData/GetAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<AppointmentDto> SelectedAppointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                return View(SelectedAppointments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            ShowAppointment ViewModel = new ShowAppointment();
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                ViewModel.Appointment = SelectedAppointment;

                //We don't need to throw any errors if this is null
                //A team not having any players is not an issue.
                url = "AppointmentData/GetPetforAppointment/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<PetDto> SelectedPets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;
                ViewModel.AppointmentPets = SelectedPets;


                url = "AppointmentData/GetPetWalkerForAppointment/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                //Put data into Team data transfer object
                IEnumerable<PetWalkerDto> SelectedPetWalkers = response.Content.ReadAsAsync<IEnumerable<PetWalkerDto>>().Result;
                ViewModel.AppointmentPetWalkers = SelectedPetWalkers;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                return View(SelectedAppointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Appointment AppointmentInfo)
        {
            
            string url = "AppointmentData/Appointment/" + id;
            Debug.WriteLine(jss.Serialize(AppointmentInfo));
            HttpContent content = new StringContent(jss.Serialize(AppointmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                return View(SelectedAppointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "AppointmentData/DeleteAppointment/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}

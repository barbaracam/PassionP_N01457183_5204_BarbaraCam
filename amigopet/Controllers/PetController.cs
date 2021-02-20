using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using amigopet.Models;
using amigopet.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace amigopet.Controllers
{
    public class PetController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static PetController()
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

        // GET: Pet/List
        public ActionResult List()
        {
            string url = "PetData/GetPets";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PetDto> SelectedPets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;
                return View(SelectedPets);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/Details/1
        public ActionResult Details(int id)
        {
            ShowPet ViewModel = new ShowPet();
            string url = "PetData/FindPet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into pet data transfer object
                PetDto SelectedPet = response.Content.ReadAsAsync<PetDto>().Result;
                ViewModel.Pet = SelectedPet;


                url = "PetData/FindAppointmentForPet/" + id;
                response = client.GetAsync(url).Result;
                IEnumerable<PetDto> SelectedAppointment = (IEnumerable<PetDto>)response.Content.ReadAsAsync<AppointmentDto>().Result;
                ViewModel.Appointment = (AppointmentDto)SelectedAppointment;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/Create
        public ActionResult Create()
        {
            UpdatePet ViewModel = new UpdatePet();
            //get information about Appointments the pet could have
            string url = "AppointmentData/GetAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AppointmentDto> PotentialAppointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            ViewModel.AllAppointments = PotentialAppointments;

            return View(ViewModel);
        }

                
        // POST: Pet/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Pet PetInfo)
        {
            Debug.WriteLine(PetInfo.PetName);
            Debug.WriteLine(PetInfo.PetBreed);
            Debug.WriteLine(PetInfo.PetTip);
            string url = "PetData/AddPet";
            Debug.WriteLine(jss.Serialize(PetInfo));
            HttpContent content = new StringContent(jss.Serialize(PetInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int PetID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id=PetID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pet/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pet/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pet/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

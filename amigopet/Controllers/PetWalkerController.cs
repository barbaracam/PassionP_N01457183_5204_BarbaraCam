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
    public class PetWalkerController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static PetWalkerController()
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

        // GET: PetWalker/List
        public ActionResult List()
        {
            string url = "PetWalkerData/GetPetWalkers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PetWalkerDto> SelectedPetWalkers = response.Content.ReadAsAsync<IEnumerable<PetWalkerDto>>().Result;
                return View(SelectedPetWalkers);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/Details/1
        public ActionResult Details(int id)
        {
            UpdatePetWalker ViewModel = new UpdatePetWalker();
            string url = "PetWalkerData/FindPetWalker/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into pet data transfer object
                PetWalkerDto SelectedPetWalker = response.Content.ReadAsAsync<PetWalkerDto>().Result;
                ViewModel.PetWalker = SelectedPetWalker;


                url = "PetWalkerData/FindAppointmentForPetWalker/" + id;
                response = client.GetAsync(url).Result;
                IEnumerable<AppointmentDto> SelectedAppointment = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                ViewModel.BookedAppointments = SelectedAppointment;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: PetWalker/Create
        public ActionResult Create()
        {
            UpdatePetWalker ViewModel = new UpdatePetWalker();
            //get information about Appointments the petwalker could have
            string url = "AppointmentData/GetAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AppointmentDto> PotentialAppointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            ViewModel.AllAppointments = PotentialAppointments;

            return View(ViewModel);

        }

        // POST: PetWalker/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(PetWalker PetWalkerInfo)
        {
            Debug.WriteLine(PetWalkerInfo.PetWalkerName);
            Debug.WriteLine(PetWalkerInfo.PetWalkerBio);
            string url = "PetWalkerdata/AddPetWalker";
            Debug.WriteLine(jss.Serialize(PetWalkerInfo));
            HttpContent content = new StringContent(jss.Serialize(PetWalkerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Teamid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Teamid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "PetWalkerData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                PetWalkerDto SelectedPetWalker = response.Content.ReadAsAsync<PetWalkerDto>().Result;
                return View(SelectedPetWalker);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, PetWalker PetWalkerInfo)
        {
            Debug.WriteLine(PetWalkerInfo.PetWalkerName);
            string url = "PetWalkerData/UpdatePetWalker/" + id;
            Debug.WriteLine(jss.Serialize(PetWalkerInfo));
            HttpContent content = new StringContent(jss.Serialize(PetWalkerInfo));
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


        // GET: PetWalker/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PetWalkerdata/FindPetWalker/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Sponsor data transfer object
                PetWalkerDto SelectedPetWalker = response.Content.ReadAsAsync<PetWalkerDto>().Result;
                return View(SelectedPetWalker);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: PetWalker/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "PetWalkerData/DeletePetWalker/" + id;
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

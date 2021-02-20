using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using amigopet.Models;
using System.Diagnostics;

namespace amigopet.Controllers
{
    public class AppointmentDataController : ApiController
    {
        //This variable is our database access point
        private AmigoPetDataContext db = new AmigoPetDataContext();

        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Appointment"
        //Choose context "AmigoPet Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.


        /// <summary>
        /// Gets a list or Appointmentss in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Appointments including their ID, name, and URL.</returns>
        /// <example>
        /// GET: api/AppointmentData/GetAppointments
        /// </example>
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        public IHttpActionResult GetAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Appointment in Appointments)
            {
                AppointmentDto NewAppointment = new AppointmentDto
                {
                    AppointmentID = Appointment.AppointmentID,
                    AppointmentTime = Appointment.AppointmentTime,
                    AppointmentNote = Appointment.AppointmentNote
                };
                AppointmentDto.Add(NewAppointment);
            }

            return Ok(AppointmentDtos);
        }

        /// <summary>
        /// Finds a particular appointment in the database with a 200 status code. If the appointment is not found, return 404.
        /// </summary>
        /// <param name="id">The appointment id</param>
        /// <returns>Information about the appointment, including appointment id, time, note, pet id
        // <example>
        // GET: api/AppointmentData/FindAppointment/2
        // </example>
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult FindAppointment(int id)
        {
            //Find the data
            Appointment Appointment = db.Appointments.Find(id);
            //if not found, return 404 status code.
            if (Appointment == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            AppointmentDto AppointmentDto = new AppointmentDto
            {
                AppointmentID = Appointment.AppointmentID,
                AppointmentTime = Appointment.AppointmentTime,
                AppointmentNote = Appointment.AppointmentNote,
                PetWalkerID = Appointment.PetWalkerID,
                PetID = Appointment.PetID
            };


            //pass along data as 200 status code OK response
            return Ok(AppointmentDto);
        }

        
        /// <summary>
        /// Gets a list of pet in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The input appointmentid</param>
        /// <returns>A list of pets associated with the appointment</returns>
        /// <example>
        /// GET: api/AppointmentData/GetPetsFromAppointment
        /// </example>
        [ResponseType(typeof(IEnumerable<PetDto>))]
        public IHttpActionResult GetPetForAppointment(int id)
        {   //select * from pets where pets.teamid = @id
            List<Pet> Pets = db
                .Pets
                .Where(p => p.PetID == id)
                .ToList();
            List<PetDto> PetDtos = new List<PetDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Pet in Pets)
            {
                PetDto NewPet = new PetDto
                {
                    PetID = Pet.PetID,
                    PetName = Pet.PetName,
                    PetBreed = Pet.PetBreed,
                    PetTip = Pet.PetTip
                    
                };
                PetDtos.Add(NewPet);
            }

            return Ok(PetDtos);
        }

        public IHttpActionResult GetPetWalkerForAppointment(int id)
        {   //select * from petwalkers where petwalkers.appointmentid = @id
            List<PetWalker> PetWalkers = db
                .PetWalkers
                .Where(pw => pw.PetWalkerID == id)
                .ToList();
            List<PetWalkerDto> PetWalkerDtos = new List<PetWalkerDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var PetWalker in PetWalkers)
            {
                PetWalkerDto NewPetWalker = new PetWalkerDto
                {
                    PetWalkerID = PetWalker.PetWalkerID,
                    PetWalkerName = PetWalker.PetWalkerName,
                    PetWalkerBio = PetWalker.PetWalkerBio
                    

                };
                PetWalkerDtos.Add(NewPetWalker);
            }

            return Ok(PetWalkerDtos);
        }



        public IHttpActionResult AddAppointment([FromBody] Appointment Appointment)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //insert into appointments
            db.Appointments.Add(Appointment);
            db.SaveChanges();

            return Ok(Appointment.AppointmentID);
        }

        /// <summary>
        /// Updates a Appointment in the database given information about the Appointment.
        /// </summary>
        /// <param name="id">The Appointment id</param>
        /// <param name="Team">An Appointment object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/AppointmentData/UpdateAppointment/5
        /// FORM DATA: Appointment JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, [FromBody] Appointment Appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Appointment.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(Appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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


        /// <summary>
        /// Deletes an appointment in the database
        /// </summary>
        /// <param name="id">The id of the appointment to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/AppointmentData/DeleteAppointment/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            if (Appointment == null)
            {
                return NotFound();
            }
            
            db.Appointments.Remove(Appointment);
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

        /// <summary>
        /// Finds a appointment in the system. Internal use only.
        /// </summary>
        /// <param name="id">The appointment id</param>
        /// <returns>TRUE if the appointment exists, false otherwise.</returns>
        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }












    }
}

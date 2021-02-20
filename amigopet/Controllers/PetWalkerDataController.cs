using System;
using System.IO;
using System.Web;
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
    public class PetWalkerDataController : ApiController
    {
        private AmigoPetDataContext db = new AmigoPetDataContext();

        [ResponseType(typeof(IEnumerable<PetWalkerDto>))]
        public IHttpActionResult GetPetWalkers()
        {
            List<PetWalker> PetWalkers = db.PetWalkers.ToList();
            List<PetWalkerDto> PetWalkerDtos = new List<PetWalkerDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var PetWalker in PetWalkers)
            {
                PetWalkerDto NewPetWalker = new PetWalkerDto
                {
                    PetWalkerID = PetWalker.PetWalkerID,
                    PetWalkerName = PetWalker.PetWalkerName,
                    PetWalkerBio = PetWalker.PetWalkerBio,
                    

                };
                PetWalkerDtos.Add(NewPetWalker);
            }

            return Ok(PetWalkerDtos);
        }


        /// <summary>
        /// Finds a particular pet in the database with a 200 status code. If the pet is not found, return 404.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <returns>Information about the pet, including pet id, name, breed and tips</returns>
        // <example>
        // GET: api/PetData/FindPet/3
        // </example>
        [HttpGet]
        [ResponseType(typeof(PetWalkerDto))]
        public IHttpActionResult FindPetWalker(int id)
        {
            //Find the data
            PetWalker PetWalker = db.PetWalkers.Find(id);
            //if not found, return 404 status code.
            if (PetWalker == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            PetWalkerDto PetWalkerDto = new PetWalkerDto
            {
                PetWalkerID = PetWalker.PetWalkerID,
                PetWalkerName = PetWalker.PetWalkerName,
                PetWalkerBio = PetWalker.PetWalkerBio                

            };

            //pass along data as 200 status code OK response
            return Ok(PetWalkerDto);
        }

        /// <summary>
        /// Finds a particular Appointment in the database given a petwalker id with a 200 status code. If the Appointment is not found, return 404.
        /// </summary>
        /// <param name="id">The petwalker id</param>
        /// <returns>Information about the Appointment, including Appointment id, time, tips</returns>
        // <example>
        // GET: api/PetData/FindAppointmentForPetWalker/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        public IHttpActionResult FindAppointmentForPetWalker(int id)
        {
            List<Appointment> Appointments = db.Appointments.Where(a => a.PetWalkerID == id)
                .ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Appointment in Appointments)
            {
                AppointmentDto NewAppointment = new AppointmentDto
                {
                    AppointmentID = Appointment.AppointmentID,
                    AppointmentTime = Appointment.AppointmentTime,
                    AppointmentNote = Appointment.AppointmentNote,
                    PetID = Appointment.PetID,
                    PetWalkerID = Appointment.PetWalkerID

                };
                AppointmentDtos.Add(NewAppointment);
            }

            return Ok(AppointmentDtos);
        }

        /// <summary>
        /// Adds a petwalker to the database.
        /// </summary>
        /// <param name="et">A petwalker object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/PetWalkerData/AddPetWalker
        ///  FORM DATA: Pet JSON Object
        /// </example>

        [ResponseType(typeof(PetWalker))]
        [HttpPost]
        public IHttpActionResult AddPetWalker([FromBody] PetWalker PetWalker)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetWalkers.Add(PetWalker);
            db.SaveChanges();

            return Ok(PetWalker.PetWalkerID);
        }


        /// <summary>
        /// Updates a PetwALKER in the database given information about the PetWalker.
        /// </summary>
        /// <param name="id">The PetWalker id</param>
        /// <param name="Team">A PetWalker object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/PetWalkerData/UpdatePetWalker/5
        /// FORM DATA: Team JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePetWalker(int id, [FromBody] PetWalker PetWalker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != PetWalker.PetWalkerID)
            {
                return BadRequest();
            }

            db.Entry(PetWalker).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetWalkerExists(id))
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
        /// Deletes a PetWalker in the database
        /// </summary>
        /// <param name="id">The id of the PetWalker to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/PetWalker/DeleteSponsor/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeletePetWalker(int id)
        {
            PetWalker PetWalker = db.PetWalkers.Find(id);
            if (PetWalker == null)
            {
                return NotFound();
            }

            db.PetWalkers.Remove(PetWalker);
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
        /// Finds a Sponsor in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Sponsor id</param>
        /// <returns>TRUE if the Sponsor exists, false otherwise.</returns>
        private bool PetWalkerExists(int id)
        {
            return db.PetWalkers.Count(e => e.PetWalkerID == id) > 0;
        }

        
    }
}

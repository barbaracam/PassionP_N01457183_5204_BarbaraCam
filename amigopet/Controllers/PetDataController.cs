﻿using System;
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
    public class PetDataController : ApiController
    {

        private AmigoPetDataContext db = new AmigoPetDataContext();

        [ResponseType(typeof(IEnumerable<PetDto>))]
        public IHttpActionResult GetPets()
        {
            List<Pet> Pets = db.Pets.ToList();
            List<PetDto> PetDtos = new List<PetDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Pet in Pets)
            {
                PetDto NewPet = new PetDto
                {
                    PetID = Pet.PetID,
                    PetName = Pet.PetName,
                    PetBreed = Pet.PetBreed,
                    PetTip = Pet.PetTip,
                                     
                };
                PetDtos.Add(NewPet);
            }

            return Ok(PetDtos);
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
        [ResponseType(typeof(PetDto))]
        public IHttpActionResult FindPet(int id)
        {
            //Find the data
            Pet Pet = db.Pets.Find(id);
            //if not found, return 404 status code.
            if (Pet == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            PetDto PetDto = new PetDto
            {
                PetID = Pet.PetID,
                PetName = Pet.PetName,
                PetBreed = Pet.PetBreed,
                PetTip = Pet.PetTip,                
               
            };

            //pass along data as 200 status code OK response
            return Ok(PetDto);
        }

        /// <summary>
        /// Finds a particular Appointment in the database given a pet id with a 200 status code. If the Appointment is not found, return 404.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <returns>Information about the Appointment, including Appointment id, time, tips</returns>
        // <example>
        // GET: api/PetData/FindAppointmentForPet/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        public IHttpActionResult FindAppointmentForPet(int id)
        {
            List<Appointment> Appointments = db.Appointments.Where(a => a.PetID == id)
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
        /// Adds a pet to the database.
        /// </summary>
        /// <param name="et">A pet object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/PetData/AddPet
        ///  FORM DATA: Pet JSON Object
        /// </example>

        [ResponseType(typeof(Pet))]
        [HttpPost]
        public IHttpActionResult AddPet([FromBody] Pet Pet)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pets.Add(Pet);
            db.SaveChanges();

            return Ok(Pet.PetID);
        }

        /// <summary>
        /// Updates a Pet in the database given information about the Pet.
        /// </summary>
        /// <param name="id">The Pet id</param>
        /// <param name="Team">A Pet object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/TeamData/UpdatePet/5
        /// FORM DATA: Pet JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePet(int id, [FromBody] Pet Pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Pet.PetID)
            {
                return BadRequest();
            }

            db.Entry(Pet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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
        /// Deletes a pet in the database
        /// </summary>
        /// <param name="id">The id of the pet to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/PetData/DeletePet/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeletePet(int id)
        {
            Pet Pet = db.Pets.Find(id);
            if (Pet == null)
            {
                return NotFound();
            }
            
            db.Pets.Remove(Pet);
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
        /// Finds a pet in the system. Internal use only.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <returns>TRUE if the pet exists, false otherwise.</returns>
        private bool PetExists(int id)
        {
            return db.Pets.Count(e => e.PetID == id) > 0;
        }













    }
}

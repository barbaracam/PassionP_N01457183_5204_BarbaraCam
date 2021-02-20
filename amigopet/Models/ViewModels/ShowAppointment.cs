using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace amigopet.Models.ViewModels
{
    public class ShowAppointment
    {
        //Information about the appointment
        public AppointmentDto Appointment { get; set; }

        //Information about all pets on that appointment
        public IEnumerable<PetDto> AppointmentPets { get; set; }

        //Information about the petwalker for that appointment
        public IEnumerable<PetWalkerDto> AppointmentPetWalkers { get; set; }
    }
}
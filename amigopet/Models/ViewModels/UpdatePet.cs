using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace amigopet.Models.ViewModels
{
    public class UpdatePet
    {
        //Information about a pet
        public PetDto Pet { get; set; }
        //Presents the pet with a choice of appointments
        public IEnumerable<AppointmentDto> AllAppointments { get; set; }
    }
}
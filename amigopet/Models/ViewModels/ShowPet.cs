using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace amigopet.Models.ViewModels
{
    public class ShowPet
    {
        //information about the pet
        public PetDto Pet { get; set; }
        //information about the Appointment the pet has
        public AppointmentDto Appointment { get; set; }
        //pet walker info
        public PetWalkerDto PetWalker { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace amigopet.Models.ViewModels
{
    public class UpdatePetWalker
    {

        //base information about the petwalker
        public PetWalkerDto PetWalker { get; set; }

        //display all teams that this sponsor is sponsoring
        public IEnumerable<AppointmentDto> BookedAppointments { get; set; }

        //Presents the pet with a choice of appointments
        public IEnumerable<AppointmentDto> AllAppointments { get; set; }
    }
}
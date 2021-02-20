using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amigopet.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        public string AppointmentTime { get; set; }

        public string AppointmentNote { get; set; }


        //Utilizes the inverse property to specify the "Many"
        //https://www.entityframeworktutorial.net/code-first/inverseproperty-dataannotations-attribute-in-code-first.aspx

        //One Appointment many pets
        public ICollection<Pet> Pets { get; set; }

        //One Appointment one Petwalker
        [ForeignKey("Petwalker")]
        public int PetWalkerID { get; set; }
        public virtual PetWalker Petwalker { get; set; }

        [ForeignKey("Pet")]
        public int PetID { get; set; }
        public virtual Pet Pet { get; set; }

    }

    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentNote { get; set; }

        public int PetID { get; set; }

        public int PetWalkerID { get; set; }

        internal static void Add(AppointmentDto newAppointment)
        {
            throw new NotImplementedException();
        }
    }
}
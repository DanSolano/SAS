using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BitSolutions.Models
{
    public class FullTicket
    {
        [Display(Name = "Ticket code")]
        public string ticketCode { get; set; }

        [Display(Name = "Enterprice")]
        public string enterprice { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        public FullTicket() { }

        public FullTicket(string ticketCode, string enterprice, string description)
        {
            this.ticketCode = ticketCode;
            this.enterprice = enterprice;
            this.description = description;
        }
    }
}
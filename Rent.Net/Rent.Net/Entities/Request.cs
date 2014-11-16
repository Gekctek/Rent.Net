using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rent.Net.Entities
{
    public class Request
    {
        public Request()
        {

        }

        public int RequestId { get; set; }

        public string PayerId { get; set; }
        public string PayeeId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }






        public virtual ApplicationUser Payer { get; set; }
        public virtual ApplicationUser Payee { get; set; }
    }
}
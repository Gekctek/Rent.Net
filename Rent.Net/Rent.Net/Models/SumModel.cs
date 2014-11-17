using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent.Net.Models
{
    public class SumModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public SumModel(string name, decimal amount)
        {
            this.Name = name;
            this.Amount = amount;
        }

    }
}
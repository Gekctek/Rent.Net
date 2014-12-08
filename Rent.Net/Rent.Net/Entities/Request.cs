using FluentValidation;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace Rent.Net.Entities
{
    [Validator(typeof(RequestValidator))]
    public class Request
    {
        public Request()
        {

        }

        public int RequestId { get; set; }

        public int PayerId { get; set; }
        public int PayeeId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public DateTime Created { get; set; }





        [JsonIgnore]
        public virtual User Payer { get; set; }
        [JsonIgnore]
        public virtual User Payee { get; set; }

        [JsonIgnore]
        public virtual ICollection<Payment> Payments { get; set; }
    }

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            this.RuleFor(r => r.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0.");
        }
    }
}
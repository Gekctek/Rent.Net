using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent.Net.Entities
{
    [Validator(typeof(PaymentValidator))]
    public class Payment
    {
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public bool Approved { get; set; }
        public DateTime Created { get; set; }

        public int? RequestId { get; set; }
        public int PayeeId { get; set; }
        public int PayerId { get; set; }

        [JsonIgnore]
        public virtual User Payee { get; set; }
        [JsonIgnore]
        public virtual User Payer { get; set; }
        [JsonIgnore]
        public virtual Request Request { get; set; }
    }

    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            this.RuleFor(p => p.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0.");
        }
    }
}
using FluentValidation;
using FluentValidation.Attributes;
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

        public int? RequestId { get; set; }
        public string PayeeId { get; set; }
        public string PayerId { get; set; }

        public virtual ApplicationUser Payee { get; set; }
        public virtual ApplicationUser Payer { get; set; }
        public virtual Request Request { get; set; }
    }

    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            this.RuleFor(r => r.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0.");
        }
    }
}
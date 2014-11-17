using FluentValidation;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using System.Collections.Generic;

namespace Rent.Net.Entities
{
    [Validator(typeof(RequestValidator))]
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
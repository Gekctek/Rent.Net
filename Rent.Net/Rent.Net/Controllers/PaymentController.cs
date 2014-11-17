using Rent.Net.Entities;
using Rent.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Rent.Net.Controllers
{
    public class PaymentController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Send()
        {
            Payment payment = new Payment
            {
                PayerId = this.UserId
            };
            return this.SendView(payment);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Send(Payment payment)
        {
            if (this.ModelState.IsValid)
            {
                this.Database.Payments.Add(payment);
                this.Database.SaveChanges();
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.SendView(payment);
            }
        }

        private ActionResult SendView(Payment payment)
        {
            this.AddUsersToViewBag();
            return this.View(payment);
        }

        public ActionResult Sum()
        {
            List<SumModel> model = new List<SumModel>();

            List<ApplicationUser> users = this.OtherUsers.ToList();
            foreach (ApplicationUser appUser in users)
            {
                IQueryable<Payment> approvedPayments = this.Database.Payments.Where(p => p.Approved);
                List<Payment> paymentsToMe = approvedPayments.Where(p => p.PayeeId == this.UserId).ToList();
                List<Payment> paymentsFromMe = approvedPayments.Where(p => p.PayerId == this.UserId).ToList();
                decimal amount = 0;
                foreach (Payment payment in paymentsToMe)
                {
                    amount += payment.Amount;
                }
                foreach (Payment payment in paymentsFromMe)
                {
                    amount -= payment.Amount;
                }
                SumModel receiptInfo = new SumModel(appUser.UserName, amount);

                model.Add(receiptInfo);
            }
            return this.View(model);
        }
    }
}
namespace Rent.Net.ApiControllers
{
    public class PaymentController : BaseApiController
    {
        public IHttpActionResult Delete(int id)
        {
            Payment payment = this.Database.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return this.BadRequest("Payment does not exist with the Id of " + id);
            }
            this.Database.Entry(payment).State = System.Data.Entity.EntityState.Deleted;
            this.Database.SaveChanges();
            return this.Ok();
        }
    }

    public class PaymentActionController : BaseApiController
    {
        public IHttpActionResult PostByRequest(int id)
        {
            Request request = this.Database.Requests.FirstOrDefault(p => p.RequestId == id);
            if (request == null)
            {
                return this.BadRequest("Request does not exist with the Id of " + id);
            }
            Payment payment = new Payment
            {
                Notes = request.Notes,
                Amount = request.Amount,
                PayerId = this.UserId,
                RequestId = request.RequestId,
                PayeeId = request.PayeeId
            };
            this.Database.Payments.Add(payment);
            this.Database.SaveChanges();
            return this.Ok();
        }

        public IHttpActionResult Approve(int id)
        {
            Payment payment = this.Database.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return this.BadRequest("Payment does not exist with the Id of " + id);
            }
            payment.Approved = true;
            if (payment.Request != null)
            {
                this.Database.Entry(payment.Request).State = System.Data.Entity.EntityState.Deleted;
            }
            this.Database.SaveChanges();
            return this.Ok();
        }
    }
}
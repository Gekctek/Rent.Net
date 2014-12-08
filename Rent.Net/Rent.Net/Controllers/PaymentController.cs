using Rent.Net.Entities;
using Rent.Net.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace Rent.Net.ApiControllers
{
    public class PaymentController : BaseApiController
    {
        public IHttpActionResult Get(string filter = null)
        {
            var payments = this.Database.Payments.AsQueryable();
            if (filter != null)
            {
                if (string.Equals(filter, BaseApiController.SentFilter, StringComparison.OrdinalIgnoreCase))
                {
                    payments = payments.Where(p => p.PayerId == this.UserId);
                }
                else if(string.Equals(filter, BaseApiController.ReceivedFilter, StringComparison.OrdinalIgnoreCase))
                {
                    payments = payments.Where(p => p.PayeeId == this.UserId);
                }
            }
            return this.Ok(payments.ToList());
        }

        public IHttpActionResult Get(int id)
        {
            Payment payment = this.Database.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return this.NotFound();
            }
            return this.Ok(id);
        }

        public IHttpActionResult Post(Payment payment)
        {
            payment.PayerId = this.UserId;
            payment.Created = DateTime.Now;
            if (this.ModelState.IsValid)
            {
                this.Database.Payments.Add(payment);
                this.Database.SaveChanges();
                return this.Ok(payment);
            }
            else
            {
                return this.BadRequest(this.ModelState);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            Payment payment = this.Database.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return this.BadRequest("Payment does not exist with the Id of " + id);
            }
            this.Database.Entry(payment).State = EntityState.Deleted;
            this.Database.SaveChanges();
            return this.Ok();
        }
    }

    public class PaymentApproveController : BaseApiController
    {
        public IHttpActionResult Post(int id)
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

    public class PaymentByRequestController : BaseApiController
    {
        public IHttpActionResult Post(int id)
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
    }

    public class PaymentSumComtroller : BaseApiController
    {
        public IHttpActionResult Get()
        {
            List<SumModel> model = new List<SumModel>();

            List<User> users = this.OtherUsers.ToList();
            foreach (User appUser in users)
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
            return this.Ok(model);
        }
    }

    public class PaymentApprovalController : BaseApiController
    {
        public IHttpActionResult Post(int id)
        {
            Payment payment = this.Database.Payments.FirstOrDefault(p => p.PaymentId == id);
            if(payment == null)
            {
                return this.NotFound();
            }
            if(payment.PayeeId != this.UserId)
            {
                return this.BadRequest("Only users who are the receiver of the payment can approve it.");
            }
            payment.Approved = true;
            if(payment.Request != null)
            {
                Request request = payment.Request;
                payment.RequestId = null;
                payment.Request = null;
                this.Database.SaveChanges();
                this.Database.Requests.Remove(request);
            }
            this.Database.SaveChanges();
            return this.Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            Payment payment = this.Database.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return this.NotFound();
            }
            if (payment.PayeeId != this.UserId)
            {
                return this.BadRequest("Only users who are the receiver of the payment can reject it.");
            }
            this.Database.Entry(payment).State = EntityState.Deleted;
            this.Database.SaveChanges();
            return this.Ok();
        }
    }
}
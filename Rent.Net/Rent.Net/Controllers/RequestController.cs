using Rent.Net.Entities;
using System.Collections.Generic;
using System.Linq;
using Rent.Net.Common;
using System.Web.Http;
using System;

namespace Rent.Net.ApiControllers
{
    public class RequestController : BaseApiController
    {
        public IHttpActionResult Get(string filter = null)
        {
            var requests = this.Database.Requests.AsQueryable();
            if (filter != null)
            {
                if (string.Equals(filter, BaseApiController.SentFilter, StringComparison.OrdinalIgnoreCase))
                {
                    requests = requests.Where(r => r.PayeeId == this.UserId);
                }
                else if (string.Equals(filter, BaseApiController.ReceivedFilter, StringComparison.OrdinalIgnoreCase))
                {
                    //Only display if there are no pending payments
                    requests = requests.Where(r => r.PayerId == this.UserId && r.Payments.Count < 1);
                }
            }
            return this.Ok(requests);
        }
        public IHttpActionResult Get(int id)
        {
            Request request = this.Database.Requests.FirstOrDefault(r => r.RequestId == id);
            if(request == null)
            {
                return this.NotFound();
            }
            return this.Ok(request);
        }


        public IHttpActionResult Post(Request request)
        {
            request.PayeeId = this.UserId;
            request.Created = DateTime.Now;
            if (this.ModelState.IsValid)
            {
                this.Database.Requests.Add(request);
                this.Database.SaveChanges();
                return this.Ok(request);
            }
            else
            {
                return this.BadRequest(this.ModelState);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            Request request = this.Database.Requests.FirstOrDefault(r => r.RequestId == id);
            if(request == null)
            {
                return this.BadRequest("Request does not exist with the id of " + id);
            }
            List<Payment> paymentsFromRequest = this.Database.Payments.Where(p => p.RequestId == request.RequestId).ToList();
            if (!paymentsFromRequest.IsNullOrEmpty())
            {
                foreach(Payment payment in paymentsFromRequest)
                {
                    this.Database.Entry(payment).State = System.Data.Entity.EntityState.Deleted;
                }
            }
            this.Database.Entry(request).State = System.Data.Entity.EntityState.Deleted;
            this.Database.SaveChanges();
            return this.Ok();
        }
    }
}
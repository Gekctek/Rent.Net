using System;
using System.Collections.Generic;

namespace Rent.Net.Entities
{
    public class User
    {
        public User()
        {

        }

        public User(string username, string password)
        {
            this.UserName = username;
            this.Password = User.EncodePassword(password);
        }

        public static string EncodePassword(string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Request> RequestsTo { get; set; }
        public virtual ICollection<Request> RequestsFrom { get; set; }
        public virtual ICollection<Payment> PaymentsTo { get; set; }
        public virtual ICollection<Payment> PaymentsFrom { get; set; }
    }
}
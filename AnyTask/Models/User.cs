using System;
namespace AnyTask.Models
{
    public class User
    {
        /// <summary>
        /// A unique user ID, assigned to the requesting user.
        /// </summary>
        public string Uid { get; set; }
        public string FirstName { get; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP231_W2020_OnDuty_Web.Models
{
    public class User : IdentityUser
    {
        public User()
            : base()
        {
            Favorites = new List<User>();
        }
        [PersonalData] // PersonalData annotation is used to make the property downloadable in the Download Personal Data link
        public string Name { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public bool IsProvider { get; set; }
        [PersonalData]
        public string Latitude { get; set; }
        [PersonalData]
        public string Longitude { get; set; }
        // Category of service that can be provided by this user if a Provider
        public string Category { get; set; }

        public int Rate { get; set; }

        public int numberEvaluations { get; set; }

        public double Evaluation { get; set; }
        public List<User> Favorites { get; set; }
    }
}

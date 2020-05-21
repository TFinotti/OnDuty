using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP231_W2020_OnDuty_Web.ViewModel
{
    public class ProviderViewModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Rate { get; set; }
        public bool IsFavorite { get; set; }
    }
}

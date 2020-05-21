using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace COMP231_W2020_OnDuty_Web.Models
{
    /// <summary>
    /// Category os services that can be provided
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP231_W2020_OnDuty_Web.Models
{
    public interface IRepository
    {
        IQueryable<Service> Services { get; }
        //IQueryable<Province> Provinces { get; }
        IQueryable<Category> Categories { get; }
    }
}

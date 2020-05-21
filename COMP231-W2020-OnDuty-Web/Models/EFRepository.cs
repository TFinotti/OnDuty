using COMP231_W2020_OnDuty_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP231_W2020_OnDuty_Web.Models
{
    public class EFRepository : IRepository
    {
        private ApplicationDbContext context;
        public EFRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Service> Services => context.Services;
        //public IQueryable<Province> Provinces => context.Provinces;
        public IQueryable<Category> Categories => context.Categories;
    }
}

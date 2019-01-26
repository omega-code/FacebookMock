using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    interface ISeedable
    {
        int Seed(AppDbContext context);
    }
}

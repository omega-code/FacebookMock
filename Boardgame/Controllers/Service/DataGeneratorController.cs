using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boardgame.Controllers.Service
{
    public class DataGeneratorController : BaseController
    {
        public DataGeneratorController(IServiceProvider sp) : base(sp) { }

        public IActionResult SeedData()
        {
            var lst = new List<string>();

            if (!_dbContext.Categories.Any())
            {
                var count = new CategoryConfiguration().Seed(_dbContext);
                lst.Add($"{count} Categories have been seeded.");
            }

            if (!_dbContext.Products.Any())
            {
                var count = new ProductConfiguration().Seed(_dbContext);
                lst.Add($"{count} Products have been seeded.");
            }

            if (lst.Count == 0)
            {
                lst.Add("Nothing has been seeded.");
            }

            return Json(lst);
        }

        public IActionResult SeedInsights()
        {
            var lst = new List<string>();

            if (!_dbContext.FacebookPages.Any())
            {
                var count = new FacebookPageConfiguration().Seed(_dbContext);
                lst.Add($"{count} Facebook pages have been seeded.");
            }

            if (!_dbContext.LeadCampaigns.Any())
            {
                var count = new LeadCampaignConfiguration().Seed(_dbContext);
                lst.Add($"{count} Lead campaigns have been seeded.");
            }

            if (!_dbContext.LeadCampaignInsights.Any())
            {
                var count = new LeadCampaignInsightsConfiguration().Seed(_dbContext);
                lst.Add($"{count} Lead insights have been seeded.");
            }

            if (lst.Count == 0)
            {
                lst.Add("Nothing has been seeded.");
            }


            return Json(lst);
        }

        public IActionResult CleanInsights()
        {
            var lst = new[] { nameof(_dbContext.LeadCampaignInsights), nameof(_dbContext.LeadCampaigns), nameof(_dbContext.FacebookPages), };

            try
            {
                _dbContext.Database.OpenConnection();
                foreach (var element in lst)
                {
                    var command = _dbContext.Database.GetDbConnection().CreateCommand();
                    command.CommandText = $"DELETE FROM [{element}]";
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _dbContext.Database.CloseConnection();
            }
            return Json("Deleted");
        }
    }
}
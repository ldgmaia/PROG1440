using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquashNiagara.Data;
using SquashNiagara.Models;

namespace SquashNiagara.Controllers
{
    public class HomeController : Controller
    {
        private readonly SquashNiagaraContext _context;

        public HomeController(SquashNiagaraContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Captain") || User.IsInRole("User"))
            {
                var player = _context.Players
                .FirstOrDefault(m => m.Email == User.Identity.Name);

                if (player != null && player.firstLogin)
                    return Redirect("identity/Account/Manage/ChangePassword");

                return RedirectToAction("Details", "Players", new { id = player.ID });
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        //public IActionResult TeamRanking()
        //{
        //    return View();
        //}

        //public IActionResult PlayerRanking()
        //{
        //    return View();
        //}
        // GET: Matches
        public async Task<IActionResult> PlayerRanking()
        {
            var playerRankings = _context.PlayerRankings.Include(p => p.Player).Include(p => p.Season).Include(p => p.Division);
            return View(await playerRankings.ToListAsync());
        }

        public async Task<IActionResult> TeamRanking()
        {
            var teamRankings = _context.TeamRankings.Include(p => p.Team).Include(p => p.Season).Include(p => p.Division);
            return View(await teamRankings.ToListAsync());
        }

        public IActionResult Standings()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

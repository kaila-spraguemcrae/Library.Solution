using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Controllers
{
  [Authorize]
  public class CopiesController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public CopiesController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
// /books/2/copies
    [HttpGet("/books/{BookId}/copies")]
    public ActionResult Index(int BookId)
    {
      int counter = 0;
      int copyOutCounter = 0;
      int copiesAvailable = 0;
      List<Copy> model = _db.Copies.Include(copies => copies.Book).ToList();
      foreach (Copy copy in model)
      {
        if (copy.BookId == BookId)
        {
          counter++;
          if (copy.CopyOut == true)
          {
            copyOutCounter++;
          }
        }
      }
      copiesAvailable = counter - copyOutCounter;
      ViewBag.Model = model; 
      ViewBag.Counter = counter; 
      ViewBag.CopiesAvailable = copiesAvailable;
      return View();
    }
  }
} 
// void Application_Start(object sender, EventArgs e) 
// {
//     RegisterRoutes(RouteTable.Routes);
// }
// public static void RegisterRoutes(RouteCollection routes)
// {
//     Route reportRoute = new Route("{locale}/{year}", new ReportRouteHandler());
//     reportRoute.Defaults = new RouteValueDictionary { { "locale", "en-US" }, { "year", DateTime.Now.Year.ToString() } };
//     reportRoute.Constraints = new RouteValueDictionary { { "locale", "[a-z]{2}-[a-z]{2}" }, { "year", @"\d{4}" } };
//     reportRoute.DataTokens = new RouteValueDictionary { { "format", "short" } };
//     routes.Add(reportRoute);
// }

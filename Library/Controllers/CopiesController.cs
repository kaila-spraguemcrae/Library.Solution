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
       /// .ThenInclude(book=>book.BookId==BookId).ToList();
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
      ViewBag.CopiesAvailable = counter - copyOutCounter;
      ViewBag.Model = model; 
      ViewBag.Counter = counter; 
      ViewBag.BookId = BookId;
      return View();
      

    }

    [HttpGet("/books/{BookId}/copies/create")]
    public ActionResult Create(int BookId)
    {
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View();
    }
    [HttpPost("/books/{BookId}/copies/create")]
    public ActionResult Create(Copy copy, int BookId)
    {
      _db.Copies.Add(copy);
      _db.SaveChanges();
      return RedirectToAction("Details", "Books", new { id = BookId});
    }
  }
} 

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
  
  public class AuthorsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthorsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    
    public ActionResult Index()
    {
      return View(_db.Authors.ToList());
    }

    [Authorize]
    public ActionResult Create()
    {
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author, int BookId)
    {
      _db.Authors.Add(author);
      if (BookId !=0)
      {
        _db.BookAuthor.Add(new BookAuthor(){AuthorId=author.AuthorId, BookId=BookId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisAuthor = _db.Authors 
        .Include(author => author.Books)
        .ThenInclude(join => join.Book)
        .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [Authorize]
    public ActionResult Edit (int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult Edit(Author author, int BookId)
    {
      _db.Entry(author).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", "Authors", new { id = author.AuthorId });
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult AddBook(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult AddBook(Author author, int BookId)
    {
      if (BookId !=0)
      {
        var returnedJoin = _db.BookAuthor.Any(join => join.AuthorId == author.AuthorId && join.BookId == BookId);

        if(!returnedJoin)
        {
        _db.BookAuthor.Add(new BookAuthor() { AuthorId = author.AuthorId, BookId = BookId });
        }
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = author.AuthorId});
    }

    [Authorize]
    [HttpPost]
    public ActionResult DeleteBook(int joinId)
    {
      var joinEntry = _db.BookAuthor.FirstOrDefault(entry => entry.BookAuthorId == joinId);
      _db.BookAuthor.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.AuthorId });
    }
  }
}


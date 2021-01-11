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
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public ActionResult Index()
    {
      // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      // var currentUser = await _usedManger.FindByAsync(userId);
      // var userBooks = _db.Books.Where(entry => entry.User.Id == currentUser.Id).ToList();
      List<Book> model = _db.Books.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList((from s in _db.Authors select new { AuthorId = s.AuthorId, FullName = s.AuthorFirstName + " " + s.AuthorMiddleInitialName + " " + s.AuthorLastName }), "AuthorId", "FullName", null);
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book book, int AuthorId)
    {
      // var userId = this._userManager.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      // var currentUser = await _userManager.FindByIdAsync(userId);
      _db.Books.Add(book);
      if (AuthorId != 0)
      {
        _db.BookAuthor.Add(new BookAuthor() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisBook = _db.Books 
        .Include(book => book.Authors)
        .ThenInclude(join => join.Author)
        .FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    public ActionResult Edit (int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList((from s in _db.Authors select new { AuthorId = s.AuthorId, FullName = s.AuthorFirstName + " " + s.AuthorMiddleInitialName + " " + s.AuthorLastName }), "AuthorId", "FullName", null);  
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.BookAuthor.Add(new BookAuthor() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = book.BookId });
    }
    public ActionResult Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}


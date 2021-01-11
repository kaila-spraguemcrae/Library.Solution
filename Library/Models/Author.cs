using System.Collections.Generic;

namespace Library.Models
{
  public class Author
  {
    public Author()
    {
      this.Books = new HashSet<BookAuthor>();
    }

    public int AuthorId { get; set; }
    public string AuthorFirstName { get; set; }
    public string AuthorMiddleInitialName { get; set; }
    public string AuthorLastName { get; set; }
    public virtual ApplicationUser User { get; set; } 
    public virtual ICollection<BookAuthor> Books { get; set; }
  }
}
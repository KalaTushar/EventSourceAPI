using Microservice_Q.DAL;
using Microservice_Q.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Microservice_Q.Services
{
    public class BookServices : IBookServices
    {
        private readonly BookContext _context;
        public BookServices(BookContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<BookViewModel>> GetAllAsync()
        {
            return await _context.Books.Select(x => new BookViewModel
            {
                BookId= x.BookId,
                BookAuthor= x.BookAuthor,
                BookTitle= x.BookTitle,
                BookPages= x.BookPages,
            }).ToListAsync();
        }
        public async Task<BookViewModel> GetByIdAsync(int id)
        {
            if(!await _context.Books.AnyAsync(x => x.BookId == id))
            {
                throw new Exception("Not Found");
            }
            var t=await _context.Books.FirstOrDefaultAsync(x => x.BookId == id);
            return new BookViewModel
            {
                BookId=t.BookId,
                BookAuthor=t.BookAuthor,
                BookTitle=t.BookTitle,
                BookPages=t.BookPages,
            };
        }
    }
}

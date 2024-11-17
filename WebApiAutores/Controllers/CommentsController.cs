using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books/{bookId:int}/comments")]
    [ApiController]
    public class CommentsController(ApplicationDBContext context, IMapper mapper) : ControllerBase
    {
        private async Task<bool> BookExists(int bookId) => await context.Books.AnyAsync(b => b.Id == bookId);

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            if (!await BookExists(bookId)) return NotFound($"Book with ID {bookId} not found");

            var comments = await context.Comments.Where(c => c.BookId == bookId).ToListAsync();

            return Ok(mapper.Map<List<CommentDTO>>(comments));
        }

        [HttpGet("{id:int}", Name = "getComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int bookId, int id)
        {
            if (!await BookExists(bookId)) return NotFound($"Book with ID {bookId} not found.");

            var comment = await context.Comments
                .Where(c => c.BookId == bookId)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null) return NotFound($"Comment with ID {id} not found for Book ID {bookId}");

            return Ok(mapper.Map<CommentDTO>(comment));
        }

        [HttpPost]
        public async Task<ActionResult<CommentDTO>> Post(int bookId, CreateCommentDTO createCommentDTO)
        {
            if (!await BookExists(bookId)) return NotFound($"Book with ID {bookId} not found");

            var comment = mapper.Map<Comment>(createCommentDTO);

            comment.BookId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("getComment", new { bookId, id = comment.Id }, commentDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CreateCommentDTO createCommentDTO)
        {
            if (!await BookExists(bookId)) return NotFound($"Book with ID {bookId} not found");

            var existingComment = await context.Comments
                .Where(c => c.BookId == bookId)
                .AnyAsync(c => c.Id == id);

            if (!existingComment) return NotFound($"Comment with ID {id} not found for Book ID {bookId}");

            var comment = mapper.Map<Comment>(createCommentDTO);
            comment.Id = id;
            comment.BookId = id;

            context.Update(comment);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

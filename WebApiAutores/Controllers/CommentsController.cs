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
        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            var existsBook = await context.Books.AnyAsync(b => b.Id == bookId);
            if (!existsBook) return NotFound();

            var comments = await context.Comments.Where(c => c.BookId == bookId).ToListAsync();

            return Ok(mapper.Map<List<CommentDTO>>(comments));
        }

        [HttpPost]
        public async Task<ActionResult> Post(int bookId, CreateCommentDTO commentDTO)
        {
            var existsBook = await context.Books.AnyAsync(b => b.Id == bookId);
            if (!existsBook) return NotFound();

            var comment = mapper.Map<Comment>(commentDTO);

            comment.BookId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();

            return Created();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books/{bookId:int}/comments")]
    [ApiController]
    public class CommentsController(ApplicationDBContext context, IMapper mapper,
        UserManager<IdentityUser> userManager) : ControllerBase
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CommentDTO>> Post(int bookId, CreateCommentDTO createCommentDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var user = await userManager.FindByEmailAsync(email);
            var userId = user?.Id;
            if (userId == null) return Unauthorized();


            if (!await BookExists(bookId)) return NotFound($"Book with ID {bookId} not found");

            var comment = mapper.Map<Comment>(createCommentDTO);

            comment.BookId = bookId;
            comment.UserId = userId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("getComment", new { bookId, id = comment.Id }, commentDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CreateCommentDTO createCommentDTO)
        {
            if (!await BookExists(bookId)) return NotFound($"Book with ID {bookId} not found");

            var commentExists = await context.Comments
                .Where(c => c.BookId == bookId)
                .AnyAsync(c => c.Id == id);

            if (!commentExists) return NotFound($"Comment with ID {id} not found for Book ID {bookId}");

            var comment = mapper.Map<Comment>(createCommentDTO);
            comment.Id = id;
            comment.BookId = id;

            context.Update(comment);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

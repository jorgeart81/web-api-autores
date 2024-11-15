using System;
using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Utilities;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<CreateAuthorDTO, Author>();
    CreateMap<Author, AuthorDTO>();
    CreateMap<CreateBookDTO, Book>()
      .ForMember(book => book.AuthorsBooks, options => options.MapFrom(MapAuthorsBooks));
    CreateMap<Book, BookDTO>();
    CreateMap<CreateCommentDTO, Comment>();
    CreateMap<Comment, CommentDTO>();
  }

  private List<AuthorBook> MapAuthorsBooks(CreateBookDTO bookDTO, Book book)
  {
    var result = new List<AuthorBook>();

    if (bookDTO.AuthorsId == null) return result;

    foreach (var authorId in bookDTO.AuthorsId)
    {
      result.Add(new AuthorBook() { AuthorId = authorId });
    }

    return result;
  }
}

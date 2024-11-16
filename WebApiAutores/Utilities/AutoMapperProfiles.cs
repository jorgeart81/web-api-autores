using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Utilities;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<CreateAuthorDTO, Author>();
    CreateMap<Author, AuthorDTO>();
    CreateMap<Author, AuthorDTOWithBooks>()
      .ForMember(authorDTO => authorDTO.Books, options => options.MapFrom(MapAuthorDTOBooks));

    CreateMap<CreateBookDTO, Book>()
      .ForMember(book => book.AuthorsBooks, options => options.MapFrom(MapAuthorsBooks));

    CreateMap<Book, BookDTO>();
    CreateMap<Book, BookDTOWithAuthors>()
      .ForMember(bookDTO => bookDTO.Authors, options => options.MapFrom(MapBookDTOAuthors));

    CreateMap<CreateCommentDTO, Comment>();
    CreateMap<Comment, CommentDTO>();
  }

  private List<BookDTO> MapAuthorDTOBooks(Author author, AuthorDTO authorDTO)
  {
    var result = new List<BookDTO>();

    if (author.AuthorsBooks == null) return result;

    foreach (var authorBook in author.AuthorsBooks)
    {
      if (authorBook.Book == null) continue;

      result.Add(new BookDTO()
      {
        Id = authorBook.BookId,
        Title = authorBook.Book.Title
      });
    }

    return result;
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

  private List<AuthorDTO> MapBookDTOAuthors(Book book, BookDTO bookDTO)
  {
    var result = new List<AuthorDTO>();

    if (book.AuthorsBooks == null) return result;

    foreach (var authorBook in book.AuthorsBooks)
    {
      if (authorBook.Author == null) continue;

      result.Add(new AuthorDTO()
      {
        Id = authorBook.AuthorId,
        Name = authorBook.Author.Name,
      });

    }

    return result;
  }


}


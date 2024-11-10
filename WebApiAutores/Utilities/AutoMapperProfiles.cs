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
    CreateMap<CreateBookDTO, Book>();
    CreateMap<Book, BookDTO>();
    CreateMap<CreateCommentDTO, Comment>();
    CreateMap<Comment, CommentDTO>();
  }
}

using System;

namespace WebApiAutores.DTOs;

public class PaginationDTO
{
  private int recordsPerPage = 10;
  private readonly int maxPerPage = 50;
  public int Page { get; set; } = 1;

  public int RecordsPerPage
  {
    get { return recordsPerPage; }
    set { recordsPerPage = (value > maxPerPage) ? maxPerPage : value; }
  }

}

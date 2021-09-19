﻿using DAL.Model;

namespace BLL.DTO
{
    public class GenreDTO
    {
        public GenreDTO()
        { }

        public GenreDTO(Genre genre)
        {
            Id = genre.Id;
            Value = genre.Value;
        }

        public int Id { get; set; }

        public string Value { get; set; }
    }
}

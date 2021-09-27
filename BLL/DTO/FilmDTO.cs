﻿using System.Collections.Generic;
using System.Linq;
using DAL.Model;

namespace BLL.DTO
{
    public class FilmDTO
    {
        public FilmDTO() { }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string Description { get; set; }

        public ICollection<CommentDTO> Comments { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }

        public ICollection<UserDTO> Users { get; set; }
    }
}

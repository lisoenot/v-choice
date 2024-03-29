﻿using System;
using System.Collections.Generic;

namespace DAL.Model
{
    public class Film
    {
        public Film()
        {
            Comments = new HashSet<Comment>();
            InFavorites = new HashSet<Favorite>();
            Genres = new HashSet<Genre>();
            RateCollection = new HashSet<Rate>();
            Persons = new HashSet<Participation>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public int TotalRate { get; set; }
        public float AverageRate { get; set; }
        public int CountRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PosterPath { get; set; }
        public int Requested { get; set; }
        public string VideoToken { get; set; }
        public int? StudioId { get; set; }
        public Studio Studio { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> InFavorites { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Rate> RateCollection { get; set; }
        public virtual ICollection<Participation> Persons { get; set; }
    }
}

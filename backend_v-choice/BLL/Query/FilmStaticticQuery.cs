﻿using BLL.Enum;

namespace BLL.Query
{
    public class FilmStaticticQuery : PaginationQueryBase
    {
        public FilmStatisticSortingType SortingType { get; set; }
    }
}

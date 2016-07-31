﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iomer.Extensions.Pagination
{

    public struct PaginationModel
    {
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
        public bool ValidCurrentPage { get; set; }
        public bool ValidPageSize { get; set; }
        public int IndexStart { get; set; }
        public int IndexEnd { get; set; }
    }
}

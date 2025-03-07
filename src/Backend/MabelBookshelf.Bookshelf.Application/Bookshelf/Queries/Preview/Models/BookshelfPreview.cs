﻿using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models
{
    public record BookshelfPreview(string Id, string Name, string OwnerId, List<BookPreview> Books);
}
using System;
using System.Collections.Generic;

namespace ReelReviewAPI.Models;

public partial class Movie
{
    public long MovieId { get; set; }

    public long? DirectorId { get; set; }

    public string? Title { get; set; }

    public long? ReleaseYear { get; set; }

    public string? Rating { get; set; }

    public string? Plot { get; set; }

    public byte[]? MovieLength { get; set; }

    public virtual Director? Director { get; set; }

   
}

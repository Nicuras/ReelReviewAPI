using System;
using System.Collections.Generic;

namespace ReelReviewAPI.Models;

public partial class Director
{
    public long DirectorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Nationality { get; set; }

    public string? Birth { get; set; }

}

using System;
using System.Collections.Generic;

namespace ReelReviewAPI.Models;

public partial class MovieActor
{
    public long MovieActorId { get; set; }

    public long MovieId { get; set; }

    public long ActorId { get; set; }

    public virtual Actor Actor { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}

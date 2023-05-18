namespace ReelReviewAPI.Models
{
    public class ReelReview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title{ get; set; }
        public string Review { get; set; } 
        public int Rating { get; set; }
        public object Movies { get; internal set; }

        internal object Entry(Movie movie)
        {
            throw new NotImplementedException();
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

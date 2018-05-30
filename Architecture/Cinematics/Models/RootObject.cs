namespace Cinematics
{
    public class RootObject
    {
        public int Page { get; set; }
        public int Total_results { get; set; }
        public int Total_pages { get; set; }
        public Movie[] Results { get; set; }
    }
}

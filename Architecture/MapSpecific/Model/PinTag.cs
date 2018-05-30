namespace MapSpecific.Model
{
    public class PinTag
    {
        public PinTag(string id, string position)
        {
            Id = id;
            Position = position;
        }

        public string Id { get; set; }
        public string Position { get; set; }
    }
}

using SQLite;
using System;

namespace Architecture.Core
{
    public class Note
    {
        public Note()
        {
            Created = DateTime.Now;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        [NotNull, MaxLength(255)]
        public string Text { get; set; }
        public DateTime Created{ get; set; }
        public DateTime LastUpdate { get; set; }
    }
}

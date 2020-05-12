using MiniMdb.Backend.Shared;

namespace MiniMdb.Backend.Models
{
    public abstract class MediaTitle
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public MediaTitleType Type { get; }

        public long ReleaseDate { get; set; }

        public string Plot { get; set; }

        public long AddedAt { get; set; }

        public long UpdatedAt { get; set; }
    }
}

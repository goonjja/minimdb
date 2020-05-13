using MiniMdb.Backend.Data;
using MiniMdb.Backend.Models;

namespace MiniMdb.Testing.Util
{
    class DbSeed
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Movies.Add(new Movie { Name = "The Platform", Plot = "A vertical prison with one cell per level. Two people per cell. One only food platform and two minutes per day to feed from up to down. An endless nightmare trapped in The Hole." });
            db.Movies.Add(new Movie { Name = "Joker ", Plot = "In Gotham City, mentally troubled comedian Arthur Fleck is disregarded and mistreated by society. He then embarks on a downward spiral of revolution and bloody crime. This path brings him face-to-face with his alter-ego: the Joker." });
            db.Movies.Add(new Movie { Name = "Ex Machina", Plot = "A young programmer is selected to participate in a ground-breaking experiment in synthetic intelligence by evaluating the human qualities of a highly advanced humanoid A.I." });
            db.Movies.Add(new Movie { Name = "Blade Runner 2049", Plot = "Young Blade Runner K's discovery of a long-buried secret leads him to track down former Blade Runner Rick Deckard, who's been missing for thirty years." });
            db.Movies.Add(new Movie { Name = "Interstellar", Plot = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival." });
            db.Movies.Add(new Movie { Name = "Inception", Plot = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O." });

            db.Series.Add(new Series { Name = "Solar Opposites", Plot = "A family of aliens move to middle America, where they debate whether life is better there or on their home planet." });
            db.Series.Add(new Series { Name = "Rick and Morty ", Plot = "An animated series that follows the exploits of a super scientist and his not-so-bright grandson." });
            db.Series.Add(new Series { Name = "Westworld ", Plot = "Set at the intersection of the near future and the reimagined past, explore a world in which every human appetite can be indulged without consequence." });
            db.Series.Add(new Series { Name = "Black Mirror", Plot = "An anthology series exploring a twisted, high-tech multiverse where humanity's greatest innovations and darkest instincts collide." });
            db.Series.Add(new Series { Name = "Stranger Things", Plot = "When a young boy disappears, his mother, a police chief and his friends must confront terrifying supernatural forces in order to get him back." });

            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            foreach (var title in db.Titles)
                db.Remove(title);
            InitializeDbForTests(db);
        }
    }
}

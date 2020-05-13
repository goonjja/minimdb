namespace MiniMdb.Backend.Helpers
{
    public static class CacheKeys
    {
        public static string MediaTitle(long id) => $"MediaTitle_{id}";
    }
}

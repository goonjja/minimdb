using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MiniMdb.Backend
{
    /// <summary>
    /// Custom environments and convenient extension methods
    /// </summary>
    public static class Environments
    {
        public const string Staging1 = "Staging1";
        public const string Staging2 = "Staging2";

        public static bool IsStaging1(this IWebHostEnvironment env) => env.IsEnvironment(Staging1);
        public static bool IsStaging2(this IWebHostEnvironment env) => env.IsEnvironment(Staging2);
    }
}

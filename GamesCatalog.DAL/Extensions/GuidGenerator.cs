using System.Runtime.CompilerServices;

namespace GamesCatalog.DAL.Extensions
{
    internal static class GuidGenerator
    {
        [SkipLocalsInit]
        public static unsafe Guid GenerateNotCryptoQualityGuid()
        {
            var bytes = stackalloc byte[16];
            Random.Shared.NextBytes(new Span<byte>(bytes, 16));
            return *(Guid*)bytes;
        }
    }
}

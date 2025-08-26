using System.Security.Cryptography;

namespace RandomNumbersApi.Services;

public class RandomService : IRandomService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public int NextIntNonNegative() => Random.Shared.Next();

    public int NextIntInclusive(int min, int max)
    {
        if (min > max) throw new ArgumentException("min must be <= max");
        long lmin = min;
        long lmaxExclusive = (long)max + 1L;
        long value = Random.Shared.NextInt64(lmin, lmaxExclusive);
        return (int)value;
    }

    public double NextUnitIntervalDouble() => Random.Shared.NextDouble(); // [0,1)

    public string NextString(int length)
    {
        if (length < 1 || length > 1024)
            throw new ArgumentOutOfRangeException(nameof(length), "length must be 1..1024");

        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            int index = RandomNumberGenerator.GetInt32(Alphabet.Length);
            chars[i] = Alphabet[index];
        }
        return new string(chars);
    }
}
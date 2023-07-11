using System;
using System.Linq;

namespace MicroService.Example.Tests.Helpers;

public static class ValueHelper
{
    public static string RandomString(int length = 5)
    {
        if (length == 0) return string.Empty;

        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz";

        return new string(Enumerable
            .Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
    public static int RandomInt32(int lowerBound = default, int upperBound = int.MaxValue) => (new Random()).Next(lowerBound, upperBound);
    public static Guid RandomGuid() => Guid.NewGuid();
    public static string RandomGuidString() => Guid.NewGuid().ToString();
}
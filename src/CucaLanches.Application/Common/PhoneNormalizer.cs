using System.Text;

namespace CucaLanches.Application.Common;

public static class PhoneNormalizer
{
    public static string Normalize(string phone)
    {
        var builder = new StringBuilder();

        foreach (var c in phone)
        {
            if (char.IsDigit(c))
            {
                builder.Append(c);
            }
        }
        
        return builder.ToString();
    }
}
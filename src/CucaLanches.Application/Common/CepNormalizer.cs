using System.Text;

namespace CucaLanches.Application.Common;

public static class CepNormalizer
{

    public static string Normalize(string cep)
    {
        var builder = new StringBuilder();

        foreach (char c in cep)
        {
            if (char.IsDigit(c))
            {
                builder.Append(c);
            }
        }
        
        return builder.ToString();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Utils;
/// <summary>
/// Obecné funkce pro práci s řetězci
/// </summary>
public static class StringUtils
{
    public static string JoinPath(string actual, string addValue, string delimeter)
    {
       return String.Join(delimeter,(actual??string.Empty).Split(delimeter).Append(addValue));
    }
}

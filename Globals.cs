using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsalAuthInMauiBlazor
{
    internal static class Globals
    {
        public static string? AccessToken = null;

        public static bool IsLoading {  get; set; }


        public static IEnumerable<string>? GrantedScopes = null;

        public static IAccount? Account = null;

        public static DateTimeOffset? Expires;
    }
}

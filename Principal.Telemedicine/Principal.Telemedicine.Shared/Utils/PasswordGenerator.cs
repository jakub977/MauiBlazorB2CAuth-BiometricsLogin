namespace Principal.Telemedicine.Shared.Utils
{
    /// <summary>
    /// Jednoduchá generátor hesla
    /// </summary>
    public static class PasswordGenerator
    {
        private static string letters = "abcdefghijklmnopqrstuvwxyz";
        private static string lettersUp = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
        private static string numbers = "0123456789";
        private static string special = "!@#$?_-";

        /// <summary>
        /// Varcí počet znaků z vybraného řetězce
        /// </summary>
        /// <param name="length">Požadovaná délka</param>
        /// <param name="data">Znaky, ze kterých se náhodně vybírá</param>
        /// <returns></returns>
        private static string Generate(int length, string data)
        {
            string ret = "";
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random();
                ret += data[rand.Next(0, data.Length - 1)];
            }
            return ret;
        }

        /// <summary>
        /// Vygeneruje heslo
        /// </summary>
        /// <returns></returns>
        public static string GetNewPassword()
        {
            string ret = "";

            ret += Generate(2, letters);
            ret += Generate(2, lettersUp);
            ret += Generate(1, numbers);
            ret += Generate(1, special);
            ret += Generate(1, lettersUp);
            ret += Generate(2, numbers);
            ret += Generate(3, letters);

            return ret;
        }
    }
}

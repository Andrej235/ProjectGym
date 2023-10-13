namespace ProjectGym.Services
{
    public static class StringExtensions
    {
        public static bool IsSimilar_Levenshtein(this string str1, string str2)
        {
            int len1 = str1.Length;
            int len2 = str2.Length;
            int[,] matrix = new int[len1 + 1, len2 + 1];

            for (int i = 0; i <= len1; i++)
                matrix[i, 0] = i;
            for (int j = 0; j <= len2; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            int res = matrix[len1, len2];
            return res < Math.Clamp(Math.Min(str1.Length * 0.85, str2.Length * 0.85), 1, 7);
        }

        public static bool IsSimilar(this string str1, string str2)
        {
            if (str1.Contains(str2, StringComparison.OrdinalIgnoreCase) || str2.Contains(str1, StringComparison.OrdinalIgnoreCase))
                return true;

            int similarCount = 0;
            var words1 = str1.ToLower().Split(' ').Select(x => x.Trim());
            var words2 = str2.ToLower().Split(' ').Select(x => x.Trim());

            if (!words1.Any())
                return false;

            if (!words2.Any())
                return true;

            foreach (var word1 in words1)
            {
                foreach (var word2 in words2)
                {
                    if (word1.Contains(word2) || word2.Contains(word1))
                        similarCount++;

/*                    if (word1.IsSimilar_Levenshtein(word2))
                        similarCount++;*/
                }
            }

            return similarCount > Math.Min(words1.Count() * 0.75, words2.Count() * 0.75);
        }
    }
}

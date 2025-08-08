using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.TextMethots
{
    public class TextsCheckMethots
    {
        // Girilen her kelimeyi büyük harfle başlayacak şekilde düzenler. Örnek: "ahmet" -> "Ahmet" veya "al yazma" -> "Al Yazma"
        // You can add more text normalization methods here.
        public string NormalizeText(string text)
        {
            var words = text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    var firstLetter = words[i].Substring(0, 1).ToUpperInvariant();
                    var restOfWord = words[i].Substring(1).ToLowerInvariant();
                    words[i] = firstLetter + restOfWord;
                }
            }
            return string.Join(' ', words);
        }
    }
}

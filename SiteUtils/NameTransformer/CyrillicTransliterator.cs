using System.Text;

namespace NameTransformer;

public static class CyrillicTransliterator
{
    // ISO 9 (GOST 7.79 System A) transliteration mapping
    private static readonly Dictionary<char, string> TransliterationMap = new()
    {
        // Uppercase
        {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Д', "D"},
        {'Е', "E"}, {'Ё', "Yo"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "I"},
        {'Й', "J"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
        {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"},
        {'У', "U"}, {'Ф', "F"}, {'Х', "H"}, {'Ц', "Ts"}, {'Ч', "Ch"},
        {'Ш', "Sh"}, {'Щ', "Shh"}, {'Ъ', ""}, {'Ы', "Y"}, {'Ь', ""},
        {'Э', "E"}, {'Ю', "Yu"}, {'Я', "Ya"},
        
        // Lowercase
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
        {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
        {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
        {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
        {'у', "u"}, {'ф', "f"}, {'х', "h"}, {'ц', "ts"}, {'ч', "ch"},
        {'ш', "sh"}, {'щ', "shh"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
        {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
    };

    public static string Transliterate(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Check if text contains Cyrillic characters
        if (!ContainsCyrillic(text))
            return text;

        var result = new StringBuilder();

        foreach (var ch in text)
        {
            if (TransliterationMap.TryGetValue(ch, out var replacement))
            {
                result.Append(replacement);
            }
            else
            {
                result.Append(ch);
            }
        }

        return result.ToString();
    }

    private static bool ContainsCyrillic(string text)
    {
        return text.Any(c => (c >= 0x0400 && c <= 0x04FF) || // Cyrillic
                             (c >= 0x0500 && c <= 0x052F));  // Cyrillic Supplement
    }
}

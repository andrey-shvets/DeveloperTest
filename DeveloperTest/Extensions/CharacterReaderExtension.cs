using DeveloperTestInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTest.Extensions
{
    public static class CharacterReaderExtension
    {
        public static string GetNextWord(this ICharacterReader charReader)
        {
            var chars = new List<char>();

            while (true)
            {
                try
                {
                    var nextChar = charReader.GetNextChar();

                    if (IsPartOfWord(chars.LastOrDefault(), nextChar))
                        chars.Add(nextChar);
                    else if (chars.Any())
                        return new string(chars.ToArray()).Trim('-');
                }
                catch (EndOfStreamException)
                {
                    if (chars.Any())
                        return new string(chars.ToArray()).Trim('-');

                    return null;
                }
            }
        }

        public static async Task<string> GetNextWordAsync(this ICharacterReader charReader, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            return charReader.GetNextWord();
        }

        private static bool IsPartOfWord(char lastchar, char nextChar) =>
            char.IsLetterOrDigit(nextChar) || nextChar == '-' && char.IsLetterOrDigit(lastchar);
    }
}

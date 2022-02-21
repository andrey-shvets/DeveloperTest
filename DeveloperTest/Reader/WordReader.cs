using DeveloperTestInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTest.Reader
{
    public class WordReader : IWordReader
    {
        private ICharacterReader CharReader { get; }
        private bool TextEndReached { get; set; }

        public WordReader(ICharacterReader charReader)
        {
            CharReader = charReader;
        }

        public string GetNextWord()
        {
            var chars = new List<char>();

            while (true)
            {
                if (!TryReadNextChar(out var nextChar))
                {
                    if (chars.Any())
                        return new string(chars.ToArray()).Trim('-').ToLower();
                    else
                        throw new EndOfStreamException();
                }

                if (IsPartOfWord(chars.LastOrDefault(), nextChar))
                    chars.Add(nextChar);
                else
                {
                    if (chars.Any())
                        return new string(chars.ToArray()).Trim('-').ToLower();

                    continue;
                }
            }
        }

        public async Task<string> GetNextWordAsync(CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            return GetNextWord();
        }

        private bool TryReadNextChar(out char nextChar)
        {
            try
            {
                nextChar = CharReader.GetNextChar();
                return true;
            }
            catch (EndOfStreamException)
            {
                if (TextEndReached)
                    throw;

                TextEndReached = true;
                nextChar = (char)0;
                return false;
            }
        }

        private bool IsPartOfWord(char lastchar, char nextChar)
        {
            if (nextChar == '-' && char.IsLetterOrDigit(lastchar))
                return true;

            return char.IsLetterOrDigit(nextChar);
        }
    }
}

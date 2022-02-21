using DeveloperTestInterfaces;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTestFramework.DeveloperTest.Helpers
{
    internal class TestCharacterReader : ICharacterReader
    {
        private string Content { get; }
        private int _currentPosition;

        public TestCharacterReader(string content)
        {
            Content = content;
        }

        public char GetNextChar()
        {
            if (_currentPosition >= Content.Length)
                throw new EndOfStreamException();

            return Content[_currentPosition++];
        }

        public async Task<char> GetNextCharAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            return GetNextChar();
        }

        public void Dispose()
        { }
    }
}

using DeveloperTest.Reader;
using DeveloperTestFramework.DeveloperTest.Helpers;
using NUnit.Framework;
using System.IO;

namespace DeveloperTestFramework.DeveloperTest
{
    [TestFixture]
    public class WordReaderTests
    {
        [TestCase("")]
        [TestCase(" !!@#$%^&*()_+=-{}[]'\"\\|/?.>,<`~--")]
        public void GetNextWord_ThrowsEndOfStreamException_ForTextWithNoWords(string content)
        {
            var characterReader = new TestCharacterReader(content);
            var reader = new WordReader(characterReader);

            Assert.Throws<EndOfStreamException>(() => reader.GetNextWord());
        }

        [TestCase("Word", "word")]
        [TestCase("Word ", "word")]
        [TestCase("  WoRd", "word")]
        [TestCase("word 2ndword", "word")]
        [TestCase("ninja-monkey!", "ninja-monkey")]
        [TestCase("ninja--monkey!", "ninja")]
        [TestCase(" -Word", "word")]
        [TestCase("Word'.", "word")]
        [TestCase("Word-!", "word")]
        public void GetNextWord_ReadsWord_FromReaderWithSingleWord(string content, string expected)
        {
            var characterReader = new TestCharacterReader(content);
            var reader = new WordReader(characterReader);

            var word = reader.GetNextWord();

            Assert.That(word, Is.EqualTo(expected));
        }

        [TestCase("first second third.", "second")]
        [TestCase("first--second", "second")]
        [TestCase("first --second", "second")]
        [TestCase("first - second", "second")]
        [TestCase("first,!? (second)", "second")]
        [TestCase("first 'second'", "second")]
        public void GetNextWord_ReadsSecondWord_FromTextWithMultipleWords(string content, string expected)
        {
            var characterReader = new TestCharacterReader(content);
            var reader = new WordReader(characterReader);

            _ = reader.GetNextWord();
            var word = reader.GetNextWord();

            Assert.That(word, Is.EqualTo(expected));
        }

        [Test]
        public void GetNextWord_ThrowsEndOfStreamException_AfterLastWordRead()
        {
            var content = "Word !";
            var characterReader = new TestCharacterReader(content);
            var reader = new WordReader(characterReader);

            _ = reader.GetNextWord();
            Assert.Throws<EndOfStreamException>(() => reader.GetNextWord());
        }
    }
}

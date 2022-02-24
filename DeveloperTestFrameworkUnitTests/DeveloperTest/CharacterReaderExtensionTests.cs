using DeveloperTest.Extensions;
using DeveloperTestFramework.DeveloperTest.Helpers;
using NUnit.Framework;

namespace DeveloperTestFramework.DeveloperTest
{
    [TestFixture]
    public class CharacterReaderExtensionTests
    {
        [TestCase("")]
        [TestCase(" !!@#$%^&*()_+=-{}[]'\"\\|/?.>,<`~--")]
        public void GetNextWord_ReturnsNull_ForTextWithNoWords(string content)
        {
            var reader = new TestCharacterReader(content);

            var word = reader.GetNextWord();

            Assert.That(word, Is.Null);
        }

        [TestCase("Word", "Word")]
        [TestCase("Word ", "Word")]
        [TestCase("  Word", "Word")]
        [TestCase("word 2ndword", "word")]
        [TestCase("ninja-monkey!", "ninja-monkey")]
        [TestCase(" ninja--monkey!", "ninja")]
        [TestCase("-Word", "Word")]
        [TestCase("Word'.", "Word")]
        [TestCase("Word-!", "Word")]
        public void GetNextWord_ReadsWord_FromReaderWithSingleWord(string content, string expected)
        {
            var reader = new TestCharacterReader(content);

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
            var reader = new TestCharacterReader(content);

            _ = reader.GetNextWord();
            var word = reader.GetNextWord();

            Assert.That(word, Is.EqualTo(expected));
        }

        [Test]
        public void GetNextWord_ReturnsNull_AfterLastWordRead()
        {
            var content = "Word !";
            var reader = new TestCharacterReader(content);

            _ = reader.GetNextWord();
            var word = reader.GetNextWord();

            Assert.That(word, Is.Null);
        }
    }
}

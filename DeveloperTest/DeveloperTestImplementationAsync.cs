#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion

using DeveloperTest.Reader;
using DeveloperTestInterfaces;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTest
{
    public sealed class DeveloperTestImplementationAsync : IDeveloperTestAsync
    {
        public async Task RunQuestionOne(ICharacterReader reader, IOutputResult output, CancellationToken cancellationToken)
        {
            var wordReader = new WordReader(reader);
            var countedWords = new ConcurrentDictionary<string, int>();

            try
            {
                while (true)
                {
                    var nextWord = await wordReader.GetNextWordAsync();
                    countedWords.AddOrUpdate(nextWord, 1, (_, v) => v + 1);
                }
            }
            catch (EndOfStreamException)
            { }

            var sortedWords = countedWords.OrderByDescending(w => w.Value).ThenBy(w => w.Key).Select(w => $"{w.Key} - {w.Value}").ToHashSet();

            foreach (var word in sortedWords)
                output.AddResult(word);
        }

        public async Task RunQuestionTwo(ICharacterReader[] readers, IOutputResult output, CancellationToken cancellationToken)
        {
            var countedWords = new ConcurrentDictionary<string, int>();

            foreach (var reader in readers)
            {
                var wordReader = new WordReader(reader);

                try
                {
                    while (true)
                    {
                        var nextWord = await wordReader.GetNextWordAsync();
                        countedWords.AddOrUpdate(nextWord, 1, (_, v) => v + 1);
                    }
                }
                catch (EndOfStreamException)
                { }
            }

            var sortedWords = countedWords.OrderByDescending(w => w.Value).ThenBy(w => w.Key).Select(w => $"{w.Key} - {w.Value}").ToHashSet();

            foreach (var word in sortedWords)
                output.AddResult(word);
        }
    }
}
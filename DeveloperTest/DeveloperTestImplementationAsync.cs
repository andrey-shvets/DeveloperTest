#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion

using DeveloperTest.Extensions;
using DeveloperTestInterfaces;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTest
{
    public sealed class DeveloperTestImplementationAsync : IDeveloperTestAsync
    {
        public async Task RunQuestionOne(ICharacterReader reader, IOutputResult output, CancellationToken cancellationToken)
        {
            var countedWords = new ConcurrentDictionary<string, int>();

            await CountWordsFromReader(countedWords, reader, cancellationToken);
            var sortedWords = countedWords.OrderByDescending(w => w.Value).ThenBy(w => w.Key).Select(w => $"{w.Key} - {w.Value}").ToHashSet();

            foreach (var word in sortedWords)
                output.AddResult(word);
        }

        public async Task RunQuestionTwo(ICharacterReader[] readers, IOutputResult output, CancellationToken cancellationToken)
        {
            var countedWords = new ConcurrentDictionary<string, int>();

            var wordsCountingTasks = readers.Select(r => CountWordsFromReader(countedWords, r, cancellationToken)).ToArray();
            await Task.WhenAll(wordsCountingTasks);

            var sortedWords = countedWords.OrderByDescending(w => w.Value).ThenBy(w => w.Key).Select(w => $"{w.Key} - {w.Value}").ToHashSet();

            foreach (var word in sortedWords)
                output.AddResult(word);
        }

        private static async Task CountWordsFromReader(ConcurrentDictionary<string, int> countedWords, ICharacterReader reader, CancellationToken cancellationToken)
        {
            while (true)
            {
                var nextWord = await reader.GetNextWordAsync(cancellationToken);

                if (nextWord is null)
                    break;

                countedWords.AddOrUpdate(nextWord.ToLower(), 1, (_, v) => v + 1);
            }
        }
    }
}
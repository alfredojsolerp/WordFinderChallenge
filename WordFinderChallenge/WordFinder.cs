namespace WordFinderChallenge
{
	internal class WordFinder
	{
		private List<string> _matrix;

		public WordFinder(IEnumerable<string> matrix)
		{
			_matrix = new List<string>(matrix);
		}

		public IEnumerable<string> Find(IEnumerable<string> wordstream)
		{
			// Let's throw some multitask work to get job done faster.
			var searchingTasks = new List<Task<IEnumerable<string>>>
			{
				SearchHorizontally(wordstream),
				SearchVertically(wordstream)
			};

			// Run tasks and capture results.
			// We can't change the firm of the method, so we need to use 'Result' instead of 'await'.
			// That is dangerous since it blocks the main thread, but is blocking the UI a concern? For this challenge, no.
			var searchResults = Task.WhenAll(searchingTasks).Result;			

			// Results are no unique here, so we need to filter the top 10 most repeated words.
			// Linq is good for stuff like this.
			var uniqueResults = searchResults
				.SelectMany(ienumerable => ienumerable) // The Task.WhenAll returns an array of IEnumerables, so lets flatten that first
				.GroupBy(word => word)
				.OrderByDescending(group => group.Count())
				.Take(10)
				.Select(group => group.Key);

			return uniqueResults ?? [];
		}

		// Method to search words from left to right.
		private async Task<IEnumerable<string>> SearchHorizontally(IEnumerable<string> wordstream)
		{
			var foundWords = new List<string>();

			// Run parallel searches for each row to maximize the speed.
			await Task.WhenAll(_matrix.Select(async row =>
			{
				foreach (var word in wordstream)
				{
					if (row.Contains(word))
					{
						// Avoid collisioning between threads by locking the list.
						lock (foundWords)
						{
							foundWords.Add(word);
						}
					}
				}
			}));

			return foundWords;
		}

		// Method to search words from top to bottom.
		private async Task<IEnumerable<string>> SearchVertically(IEnumerable<string> wordstream)
		{
			var foundWords = new List<string>();

			// For vertical search we need to first create the columns.
			// We can assume that all words have the same length because the exercise told us.
			var columns = new List<string>();

			// A column is created selecting the character in the X position of the N rows.
			for (int x = 0; x < _matrix.First().Length; x++)
			{
				columns.Add(new string(_matrix.Select(row => row[x]).ToArray()));
			}

			// Run parallel searches for each column to maximize the speed.
			await Task.WhenAll(columns.Select(async column =>
			{
				foreach (var word in wordstream)
				{
					if (column.Contains(word))
					{
						// Avoid collisioning between threads by locking the list.
						lock (foundWords)
						{
							foundWords.Add(word);
						}
					}
				}
			}));

			return foundWords;
		}
	}
}

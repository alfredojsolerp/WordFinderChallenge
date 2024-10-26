/*
 *	My reasoning was as follows:

	The exercise was all about an alphabet soup, and this implies one thing: you have to do a linear search. There's no way to escape from that. You have to search for words 
	vertically or horizontally. So how can you do this quickly? Well, a better question would be: do I have to search row by row and column by column? Or can I search all rows and columns at the
	same time? Once I asked myself the question the answer came naturally to my head: asynchronous code.

	And that was it, that was my reasoning :D
 */

// 1. Usings.
using System.Diagnostics;
using WordFinderChallenge;

// 2. Declarations.
IEnumerable<string> wordstream = [];    // Insert your wordstream here.
IEnumerable<string> matrix = [];		// Insert your matrix here.
var timer = new Stopwatch();            // Timer to evaluate which approximation is the most efficient (List, HashSet, with async, without async, etc).

// 3. Main program.
timer.Start();
wordstream = ["toast", "yellow", "backend", "frontend", "engineer", "keyboard", "programmer", "friday", "amen", "mortal"];
matrix = ["agmqexeetpj", "frontendorh", "rbriergyaok", "istcrjizsgb", "dtaandnktra", "aulbmfeemac", "yellowegfmk", "nvchzmrlbme", "wodihinamen", "keyboardprd"];
var wordFinder = new WordFinder(matrix);

var result = wordFinder.Find(wordstream);
timer.Stop();

// 4. Show the results.
Console.WriteLine($"Top 10 results");
Console.WriteLine("------------------------------");

foreach (var word in result)
{
	Console.WriteLine($"> {word}.");
}

Console.WriteLine("------------------------------");
Console.WriteLine($"> Matrix count: {matrix.Count()}");
// I'm wondering if the block of the main thread affects the timer.
Console.WriteLine($"> Total milliseconds: {timer.Elapsed.TotalMilliseconds}");
Console.WriteLine("------------------------------");
Console.WriteLine("I hope you find all that you are searching for in this code :)");

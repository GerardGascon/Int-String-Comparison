using System.Diagnostics;
using System.Text;
using TextCopy;

namespace IntStringComparison;

internal abstract class Program {
	//Limit the number generated to avoid it being too big
	private const int MaxNumbersToTest = 100_000_000;
	private static void Main() {
		DateTime start = DateTime.Now;
		Console.WriteLine("### Overall Start Time: " + start.ToLongTimeString());
		Console.WriteLine();
		RunTest();
		DateTime end = DateTime.Now;
		Console.WriteLine();
		Console.WriteLine("### Overall End Time: " + end.ToLongTimeString());
		Console.WriteLine("### Overall Run Time: " + (end - start));
		Console.WriteLine();
		Console.WriteLine("Hit Enter to Exit");
		Console.ReadLine();
	}

	private static void RunTest() {
		const int testRuns = 1000;

		TestResult[] results = new TestResult[testRuns];

		Random rand = new();
		for (int i = 0; i < testRuns; i++) {
			Console.WriteLine($"Test Run no. {i + 1} out of {testRuns}");
			MeasureDifference(rand.Next(0, MaxNumbersToTest), out results[i]);
			GC.Collect();
		}

		Console.WriteLine();
		Console.WriteLine("###########################################################");
		Console.WriteLine("######## Test Results");
		StringBuilder resultBuilder = new("iAdd\tsAdd\tiRead\tsRead\tsize\n");
		Console.WriteLine("   Int Add   |  String Add  |   Int Read   |  String Read |   Size");
		foreach (TestResult result in results) {
			resultBuilder.AppendLine(result.ToString());
			Console.WriteLine(result.ToString());
		}
		ClipboardService.SetText(resultBuilder.ToString());
		Console.WriteLine();
		Console.WriteLine("Result CSV copied to clipboard");
	}

	private static void MeasureDifference(int numberOfNumbersToTest, out TestResult result) {
		Console.WriteLine($"######## {nameof(MeasureDifference)}");
		Console.WriteLine($"Number of values to convert: {numberOfNumbersToTest:#,##0}");

		Stopwatch sw = new();

		string[] s = new string[numberOfNumbersToTest];
		int[] i = new int[numberOfNumbersToTest];

		Dictionary<string, int> ds = new(numberOfNumbersToTest);
		Dictionary<int, int> di = new(numberOfNumbersToTest);

		Console.WriteLine("Initializing int items array...");
		for (int x = 0; x < numberOfNumbersToTest; x++) {
			i[x] = x;
		}
		Console.WriteLine("Int items array initialized.");
		Thread.Sleep(250);
		Console.WriteLine("Shuffling items...");
		Random rand = new();
		rand.Shuffle(i);
		Console.WriteLine("Items shuffled.");
		Thread.Sleep(250);
		Console.WriteLine("Converting integers to strings...");
		for (int x = 0; x < numberOfNumbersToTest; x++) {
			s[x] = i[x].ToString();
		}
		Console.WriteLine("Integers converted.");
		Console.WriteLine();
		Thread.Sleep(500);

		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting adding int-key to dictionary test " + numberOfNumbersToTest.ToString("#,##0") +
		                  " at: " + DateTime.Now.ToLongTimeString());
		sw.Restart();
		for (int x1 = 0; x1 < numberOfNumbersToTest; x1++)
			di.Add(i[x1], i[x1]);
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Number of elements: " + di.Count.ToString("#,##0"));
		string addIntElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + addIntElapsed);
		Thread.Sleep(500);

		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting adding stringified-int-key to dictionary test " +
		                  numberOfNumbersToTest.ToString("#,##0") + " at: " + DateTime.Now.ToLongTimeString());
		sw.Restart();
		for (int x2 = 0; x2 < numberOfNumbersToTest; x2++)
			ds.Add(s[x2], i[x2]);
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Number of elements: " + ds.Count.ToString("#,##0"));
		string addStringElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + addStringElapsed);
		Thread.Sleep(500);

		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting reading int-key from dictionary test " +
		                  numberOfNumbersToTest.ToString("#,##0") + " at: " + DateTime.Now.ToLongTimeString());
		long total = 0;
		sw.Restart();
		for (int x3 = 0; x3 < numberOfNumbersToTest; x3++)
			total += di[i[x3]];
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Total: " + total.ToString("#,##0"));
		string readIntElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + readIntElapsed);
		Thread.Sleep(500);

		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting reading stringified-int-key from dictionary test " +
		                  numberOfNumbersToTest.ToString("#,##0") + " at: " + DateTime.Now.ToLongTimeString());
		long total1 = 0;
		sw.Restart();
		for (int x4 = 0; x4 < numberOfNumbersToTest; x4++)
			total1 += ds[s[x4]];
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Total: " + total1.ToString("#,##0"));
		string readStringElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + readStringElapsed);
		Thread.Sleep(500);

		Array.Clear(i);
		Array.Clear(s);
		di.Clear();
		ds.Clear();

		result = new TestResult(addIntElapsed, addStringElapsed, readIntElapsed, readStringElapsed,
			numberOfNumbersToTest.ToString("#,##0"));

		Console.WriteLine("###########################################################");
		Console.WriteLine("Results Summary");
		Console.WriteLine($"Number of dictionary elements: {numberOfNumbersToTest:#,##0}");
		Console.WriteLine("Add int to dictionary: " + addIntElapsed);
		Console.WriteLine("Add string to dictionary: " + addStringElapsed);
		Console.WriteLine("Read int from dictionary: " + readIntElapsed);
		Console.WriteLine("Read string from dictionary: " + readStringElapsed);
	}
}
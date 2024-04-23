using System.Diagnostics;

namespace IntStringComparison;

internal abstract class Program {
	//Limit the number generated to avoid it being too big
	private const int MaxNumbersToTest = 100_000_000;
	private static void Main() {
		DateTime start = DateTime.Now;
		Console.WriteLine("### Overall Start Time: " + start.ToLongTimeString());
		Console.WriteLine();
		Random rand = new();
		MeasureDifference(rand.Next(0, MaxNumbersToTest));
		DateTime end = DateTime.Now;
		Console.WriteLine();
		Console.WriteLine("### Overall End Time: " + end.ToLongTimeString());
		Console.WriteLine("### Overall Run Time: " + (end - start));
		Console.WriteLine();
		Console.WriteLine("Hit Enter to Exit");
		Console.ReadLine();
	}

	private static void MeasureDifference(int numberOfNumbersToTest) {
		Console.WriteLine($"######## {nameof(MeasureDifference)}");
		Console.WriteLine($"Number of values to convert: {numberOfNumbersToTest:#,##0}");

		Stopwatch sw = new();

		string[] s = new string[numberOfNumbersToTest];
		int[] i = new int[numberOfNumbersToTest];

		Dictionary<string, int> ds = new();
		Dictionary<int, int> di = new();

		InitializeArrays(numberOfNumbersToTest, i, s);
		Thread.Sleep(500);

		string addIntElapsed = MeasureAddInt(numberOfNumbersToTest, sw, di, i);
		Thread.Sleep(500);

		string addStringElapsed = MeasureAddString(numberOfNumbersToTest, sw, ds, s, i);
		Thread.Sleep(500);

		string readIntElapsed = MeasureReadInt(numberOfNumbersToTest, sw, di, i);
		Thread.Sleep(500);

		string readStringElapsed = MeasureReadString(numberOfNumbersToTest, sw, ds, s);
		Thread.Sleep(500);

		Array.Clear(i, 0, i.Length);
		Array.Clear(s, 0, s.Length);
		di.Clear();
		ds.Clear();

		Console.WriteLine("###########################################################");
		Console.WriteLine("Results Summary");
		Console.WriteLine($"Number of dictionary elements: {numberOfNumbersToTest:#,##0}");
		Console.WriteLine("Add int to dictionary: " + addIntElapsed);
		Console.WriteLine("Add string to dictionary: " + addStringElapsed);
		Console.WriteLine("Read int from dictionary: " + readIntElapsed);
		Console.WriteLine("Read string from dictionary: " + readStringElapsed);
	}

	private static string MeasureReadString(int numberOfNumbersToTest, Stopwatch sw, IReadOnlyDictionary<string, int> ds, IReadOnlyList<string> s) {
		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting reading stringified-int-key from dictionary test " +
		                  numberOfNumbersToTest.ToString("#,##0") + " at: " + DateTime.Now.ToLongTimeString());
		long total = 0;
		sw.Restart();
		for (int x = 0; x < numberOfNumbersToTest; x++)
			total += ds[s[x]];
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Total: " + total.ToString("#,##0"));
		string readStringElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + readStringElapsed);
		return readStringElapsed;
	}

	private static string MeasureReadInt(int numberOfNumbersToTest, Stopwatch sw, IReadOnlyDictionary<int, int> di, IReadOnlyList<int> i) {
		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting reading int-key from dictionary test " +
		                  numberOfNumbersToTest.ToString("#,##0") + " at: " + DateTime.Now.ToLongTimeString());
		long total = 0;
		sw.Restart();
		for (int x = 0; x < numberOfNumbersToTest; x++)
			total += di[i[x]];
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Total: " + total.ToString("#,##0"));
		string readIntElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + readIntElapsed);
		return readIntElapsed;
	}

	private static string MeasureAddString(int numberOfNumbersToTest, Stopwatch sw, IDictionary<string, int> ds, IReadOnlyList<string> s, IReadOnlyList<int> i) {
		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting adding stringified-int-key to dictionary test " +
		                  numberOfNumbersToTest.ToString("#,##0") + " at: " + DateTime.Now.ToLongTimeString());
		sw.Restart();
		for (int x = 0; x < numberOfNumbersToTest; x++)
			ds.Add(s[x], i[x]);
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Number of elements: " + ds.Count.ToString("#,##0"));
		string addStringElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + addStringElapsed);
		return addStringElapsed;
	}

	private static string MeasureAddInt(int numberOfNumbersToTest, Stopwatch sw, IDictionary<int, int> di, IReadOnlyList<int> i) {
		Console.WriteLine("###########################################################");
		Console.WriteLine("Starting adding int-key to dictionary test " + numberOfNumbersToTest.ToString("#,##0") +
		                  " at: " + DateTime.Now.ToLongTimeString());
		sw.Restart();
		for (int x = 0; x < numberOfNumbersToTest; x++)
			di.Add(i[x], i[x]);
		sw.Stop();
		Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString());
		Console.WriteLine("Number of elements: " + di.Count.ToString("#,##0"));
		string addIntElapsed = sw.Elapsed.ToString(@"mm\:ss\.fffffff");
		Console.WriteLine("Time to run: " + addIntElapsed);
		return addIntElapsed;
	}

	private static void InitializeArrays(int numberOfNumbersToTest, int[] i, IList<string> s) {
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
	}
}
using System.Diagnostics;

namespace IntStringComparison;

class Program {
	private const string DummyDictionaryItem = "Potato";

	private static long _nanosecPerTick = 1000000000L / Stopwatch.Frequency;
	private const string TimeMeasureUnits = "ns";

	private static void Main() {
		CalculateIntDictionary();
		CalculateStringDictionary();
	}

	private static void CalculateIntDictionary() {
		Random rng = new();
		RandomDictionary<int, string> dictionaryInt = new(10000000, GenerateInt, () => DummyDictionaryItem);

		Stopwatch watch = Stopwatch.StartNew();
		dictionaryInt.Iterate();
		watch.Stop();
		long elapsed = watch.ElapsedTicks * _nanosecPerTick;
		Console.WriteLine($"Time elapsed (int): {elapsed}{TimeMeasureUnits}");
		return;

		int GenerateInt() => rng.Next();
	}

	private static void CalculateStringDictionary() {
		RandomDictionary<string, string> dictionaryString = new(10000000, GenerateGuid, () => DummyDictionaryItem);

		Stopwatch watch = Stopwatch.StartNew();
		dictionaryString.Iterate();
		watch.Stop();
		long elapsed = watch.ElapsedTicks * _nanosecPerTick;
		Console.WriteLine($"Time elapsed (string): {elapsed}{TimeMeasureUnits}");
		return;

		string GenerateGuid() => Guid.NewGuid().ToString();
	}
}
namespace IntStringComparison;

public readonly struct TestResult(string intAddTime, string stringAddTime, string intReadTime, string stringReadTime, string elementsTested) {
	public override string ToString() => $"{intAddTime}\t{stringAddTime}\t{intReadTime}\t{stringReadTime}\t{elementsTested}";
}
namespace IntStringComparison;

public class RandomDictionary<K, V> where K : notnull {
	private readonly List<K> _keys = [];

	private readonly Dictionary<K, V> _dictionary = new();

	public RandomDictionary(int members, Func<K> keyGenerator, Func<V> valueGenerator) {
		for (int i = 0; i < members; i++) {
			K key = keyGenerator.Invoke();
			while (_dictionary.TryAdd(key, valueGenerator.Invoke()) == false) {
				key = keyGenerator.Invoke();
			}
			_keys.Add(key);
		}
	}

	public void Iterate() {
		foreach (V _ in _keys.Select(key => _dictionary[key])) { }
	}
}
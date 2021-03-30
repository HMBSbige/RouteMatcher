using BenchmarkDotNet.Attributes;
using RouteMatcher.Abstractions;
using RouteMatcher.DomainMatchers;
using System.IO;

namespace RouteMatcher.Benchmark
{
	[MemoryDiagnoser, RankColumn]
	public class DomainMatcherBenchmark
	{
		private readonly DomainMatcherHash<Rule> _hash;
		private readonly DomainMatcherTrie<Rule> _trie;

		public DomainMatcherBenchmark()
		{
			_hash = LoadHash();
			_trie = LoadTrie();
		}

		[Benchmark]
		public DomainMatcherHash<Rule> LoadHash()
		{
			return LoadBase<DomainMatcherHash<Rule>>();
		}

		[Benchmark]
		public DomainMatcherTrie<Rule> LoadTrie()
		{
			return LoadBase<DomainMatcherTrie<Rule>>();
		}

		[Benchmark]
		public void Hash()
		{
			Base(_hash);
		}

		[Benchmark(Baseline = true)]
		public void Trie()
		{
			Base(_trie);
		}

		private static void Base(IMatcher<string, Rule> tree)
		{
			tree.Match(@"com");
			tree.Match(@"b.com");
			tree.Match(@"ab.com");
			tree.Match(@"d.b.com");
			tree.Match(@"a.d.b.com");

			tree.Match(@"a.b.cn");
			tree.Match(@"c.cn");
			tree.Match(@"d.cn");
			tree.Match(@"b.c.cn");
			tree.Match(@"a.b.c.cn");

			tree.Match(@"1234.org");
		}

		private static T LoadBase<T>() where T : IDomainMatcher<Rule>, new()
		{
			var res = new T();
			var lines = File.ReadAllLines(@"chndomains.txt");
			foreach (var line in lines)
			{
				res.Update(line, Rule.Direct);
			}
			return res;
		}
	}
}

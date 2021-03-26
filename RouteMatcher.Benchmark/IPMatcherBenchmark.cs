using BenchmarkDotNet.Attributes;
using RouteMatcher.Abstractions;
using RouteMatcher.Enums;
using RouteMatcher.IPMatchers;
using System.IO;
using System.Net;

namespace RouteMatcher.Benchmark
{
	[MemoryDiagnoser, RankColumn]
	public class IPMatcherBenchmark
	{
		private readonly IPMatcherHash _hash;
		private readonly IPMatcherTrie _trie;

		public IPMatcherBenchmark()
		{
			_hash = LoadHash();
			_trie = LoadTrie();
		}

		[Benchmark]
		public IPMatcherHash LoadHash()
		{
			return LoadBase<IPMatcherHash>();
		}

		[Benchmark]
		public IPMatcherTrie LoadTrie()
		{
			return LoadBase<IPMatcherTrie>();
		}

		[Benchmark(Baseline = true)]
		public void Hash()
		{
			Base(_hash);
		}

		[Benchmark]
		public void Trie()
		{
			Base(_trie);
		}

		private static void Base(IMatcher<IPAddress, Rule> tree)
		{
			tree.Match(IPAddress.Parse(@"10.0.0.123"));
			tree.Match(IPAddress.Parse(@"1.0.1.0"));
			tree.Match(IPAddress.Parse(@"1.0.1.1"));
			tree.Match(IPAddress.Parse(@"1.0.1.15"));
			tree.Match(IPAddress.Parse(@"1.0.1.16"));
			tree.Match(IPAddress.Parse(@"1.0.1.250"));
			tree.Match(IPAddress.Parse(@"1.0.2.0"));
			tree.Match(IPAddress.Parse(@"1.1.1.1"));
			tree.Match(IPAddress.Parse(@"8.8.8.8"));

			tree.Match(IPAddress.Parse(@"192.168.20.15"));
			tree.Match(IPAddress.Parse(@"192.168.20.19"));
		}

		private static T LoadBase<T>() where T : IIPAddressMatcher<IPAddress, Rule>, new()
		{
			var res = new T();
			var lines = File.ReadAllLines(@"china_ipv4_ipv6_list.txt");

			foreach (var line in lines)
			{
				var strings = line.Split('/');
				res.Update(IPAddress.Parse(strings[0]), byte.Parse(strings[1]), Rule.Direct);
			}

			return res;
		}
	}
}

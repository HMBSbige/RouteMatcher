using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteMatcher.Benchmark;
using RouteMatcher.DomainMatchers;

namespace UnitTest
{
	[TestClass]
	public class DomainMatcherTest
	{
		[TestMethod]
		public void Hash()
		{
			TestUtils.TestDomainMatcher(new DomainMatcherHash<Rule>());
		}

		[TestMethod]
		public void Trie()
		{
			TestUtils.TestDomainMatcher(new DomainMatcherTrie<Rule>());
		}

		[TestMethod]
		public void Benchmark()
		{
			var _ = BenchmarkRunner.Run<DomainMatcherBenchmark>();
		}
	}
}

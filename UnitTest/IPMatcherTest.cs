using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteMatcher.Benchmark;
using RouteMatcher.IPMatchers;

namespace UnitTest
{
	[TestClass]
	public class IPMatcherTest
	{
		[TestMethod]
		public void Hash()
		{
			TestUtils.TestIpMatcher(new IPMatcherHash());
		}

		[TestMethod]
		public void Trie()
		{
			TestUtils.TestIpMatcher(new IPMatcherTrie());
		}

		[TestMethod]
		public void Benchmark()
		{
			var _ = BenchmarkRunner.Run<IPMatcherBenchmark>();
		}
	}
}

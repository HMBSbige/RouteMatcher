using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteMatcher.Abstractions;
using RouteMatcher.Enums;
using System.Net;

namespace UnitTest
{
	public static class TestUtils
	{
		public static void TestIpMatcher(IIPAddressMatcher<IPAddress, Rule> tree)
		{
			tree.Update(IPAddress.Parse(@"1.0.1.0"), 28, Rule.Direct);
			tree.Update(IPAddress.Parse(@"1.1.1.1"), 32, Rule.Block);
			tree.Update(IPAddress.Parse(@"8.8.8.8"), 32, Rule.Proxy);

			tree.Update(IPAddress.Parse(@"192.168.0.0"), 16, Rule.Direct);
			tree.Update(IPAddress.Parse(@"192.168.20.16"), 28, Rule.Proxy);

			Assert.AreEqual(Rule.Unknown, tree.Match(IPAddress.Parse(@"10.0.0.123")));
			Assert.AreEqual(Rule.Direct, tree.Match(IPAddress.Parse(@"1.0.1.0")));
			Assert.AreEqual(Rule.Direct, tree.Match(IPAddress.Parse(@"1.0.1.1")));
			Assert.AreEqual(Rule.Direct, tree.Match(IPAddress.Parse(@"1.0.1.15")));
			Assert.AreEqual(Rule.Unknown, tree.Match(IPAddress.Parse(@"1.0.1.16")));
			Assert.AreEqual(Rule.Unknown, tree.Match(IPAddress.Parse(@"1.0.1.250")));
			Assert.AreEqual(Rule.Unknown, tree.Match(IPAddress.Parse(@"1.0.2.0")));
			Assert.AreEqual(Rule.Block, tree.Match(IPAddress.Parse(@"1.1.1.1")));
			Assert.AreEqual(Rule.Proxy, tree.Match(IPAddress.Parse(@"8.8.8.8")));
			Assert.AreEqual(Rule.Direct, tree.Match(IPAddress.Parse(@"192.168.20.15")));
			Assert.AreEqual(Rule.Proxy, tree.Match(IPAddress.Parse(@"192.168.20.19")));
		}

		public static void TestDomainMatcher(IDomainMatcher<string, Rule> tree)
		{
			tree.Update(@"a.b.c.cn", Rule.Block);
			tree.Update(@"b.c.cn", Rule.Proxy);
			tree.Update(@"cn", Rule.Direct);
			tree.Update(@"b.com", Rule.Proxy);
			tree.Update(@"ab.com", Rule.Block);

			Assert.AreEqual(Rule.Unknown, tree.Match(@"com"));
			Assert.AreEqual(Rule.Proxy, tree.Match(@"b.com"));
			Assert.AreEqual(Rule.Block, tree.Match(@"ab.com"));
			Assert.AreEqual(Rule.Proxy, tree.Match(@"d.b.com"));
			Assert.AreEqual(Rule.Proxy, tree.Match(@"a.d.b.com"));

			Assert.AreEqual(Rule.Direct, tree.Match(@"a.b.cn"));
			Assert.AreEqual(Rule.Direct, tree.Match(@"c.cn"));
			Assert.AreEqual(Rule.Direct, tree.Match(@"d.cn"));
			Assert.AreEqual(Rule.Proxy, tree.Match(@"b.c.cn"));
			Assert.AreEqual(Rule.Block, tree.Match(@"a.b.c.cn"));

			Assert.AreEqual(Rule.Unknown, tree.Match(@"1234.org"));
		}
	}
}

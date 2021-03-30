using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteMatcher.Abstractions;
using System.Net;
using static UnitTest.Rule;

namespace UnitTest
{
	public static class TestUtils
	{
		public static void TestIpMatcher(IIPAddressMatcher<Rule> tree)
		{
			tree.Update(IPAddress.Parse(@"1.0.1.0"), 28, Direct);
			tree.Update(IPAddress.Parse(@"1.1.1.1"), 32, Block);
			tree.Update(IPAddress.Parse(@"8.8.8.8"), 32, Proxy);

			tree.Update(IPAddress.Parse(@"192.168.0.0"), 16, Direct);
			tree.Update(IPAddress.Parse(@"192.168.20.16"), 28, Proxy);

			Assert.AreEqual(Unknown, tree.Match(IPAddress.Parse(@"10.0.0.123")));
			Assert.AreEqual(Direct, tree.Match(IPAddress.Parse(@"1.0.1.0")));
			Assert.AreEqual(Direct, tree.Match(IPAddress.Parse(@"1.0.1.1")));
			Assert.AreEqual(Direct, tree.Match(IPAddress.Parse(@"1.0.1.15")));
			Assert.AreEqual(Unknown, tree.Match(IPAddress.Parse(@"1.0.1.16")));
			Assert.AreEqual(Unknown, tree.Match(IPAddress.Parse(@"1.0.1.250")));
			Assert.AreEqual(Unknown, tree.Match(IPAddress.Parse(@"1.0.2.0")));
			Assert.AreEqual(Block, tree.Match(IPAddress.Parse(@"1.1.1.1")));
			Assert.AreEqual(Proxy, tree.Match(IPAddress.Parse(@"8.8.8.8")));
			Assert.AreEqual(Direct, tree.Match(IPAddress.Parse(@"192.168.20.15")));
			Assert.AreEqual(Proxy, tree.Match(IPAddress.Parse(@"192.168.20.19")));
		}

		public static void TestDomainMatcher(IDomainMatcher<Rule> tree)
		{
			tree.Update(@"a.b.c.cn", Block);
			tree.Update(@"b.c.cn", Proxy);
			tree.Update(@"cn", Direct);
			tree.Update(@"b.com", Proxy);
			tree.Update(@"ab.com", Block);

			Assert.AreEqual(Unknown, tree.Match(@"com"));
			Assert.AreEqual(Proxy, tree.Match(@"b.com"));
			Assert.AreEqual(Block, tree.Match(@"ab.com"));
			Assert.AreEqual(Proxy, tree.Match(@"d.b.com"));
			Assert.AreEqual(Proxy, tree.Match(@"a.d.b.com"));

			Assert.AreEqual(Direct, tree.Match(@"a.b.cn"));
			Assert.AreEqual(Direct, tree.Match(@"c.cn"));
			Assert.AreEqual(Direct, tree.Match(@"d.cn"));
			Assert.AreEqual(Proxy, tree.Match(@"b.c.cn"));
			Assert.AreEqual(Block, tree.Match(@"a.b.c.cn"));

			Assert.AreEqual(Unknown, tree.Match(@"1234.org"));
		}
	}
}

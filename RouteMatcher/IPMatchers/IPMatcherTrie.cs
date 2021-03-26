using RouteMatcher.Abstractions;
using RouteMatcher.Enums;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace RouteMatcher.IPMatchers
{
	public class IPMatcherTrie : IIPAddressMatcher<IPAddress, Rule>
	{
		private class Node
		{
			public Rule Result { get; set; }
			public Node?[]? Children { get; set; }

			[MemberNotNull(nameof(Children))]
			public void CreateNodes()
			{
				Children ??= new Node[2];
			}
		}

		private readonly Node _root = new();
		private readonly byte[] _buffer = new byte[16];
		private readonly bool[] _bitBuffer = new bool[16 * 8];

		public void Update(IPAddress data, byte netmask, Rule result)
		{
			Init(data, ref netmask, _bitBuffer);
			var bits = _bitBuffer.AsSpan(0, netmask);

			var root = _root;
			foreach (var bit in bits)
			{
				root.CreateNodes();
				var children = root.Children;

				var index = Convert.ToByte(bit);

				children[index] ??= new Node();

				root = children[index]!;
			}

			root.Result = result;
		}

		public Rule Match(IPAddress data)
		{
			var netmask = data.AddressFamily == AddressFamily.InterNetwork ? (byte)32 : (byte)128;

			Init(data, ref netmask, _bitBuffer);
			var bits = _bitBuffer.AsSpan(0, netmask);

			var root = _root;
			var cacheRule = root.Result;

			foreach (var bit in bits)
			{
				var children = root.Children;
				var index = Convert.ToByte(bit);

				if (children?[index] is not null)
				{
					if (!Equals(root.Result, default(Rule)))
					{
						cacheRule = root.Result;
					}

					root = children[index]!;
				}
				else
				{
					break;
				}
			}

			if (!Equals(root.Result, default(Rule)))
			{
				cacheRule = root.Result;
			}

			return cacheRule;
		}

		private void Init(IPAddress ip, ref byte netmask, Span<bool> root)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				ip = ip.MapToIPv6();
				netmask += 96; // 128-32
			}

			ip.TryWriteBytes(_buffer, out _); // out length 16
			var bytes = _buffer.AsSpan(0, 16);

			var children = root;
			foreach (var b in bytes)
			{
				children.GetBits(b);
				children = children.Slice(8);
			}
		}
	}
}

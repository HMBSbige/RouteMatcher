using RouteMatcher.Abstractions;
using RouteMatcher.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RouteMatcher.IPMatchers
{
	public class IPMatcherHash : IIPAddressMatcher<IPAddress, Rule>
	{
		private readonly Dictionary<string, Rule> _matcher = new();
		private readonly byte[] _buffer = new byte[16 + 16 * 8];

		public void Update(IPAddress data, byte netmask, Rule result)
		{
			var str = Init(data, netmask);
			_matcher[str] = result;
		}

		public Rule Match(IPAddress data)
		{
			var str = Init(data, data.AddressFamily == AddressFamily.InterNetwork ? (byte)32 : (byte)128);

			for (var i = str.Length; i >= 1; --i)
			{
				var temp = str.Substring(0, i);
				if (_matcher.TryGetValue(temp, out var res))
				{
					return res;
				}
			}

			return default;
		}

		private string Init(IPAddress ip, byte netmask)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				ip = ip.MapToIPv6();
				netmask += 96; // 128-32
			}

			ip.TryWriteBytes(_buffer, out _); // out length 16

			var bytes = _buffer.AsSpan(0, 16);
			Span<byte> root = _buffer.AsSpan(16);

			var children = root;
			foreach (var b in bytes)
			{
				children.GetBits(b);
				children = children.Slice(8);
			}

			return Encoding.UTF8.GetString(root.Slice(0, netmask));
		}
	}
}

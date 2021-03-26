using RouteMatcher.Abstractions;
using RouteMatcher.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RouteMatcher.DomainMatchers
{
	public class DomainMatcherTrie : IDomainMatcher<string, Rule>
	{
		private const int CharsCount = 26 + 10 + 1 + 1; // a-z,0-9,-,.

		private class Node
		{
			public Rule Result { get; set; }
			public Node?[]? Children { get; set; }

			[MemberNotNull(nameof(Children))]
			public void CreateNodes()
			{
				Children ??= new Node[CharsCount];
			}
		}

		private readonly Node _root = new();

		public void Update(string data, Rule result)
		{
			data = Init(data);

			var root = _root;
			foreach (var c in data)
			{
				root.CreateNodes();

				var children = root.Children;
				var index = GetCode(c);

				if (children[index] is not null)
				{
					root = children[index]!;
				}
				else
				{
					children[index] = new Node();
					root = children[index]!;
				}
			}
			root.Result = result;
		}

		public Rule Match(string data)
		{
			data = Init(data);

			var root = _root;
			var cacheRule = root.Result;

			foreach (var c in data)
			{
				var children = root.Children;
				var index = GetCode(c);

				if (children?[index] is not null)
				{
					if (index == CharsCount - 1 && !Equals(root.Result, default(Rule)))
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

		private static string Init(string input)
		{
			return string.Create(input.Length, input, (chars, state) =>
			{
				state.ToLower().Trim('.').AsSpan().CopyTo(chars);
				chars.Reverse();
			});
		}

		private static byte GetCode(char c)
		{
			return c switch
			{
				'a' => 0,
				'b' => 1,
				'c' => 2,
				'd' => 3,
				'e' => 4,
				'f' => 5,
				'g' => 6,
				'h' => 7,
				'i' => 8,
				'j' => 9,
				'k' => 10,
				'l' => 11,
				'm' => 12,
				'n' => 13,
				'o' => 14,
				'p' => 15,
				'q' => 16,
				'r' => 17,
				's' => 18,
				't' => 19,
				'u' => 20,
				'v' => 21,
				'w' => 22,
				'x' => 23,
				'y' => 24,
				'z' => 25,
				'0' => 26,
				'1' => 27,
				'2' => 28,
				'3' => 29,
				'4' => 30,
				'5' => 31,
				'6' => 32,
				'7' => 33,
				'8' => 34,
				'9' => 35,
				'-' => 36,
				'.' => 37,
				_ => throw new FormatException()
			};
		}
	}
}

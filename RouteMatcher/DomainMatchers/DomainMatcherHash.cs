using RouteMatcher.Abstractions;
using System;
using System.Collections.Generic;

namespace RouteMatcher.DomainMatchers
{
	public class DomainMatcherHash<TResult> : IDomainMatcher<TResult> where TResult : struct
	{
		private readonly Dictionary<string, TResult> _matcher = new(StringComparer.OrdinalIgnoreCase);

		public void Update(string data, TResult result)
		{
			data = Init(data);
			_matcher[data] = result;
		}

		public TResult Match(string data)
		{
			data = Init(data);
			while (true)
			{
				if (_matcher.TryGetValue(data, out var res))
				{
					return res;
				}
				var index = data.IndexOf('.');
				if (index < 0)
				{
					break;
				}

				data = data[(index + 1)..];
			}

			return default;
		}

		/// <summary>
		/// 输入前自行处理 Idn
		/// </summary>
		private static string Init(string input)
		{
			return input.Trim('.');
		}
	}
}

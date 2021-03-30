using System.Net;

namespace RouteMatcher.Abstractions
{
	public interface IIPAddressMatcher<TResult> : IMatcher<IPAddress, TResult> where TResult : struct
	{
		void Update(IPAddress data, byte netmask, TResult result);
	}
}

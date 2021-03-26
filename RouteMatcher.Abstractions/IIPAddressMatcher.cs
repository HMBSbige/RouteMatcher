namespace RouteMatcher.Abstractions
{
	public interface IIPAddressMatcher<in TType, TResult> : IMatcher<TType, TResult>
	{
		void Update(TType data, byte netmask, TResult result);
	}
}

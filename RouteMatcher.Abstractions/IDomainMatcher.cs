namespace RouteMatcher.Abstractions
{
	public interface IDomainMatcher<TResult> : IMatcher<string, TResult> where TResult : struct
	{
		void Update(string data, TResult result);
	}
}

namespace RouteMatcher.Abstractions
{
	public interface IDomainMatcher<in TType, TResult> : IMatcher<TType, TResult>
	{
		void Update(TType data, TResult result);
	}
}

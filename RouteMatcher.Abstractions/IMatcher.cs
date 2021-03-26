namespace RouteMatcher.Abstractions
{
	public interface IMatcher<in TType, out TResult>
	{
		TResult Match(TType data);
	}
}

namespace Hybrid.CQRS.GraphQL.Client
{
    public class ErrorPath : List<object>
    {
        public ErrorPath()
        {
        }

        public ErrorPath(IEnumerable<object> collection) : base(collection)
        {
        }
    }
}
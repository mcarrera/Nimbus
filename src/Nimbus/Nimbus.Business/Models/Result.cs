namespace Nimbus.Business.Models
{
    public class Result<T>
    {
        public T? Value { get; set; }

        //todo: create a ResultMessage class with status, message, etc
        public List<string> Messages { get; set; } = [];
    }
}

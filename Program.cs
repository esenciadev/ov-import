using NetMQ;

internal class Program
{
    private static void Main(string[] args)
    {
        using (var runtime = new NetMQRuntime())
        {
            var sub = new Subscriber();
            runtime.Run(sub.Run());
        }
    }
}
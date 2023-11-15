using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting main");
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        host.Run();
    }
}
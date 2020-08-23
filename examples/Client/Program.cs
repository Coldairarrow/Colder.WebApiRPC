using Colder.WebApiRPC.Client;
using Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:5000/api/") };
            var client = WebApiRPCClientFactory.GetClient<ITestService>(httpClient);
            object res;
            res = await client.Hi(Guid.NewGuid().ToString(), "小明");
            Console.WriteLine(res);

            res = await client.IsOK(Guid.NewGuid().ToString(), "小明");
            Console.WriteLine(res);

            Console.ReadLine();
        }
    }
}

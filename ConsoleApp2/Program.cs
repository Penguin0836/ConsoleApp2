using TestTask.Services.Utility;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestTask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите адрес страницы:");
            string url = Console.ReadLine();
            try
            {
                await HTML.Download(url);
                HTML.FindUniqueWord();
            }
            catch (Exception e)
            {
                MyLogger.GetInstance().Error(e.ToString());
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}

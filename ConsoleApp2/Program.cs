using TestTask.Services.Utility;
using System;
using System.Threading.Tasks;

namespace TestTask
{
    internal static class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Введите адрес страницы:");
            var url = Console.ReadLine();
            try
            {
                await Html.Download(url);
                Html.FindUniqueWord();
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

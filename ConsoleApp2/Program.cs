using System;
using System.Data.SQLite;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using TestTask.Services.Implementations;
using TestTask.Services.Interfaces;
using TestTask.Services.Utility;
using ILogger = TestTask.Services.Utility.ILogger;

namespace TestTask
{
    internal static class Program
    {
        private static async Task Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NLogLogger>().As<ILogger>().SingleInstance();

            builder.RegisterType<DatabaseWorker>().As<IDatabaseWorker>().SingleInstance();
            
            builder.RegisterType<HtmlDocument>()
                .AsSelf()
                .InstancePerDependency()
                .OnActivated(e =>
                {
                    var path = "site.html";
                    e.Instance.Load(path);
                });
            builder.RegisterType<HtmlReader>().As<IDocumentReader>().InstancePerDependency();
            
            builder.RegisterType<HtmlWorker>().InstancePerDependency();
            
            builder.Register(_ => new HttpClient())
                .As<HttpClient>();
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\Нияз\RiderProjects\ConsoleApp2\ConsoleApp2\appsettings.json")
                .Build();
            builder.RegisterInstance<IConfiguration>(configuration);
            
            builder.Register(ctx =>
            {
                var connStr = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
                var connection = new SQLiteConnection(connStr);
                connection.Open();
                return connection;
            }).As<SQLiteConnection>().InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                var connection = ctx.Resolve<SQLiteConnection>();
                return new SQLiteCommand { Connection = connection };
            }).As<SQLiteCommand>().InstancePerDependency();

            builder.RegisterType<DatabaseWorker>()
                .UsingConstructor(typeof(SQLiteConnection), typeof(SQLiteCommand))
                .As<IDatabaseWorker>()
                .InstancePerDependency();

            var container = builder.Build();
            
            await using (var scope = container.BeginLifetimeScope())
            {
                var logger = scope.Resolve<ILogger>();
                var htmlWorker = scope.Resolve<HtmlWorker>();
                var databaseWorker = scope.Resolve<IDatabaseWorker>();
                
                databaseWorker.CreateDatabase("WordTable.db");
                databaseWorker.CreateTable("words", [
                    ("word", "TEXT NOT NULL"),
                    ("count_of_word", "INTEGER NOT NULL")
                ]);

                Console.WriteLine("Введите адрес страницы:");
                var url = Console.ReadLine();
                try
                {
                    await htmlWorker.Download(url);
                    var rawText = htmlWorker.GetText();
                    var words = htmlWorker.ClearText(rawText);
                    var uniqueWords = htmlWorker.FindUniqueWord(words);
                    //TODO: вынести в отдельный класс для работы с таблицами
                    databaseWorker.Insert(uniqueWords);
                    //TODO: вынести в отдельный класс
                    htmlWorker.DisplayWords(uniqueWords.Keys.ToArray());
                }
                catch (Exception e)
                {
                    logger.Error(e.ToString());
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }
}

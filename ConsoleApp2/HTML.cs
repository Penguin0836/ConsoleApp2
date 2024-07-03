using TestTask.Services.Utility;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestTask
{
    public static class Html
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly HtmlDocument Site = new HtmlDocument();
        public static async Task Download(string uri)
        {
            MyLogger.GetInstance().Info("Send a GET request");
            var responseBody = await Client.GetStringAsync(uri);

            MyLogger.GetInstance().Info("Save a HTML");
            await using var htmlWriter = new StreamWriter("site.html", false, Encoding.Default);
            await htmlWriter.WriteLineAsync(responseBody);
        }

        private static string[] ClearText()
        {
            var htmlSymbols = new Regex(@"&..*");
            var charsToRemove = new[] { "@", "+", "=", "{", "}", "*", "/", "<", ">", "%", "^", "~" };
            var separators = new[] { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '-', '—' };

            MyLogger.GetInstance().Info("Search a text");
            var htmlNodes = Site.DocumentNode.SelectSingleNode("/html/body").InnerText.Trim().Replace("&nbsp;", "").Replace("-", "");

            MyLogger.GetInstance().Info("Delete HTML symbols");
            var deletingHtmlSymbols = htmlSymbols.Replace(htmlNodes.ToString(), ""); // delete html symbols
            
            MyLogger.GetInstance().Info("Delete numbers");
            var deletingNumbers = new string(deletingHtmlSymbols.Where(c => c != '-' && (c < '0' || c > '9')).ToArray()); // delete numbers
            
            MyLogger.GetInstance().Info("Escape words");
            var escapedWords = deletingNumbers.Replace("'", "''");

            MyLogger.GetInstance().Info("Delete symbols");
            escapedWords = charsToRemove.Aggregate(escapedWords, (current, c) => current.Replace(c, string.Empty));

            MyLogger.GetInstance().Info("Split string");
            var words = escapedWords.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

        private static void WriteData(Dictionary<string, int> wordsCount)
        {
            MyLogger.GetInstance().Info("Write data into database");
            var data = new WorkWithSqLite();
            data.CreateDatabaseAndTable();
            
            for (var i = 0; i < wordsCount.Count; i++)
            {
                data.AddData(wordsCount.ElementAt(i).Key, wordsCount.ElementAt(i).Value);
            }
        }
        public static void FindUniqueWord()
        {
            var wordsCount = new Dictionary<string, int>();
            Console.WriteLine("Ожидайте...");

            MyLogger.GetInstance().Info("Load a site");
            Site.Load("site.html");

            MyLogger.GetInstance().Info("TempWords == words");
            var words = ClearText();
            foreach (var word in words)
            {
                var countOfWord = 1;
                foreach (var tempWord in words)
                {
                    if (word != tempWord) continue;
                    
                    if (wordsCount.ContainsKey(word))
                    {
                        wordsCount[word] += 1;
                    }
                    else
                    {
                        wordsCount.Add(word, countOfWord++);
                    }
                    word.Replace(word, "");
                }
            }

            MyLogger.GetInstance().Info("Order by value");
            wordsCount = wordsCount.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            WriteData(wordsCount);

            MyLogger.GetInstance().Info("Display words and their count");
            foreach (var word in wordsCount)
            {
                Console.WriteLine(word);
            }
        }
    }
}

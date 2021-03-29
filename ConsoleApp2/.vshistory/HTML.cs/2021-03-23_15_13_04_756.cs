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
    public class HTML
    {
        static readonly HttpClient client = new HttpClient();
        static HtmlDocument site = new HtmlDocument();
        public static async Task Download(string URI)
        {
            MyLogger.GetInstance().Info("Send a GET request");
            string responseBody = await client.GetStringAsync(URI);

            MyLogger.GetInstance().Info("Save a HTML");
            using StreamWriter htmlWriter = new StreamWriter("site.html", false, Encoding.Default);
            htmlWriter.WriteLine(responseBody);
        }
        public static string[] ClearText()
        {
            Regex htmlSymbols = new Regex(@"&..*");
            string[] charsToRemove = new string[] { "@", "+", "=", "{", "}", "*", "/", "<", ">", "%", "^", "~" };
            char[] separators = new char[] { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '-', '—' };

            MyLogger.GetInstance().Info("Search a text");
            string htmlNodes = site.DocumentNode.SelectSingleNode("/html/body").InnerText.Trim().Replace("&nbsp;", "").Replace("-", "");

            MyLogger.GetInstance().Info("Delete HTML symbols");
            string deletingHTMLSymbols = htmlSymbols.Replace(htmlNodes.ToString(), ""); // delete html symbols
            
            MyLogger.GetInstance().Info("Delete numbers");
            string deletingNumbers = new string(deletingHTMLSymbols.Where(c => c != '-' && (c < '0' || c > '9')).ToArray()); // delete numbers
            
            MyLogger.GetInstance().Info("Escape words");
            string escapedWords = htmlNodes.Replace("'", "''");

            MyLogger.GetInstance().Info("Delete symbols");
            foreach (string c in charsToRemove)
            {
                escapedWords = escapedWords.Replace(c, string.Empty);
            }

            MyLogger.GetInstance().Info("Split string");
            string[] words = escapedWords.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        public static void WriteData(Dictionary<string, int> wordsCount)
        {
            MyLogger.GetInstance().Info("Write data into database");
            WorkWithSQLite data = new WorkWithSQLite();
            data.CreateDatabaseAndTable();
            
            for (int i = 0; i < wordsCount.Count; i++)
            {
                data.AddData(wordsCount.ElementAt(i).Key, wordsCount.ElementAt(i).Value);
            }
        }
        public static void FindUniqueWord()
        {
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            Console.WriteLine("Ожидайте...");

            MyLogger.GetInstance().Info("Load a site");
            site.Load("site.html");

            MyLogger.GetInstance().Info("TempWords == words");
            string[] words = ClearText();
            string[] tempWords = words;
            foreach (string word in words)
            {
                int countOfWord = 1;
                foreach (string tempWord in tempWords)
                {
                    if (word == tempWord)
                    {
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

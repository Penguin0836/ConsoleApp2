using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestTask.Services.Interfaces;
using ILogger = TestTask.Services.Utility.ILogger;

namespace TestTask
{
    public partial class HtmlWorker(ILogger logger, IDocumentReader documentReader, HttpClient httpClient)
    {
        public async Task Download(string uri)
        {
            logger.Info("Send a GET request");
            var responseBody = await httpClient.GetStringAsync(uri);

            logger.Info("Save a HTML");
            await using var htmlWriter = new StreamWriter("site.html", false, Encoding.Default);
            await htmlWriter.WriteLineAsync(responseBody);
        }

        public string GetText()
        {
            logger.Info("Search a text");
            return documentReader.GetDocumentInfo().Trim().Replace("&nbsp;", "").Replace("-", "");
        }

        public string[] ClearText(string rawText)
        {
            var htmlSymbols = MyRegex();
            var charsToRemove = new[] { "@", "+", "=", "{", "}", "*", "/", "<", ">", "%", "^", "~" };
            var separators = new[] { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '-', '—' };

            logger.Info("Delete HTML symbols");
            var deletingHtmlSymbols = htmlSymbols.Replace(rawText, "");
            
            logger.Info("Delete numbers");
            var deletingNumbers = new string(deletingHtmlSymbols.Where(c => c != '-' && c is < '0' or > '9').ToArray());
            
            logger.Info("Escape words");
            var escapedWords = deletingNumbers.Replace("'", "''");

            logger.Info("Delete symbols");
            escapedWords = charsToRemove.Aggregate(escapedWords, (current, c) => current.Replace(c, string.Empty));

            logger.Info("Split string");
            var words = escapedWords.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        //TODO: вероятно, можно сделать проще
        public Dictionary<string, int> FindUniqueWord(string[] words)
        {
            var uniqueWords = new Dictionary<string, int>();
            Console.WriteLine("Ожидайте...");
            
            logger.Info("TempWords == words");
            foreach (var word in words)
            {
                var countOfWord = 1;
                foreach (var tempWord in words)
                {
                    if (word != tempWord) continue;
                    
                    if (uniqueWords.ContainsKey(word))
                    {
                        uniqueWords[word] += 1;
                    }
                    else
                    {
                        uniqueWords.Add(word, countOfWord++);
                    }
                    word.Replace(word, "");
                }
            }

            logger.Info("Order by value");
            return uniqueWords.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public void DisplayWords(string[] uniqueWords)
        {
            logger.Info("Display words and their count");
            foreach (var word in uniqueWords)
            {
                Console.WriteLine(word);
            }
        }

        [GeneratedRegex("&..*")]
        private static partial Regex MyRegex();
    }
}

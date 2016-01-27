using System;
using System.Collections.Generic;
using System.IO;

namespace Dojo.HamOrSpam
{
    class Program
    {
        private class Message
        {
            public bool IsSpam { get; set; }
            public string Contents { get; set; }
        }

        static void Main(string[] args)
        {
            var spamDetector = new SpamDetector();
            foreach (var message in ReadMessageSet("SpamTraining.txt"))
            {
                spamDetector.TrainMessage(message.IsSpam, message.Contents);
            }

            var correct = 0;
            var incorrect = 0;
            foreach (var message in ReadMessageSet("SpamValidation.txt"))
            {
                var isSpam = spamDetector.IsSpam(message.Contents);
                if ((isSpam && !message.IsSpam) || (!isSpam && message.IsSpam))
                    incorrect++;
                else
                    correct++;
            }

            Console.WriteLine("Correct: {0}, Incorrect: {1} - {2:N2}%", correct, incorrect, (correct / (decimal)(incorrect + correct)) * 100.0M);
        }

        private static IEnumerable<Message> ReadMessageSet(string filename)
        {
            foreach (var line in File.ReadAllLines(filename))
            {
                if (line.StartsWith("ham", StringComparison.InvariantCultureIgnoreCase))
                    yield return new Message {IsSpam = false, Contents = line.Remove(4)};
                else if (line.StartsWith("spam", StringComparison.InvariantCultureIgnoreCase))
                    yield return new Message {IsSpam = true, Contents = line.Remove(5)};
                else
                    throw new Exception("Hahaha. What are you reading?!");
            }
        } 
    }
}

namespace Game.ConsoleUI.Infrastructure.Helpers
{
    using System;
    using System.IO;


    //TODO: Should be made as a Service
    public class FileHelpers
    {
        public static void FileReaderBorrower(string filePath, Action<string> stringProcessor)
        {
            FileReader(filePath, reader =>
            {
                string readLine = null;
                while ((readLine = reader.ReadLine()) != null)
                {
                    stringProcessor(readLine);
                }
            });
        }

        public static string FileReaderBorrower(string filePath)
        {
            string content = null;
            if (File.Exists(filePath))
            {
                FileReader(filePath, reader =>
                {
                    content = reader.ReadToEnd();
                });
            }

            return content;
        }

        private static void FileReader(string filePath, Action<StreamReader> readerProcessor)
        {
            using (var file = File.OpenRead(filePath))
            using (var reader = new StreamReader(file))
            {
                readerProcessor(reader);
            }
        }

        public static void WriteToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
    }
}
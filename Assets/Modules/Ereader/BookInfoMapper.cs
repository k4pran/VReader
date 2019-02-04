using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Ereader{
    
    // Serializing and deserializing BookInfo using yaml
    public class BookInfoMapper {

        public static void SerializeInfo(string outputPath, BookFormat bookType, string location, string title) {
            BookInfo bookInfo = new BookInfo(title, bookType, location);
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(bookInfo);
            File.WriteAllText(outputPath, yaml);
        }
        
        public static void SerializeInfo(string outputPath, BookInfo bookInfo) {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(bookInfo);
            File.WriteAllText(outputPath, yaml);
        }

        public static BookInfo DeserializeInfo(string title) {
            string bookPath = Config.Instance.BookLibraryPath + "/VReader/"  + title + "/info.yaml";
            string bookStr;
            FileStream fileStream = new FileStream(bookPath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                bookStr = streamReader.ReadToEnd();
            }
            
            StringReader yamlInput = new StringReader(bookStr);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            BookInfo bookInfo = deserializer.Deserialize<BookInfo>(yamlInput);

            return bookInfo;
        }

        public static List<BookInfo> DeserializeAll() {
            List<BookInfo> allBooks = new List<BookInfo>();
            HashSet<string> bookNames = GetBookNames();

            foreach(string bookName in bookNames) {
                allBooks.Add(DeserializeInfo(bookName));
            }

            return allBooks;
        }
        
        public static HashSet<string> GetBookNames() {
            HashSet<string> bookNames = new HashSet<string>();
            
            string libDir = Config.Instance.BookLibraryPath + "/VReader";
            string bookLog = libDir + "/book-log.txt";
            // Empty library - lib directory created on first import
            if (!Directory.Exists(libDir) || !File.Exists(bookLog)) {
                return bookNames;
            }
            
            StreamReader file = new StreamReader(bookLog);
            string line;
            while((line = file.ReadLine()) != null) {
                if (line.Length > 0) bookNames.Add(line);
            }  
            
            Debug.Log("Found " + bookNames.Count + " books in library log");
            return bookNames;
        }
    }
}
using System;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Samples.Helpers;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Ereader{
    
    public class BookInfoMapper {

        public static void SerializeInfo(string outputPath, BookFormat bookType, string location, string title) {
            
            BookInfo bookInfo = new BookInfo(title, bookType, location);
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(bookInfo);
            File.WriteAllText(outputPath + "/info.yaml", yaml);
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
    }
}
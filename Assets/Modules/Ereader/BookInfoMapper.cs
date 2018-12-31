using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.RepresentationModel;

namespace Ereader{
    
    public class BookInfoMapper {

        public static void GenerateMapping(string outputPath, string bookType, string location, string title) {
            
            var stream = new YamlStream (
                new YamlDocument(
                    new YamlMappingNode(
                        new YamlScalarNode("type"), new YamlScalarNode(bookType),
                        new YamlScalarNode("origin"), new YamlScalarNode(location),
                        new YamlScalarNode("title"), new YamlScalarNode(title)
                    )
                )
            );
            
            using (TextWriter writer = File.CreateText(outputPath + "/info.yaml"))
                stream.Save(writer, false);
        }


        public static void GetMapping(string bookTitle) {
                        
            string configPath = Directory.GetCurrentDirectory() + "/Assets/Books/" + bookTitle;
            string fileContent;
            FileStream fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                fileContent = streamReader.ReadToEnd();
            }
            
            var yaml = new YamlStream();
            yaml.Load(new StringReader(fileContent));
            
            var mapping =
                (YamlMappingNode)yaml.Documents[0].RootNode;
            
        }
        
    }
}
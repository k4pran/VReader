﻿using System;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Ereader {
    
    public class Config{

        // Singleton pattern
        private static Config instance;
        private static readonly object padlock = new object();
        private static bool isInstantiated;
        
        // Properties
        private String bookLibraryPath;
        private int linesPerPage;

        public Config() {
            if (instance != null){
                throw new NotSupportedException("Only one instance of Config is allowed. Access via Config.Instance");
            }
        }

        public static Config Instance {
            get {
                lock(padlock) {
                    if (instance == null) {
                        instance = Deserialize();
                        isInstantiated = true;
                    }
                    return instance;
                }
            }
        }

        private static Config Deserialize(){

            // todo better option as Resources.Load()?
            String configPath = Directory.GetCurrentDirectory() + "/Assets/Modules/Ereader/config.yaml";
            String configStr;
            FileStream fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                configStr = streamReader.ReadToEnd();
            }
            
            StringReader yamlInput = new StringReader(configStr);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            Config config = deserializer.Deserialize<Config>(yamlInput);

            return config;
        }

        public string BookLibraryPath{
            get{ return bookLibraryPath; }
            private set{
                bookLibraryPath = value;
            }
        }

        public int LinesPerPage{
            get{ return linesPerPage; }
            private set{
                linesPerPage = value;
            }
        }
    }
}
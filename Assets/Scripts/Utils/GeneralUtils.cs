using System;
using System.IO;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Utils{
    public class GeneralUtils{
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }

        public static string CombinePath(string[] pathParts){
            if (pathParts.Length == 0){
                return "";
            }
            if (pathParts.Length == 1){
                return pathParts[0];
            }
            
            string path = pathParts[0];
            for(int i = 1; i < pathParts.Length; i++){
                path = Path.Combine(path, pathParts[i]);
            }

            return path;
        }
    }
}
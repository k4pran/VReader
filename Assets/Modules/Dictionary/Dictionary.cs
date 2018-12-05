using System.Net;

namespace Ereader.Knowledge{
    public class Dictionary{

        public static void LookUp(string word){
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key=APIkey&lang=en-ru&text=" + word);//
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }
        }
        
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BlihMobile.Models
{
    public class RequestBlih
    {
        private static List<string> JSonStringtoList(JObject jobject)
        {
            List<string> List = new List<string>();
            foreach (var x in jobject)
                List.Add(x.Key);
            return List;
        }
        public static string Request(string username, string password, string method, string repository = "")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/" + method);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password;
                if (repository != "")
                    json += "\"," + "\"name\":\"" + repository + "\"}";
                else
                    json += "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var data = JObject.Parse(result)["data"];
                if (method == "repository/list")
                {
                    var repo = JObject.Parse(data.ToString())["repositories"];
                    return repo.ToString();
                }
                else if (method == "sshkey/list")
                {
                    var repo = JObject.Parse(data.ToString())["sshkeys"];
                    return repo.ToString();
                }
                return null;
            }
        }
    }
}
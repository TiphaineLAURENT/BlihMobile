using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using BlihMobile.Models;

namespace BlihMobile.Models
{
    public class RequestBlih
    {
        public static bool ManageRepo(string username, string password, string repository, string method)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/" + method);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password + "\"," + "\"name\":\"" + repository + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var data = (JObject)JsonConvert.DeserializeObject(result);
                string status = data["status"].Value<string>();
                Constants.Reason = data["reason"].Value<string>();
                if (status != "200")
                    return false;
                return true;
            }
        }

        public static bool TestLogIn(string username, string password)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/repository/list");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var data = (JObject)JsonConvert.DeserializeObject(result);
                string status = data["status"].Value<string>();
                Constants.Reason = data["reason"].Value<string>();
                if (status != "200")
                    return false;
                return true;
            }
        }
        public static bool SSHKeyDelete(string username, string password, string sshkey)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/sshkey/delete");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password + "\"," + "\"sshkey\":\"" + sshkey + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var data = (JObject)JsonConvert.DeserializeObject(result);
                string status = data["status"].Value<string>();
                Constants.Reason = data["reason"].Value<string>();
                if (status != "200")
                    return false;
                return true;
            }
        }

        public static bool SetAcl(string username, string password, string repository, string user, string acl)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/repository/setacl");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password + "\"," 
                    + "\"name\":\"" + repository + "\"," + "\"acluser\":\"" + user  + "\"," + "\"acl\":\"" + acl + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var data = (JObject)JsonConvert.DeserializeObject(result);
                string status = data["status"].Value<string>();
                Constants.Reason = data["reason"].Value<string>();
                if (status != "200")
                    return false;
                return true;
            }
        }

        public static List<AclUser> RequestAcl(string username, string password, string repository)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/repository/getacl");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password + "\"," + "\"name\":\"" + repository + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                string UserName = System.String.Empty;
                string UserRights = System.String.Empty;
                string Status = System.String.Empty;
                string Reason = System.String.Empty;
                List<AclUser> Users = new List<AclUser>();

                var data = (JObject)JsonConvert.DeserializeObject(result);
                string status = data["status"].Value<string>();
                Constants.Reason = data["reason"].Value<string>();
                if (status != "200")
                    return null;

                var json = JObject.Parse(result)["data"]["acls"];

                foreach (var user in json)
                {
                    AclUser acluser = new AclUser();
                    UserName = "";
                    UserName += user["user"];
                    UserRights = "";
                    UserRights += user["rights"];

                    if (UserRights.Contains('r'))
                        acluser.read = true;
                    else
                        acluser.read = false;

                    if (UserRights.Contains('w'))
                        acluser.write = true;
                    else
                        acluser.write = false;

                    if (UserRights.Contains('x'))
                        acluser.execute = true;
                    else
                        acluser.execute = false;

                    acluser.name = UserName;

                    Users.Add(acluser);
                }
                Users.Sort((x, y) => x.name.CompareTo(y.name));
                return Users;
            }
        }

        public static List<string> RequestList(string username, string password, string method, string repository = "", string user = "")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://tiphaine-laurent.fr/api/blih/" + method);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"login\":\"" + username + "\"," + "\"password\":\"" + password + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JToken json = null;
                var result = streamReader.ReadToEnd();

                var data = (JObject)JsonConvert.DeserializeObject(result);
                string status = data["status"].Value<string>();
                Constants.Reason = data["reason"].Value<string>();
                if (status != "200")
                    return null;

                if (method.StartsWith("repository"))
                    json = JObject.Parse(result)["data"]["repositories"];
                else if (method.StartsWith("sshkey"))
                    json = JObject.Parse(result)["data"]["sshkeys"];

                string temp = System.String.Empty;
                List<string> ListRepoName = new List<string>();

                foreach (var repo in json)
                {
                    temp = "";
                    temp += repo["name"];
                    ListRepoName.Add(temp);
                }
                ListRepoName.Sort();
                return ListRepoName;
                ;
            }
        }
    }
}
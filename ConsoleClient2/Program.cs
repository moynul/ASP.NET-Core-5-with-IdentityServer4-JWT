using IdentityModel.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ConsoleClient2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            /////////////////////GrantTypes.ResourceOwnerPassword /////////////////////////

            var client4AdminSecure = new RestClient("http://localhost:54416/connect/token");
            client4AdminSecure.Timeout = -1;
            var request4AdminSecure = new RestRequest(Method.POST);


            request4AdminSecure.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request4AdminSecure.AddParameter("grant_type", "password");
            request4AdminSecure.AddParameter("client_id", "secret_user_client_id");
            request4AdminSecure.AddParameter("client_secret", "secret");
            request4AdminSecure.AddParameter("scope", "api1");
            request4AdminSecure.AddParameter("username", "moynul");
            request4AdminSecure.AddParameter("password", "pass1234");
            IRestResponse response4AdminSecure = client4AdminSecure.Execute(request4AdminSecure);
            var bearertoken4AdminSecure = response4AdminSecure.Content;
            string accessToken4AdminSecure = JsonConvert.DeserializeObject<Dictionary<string, string>>(bearertoken4AdminSecure)["access_token"];


            var client4AdminSecureContent = new HttpClient();
            client4AdminSecureContent.SetBearerToken(accessToken4AdminSecure);
             var responseAdminSecureContent = client4AdminSecureContent.GetAsync("http://localhost:55495/api/DoctorDetails/admindoctor").Result;
            var Detailscontent = responseAdminSecureContent.Content.ReadAsStringAsync().Result;
            Console.WriteLine(Detailscontent);

            ////////////////////// End /////////////////////////////////////////////
        }
    }
}

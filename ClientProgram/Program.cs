// See https://aka.ms/new-console-template for more information

using MySql.Data.MySqlClient.Memcached;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using System.Net.Http.Json;
using System.Data;
using ClientProgram;

namespace HttpClientSample
{

class Program
    {
        
        static HttpClient client = new HttpClient();
        static DatabaseInfo info = new DatabaseInfo();
        static ClientProgram.Client p = new ClientProgram.Client();
        




        public async Task<DatabaseInfo> GetDatabaseInfoAsync(string Scancode)
        {
            try
            {
               // await UpdateSetStatusAsync("3", "123@");
                HttpResponseMessage response = await client.GetAsync($"/database/getdata/{Scancode}");/// database / getdata /{ Scancode}
                if (response.IsSuccessStatusCode)
                {
                    info = await response.Content.ReadAsAsync<DatabaseInfo>();
                }
                await p.ShowDatabaseInfo(info);
                
                return info;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task UpdateSetStatusAsync(string SetStatus, string ScanCode)
        {
            string[] s = new string[2];
            s[0] = ScanCode;    
            s[1] = SetStatus;   
            HttpResponseMessage response = await client.PostAsJsonAsync(
               $"/database/setdata/", s);
            response.EnsureSuccessStatusCode();
            

        }

        static async Task Main()
        {

            await RunAsync();
            Console.ReadLine();
            Console.ReadLine();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:7134/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            await p.NextSession(3);
           
        }

        public async Task  StartSession()
        {
            
            string s = Convert.ToString(Console.ReadLine());
            await GetDatabaseInfoAsync(s);
        }

    }
}

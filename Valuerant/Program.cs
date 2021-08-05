using MoonSharp.Interpreter;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace Valuerant
{
    static class Program
    {
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string hwid = "";// Utils.Value();
            //MoonSharpFactorial2();
            string key = Prompt.ShowDialog("TestKey", "Please enter the key");
            var check = CreateProductAsync(new UserModel { hwid= hwid, key= key });
            check.Wait();
            if (check.Result)
            {
                Application.Run(new Main());
            }
            
        }
        public static void MoonSharpFactorial2()
        {
            string scriptCode = @"    
        -- defines a factorial function
        function fact (n)
            MoveMouseRelative(-20,0)
        end";

            Script script = new Script();
            script.DoString(scriptCode);

            DynValue res = script.Call(script.Globals["fact"], 4);

            var a= res.Number;
        }
        static async Task<bool> CreateProductAsync(UserModel login)
        {
            

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://valueranthwid20210801171325.azurewebsites.net/WeatherForecast");

            string json = JsonConvert.SerializeObject(login);

            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var customerJsonString = await response.Content.ReadAsStringAsync();
                if (customerJsonString == "OK")
                {
                    return true;
                }
                MessageBox.Show(customerJsonString);
            }
         
            return false;
        }
       
    }
}

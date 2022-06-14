using HttpClientSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace ClientProgram
{
    internal class Client
    {
        static Program p = new Program();
        public string Scancode { get; set; }
        public string SetStatus { get; set; }
        public DateTime Date { get; set; }



        public DatabaseInfo info { get; set; }  

        public async Task ShowDatabaseInfo(DatabaseInfo info)
        {
            if (info.SKUName != null)
            {
                Console.WriteLine(info.ConnectionInfo);
                Console.WriteLine("{0}{1}{2}",
                     info.SKUName,
                     info.SKUDescription,
                     info.Status);
                Console.WriteLine(info.ChooseStatus);
                Console.WriteLine(info.statuses[0] + " " + info.statuses[1] + " " + info.statuses[2]);
                Scancode = info.Scancode;
                await CheckData();
            }
            else
            {
                await NextSession(2);
            }


        }


        public async Task NextSession(int a)
        {
            Console.Clear();
            int variant = a;
            switch (variant)
            {
                case 0:
                    Console.WriteLine("Статус успешно обновлен");
                    Console.WriteLine("Отсканируйте код:");
                    break;
                case 1:
                    Console.WriteLine("Отмена подтверждена");
                    Console.WriteLine("Отсканируйте код:");
                    break;
                case 2:
                    Console.WriteLine("Данные не найдены");
                    Console.WriteLine("Отсканируйте код:");
                    break;
                case 3:
                    Console.WriteLine("Отсканируйте код:");
                    break;
                case 4:
                    Console.WriteLine("Отсканируйте код:");
                    await p.GetDatabaseInfoAsync(SetStatus);
                    break;
            }
            if (variant != 4)
            {
                //string s = Convert.ToString(Console.ReadLine());
                //info = p.GetDatabaseInfoAsync(s); 
                await p.StartSession();

            }

        }

        public async Task CheckNewScanCode(string SetStatus)
        {
            if (SetStatus.Contains('@'))
            {
                await NextSession(4);
            }
        }

        public async Task CheckData()
        {
            //Console.ReadLine();
           
            SetStatus = Convert.ToString(Console.ReadLine());

            if ((SetStatus != "1") && (SetStatus != "2") && (SetStatus != "3"))
            {
                await CheckNewScanCode(SetStatus);
                Console.WriteLine("Недопустимый ввод");
                //Console.Clear();
                await CheckData();
            }
            else
            {
                Console.WriteLine("1 - подтвердить, любая другая цифра - отмена");
                string confirmstatus = Convert.ToString(Console.ReadLine());
                if (confirmstatus == "1")
                {
                    Console.WriteLine("Подтверждено!\nИдет отправка данных на сервер");
                    await p.UpdateSetStatusAsync(SetStatus, Scancode);      //await GetSKUAsync("http://localhost:7134/database/setdata/{SetStatus}"); //SetData(SetStatus);
                    await NextSession(0);
                }
                else if (confirmstatus != "1")
                {
                    await CheckNewScanCode(confirmstatus);
                    await NextSession(1);
                }

            }
            Console.Read();


        }

    }
}

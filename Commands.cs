
using System;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Phonebook
{
    class Commands
    {
        public string filePath = Path.Combine(Environment.CurrentDirectory, "Phonebook.json");
        public List<Abonent> abonentList;

        public string[] Separate(string commandString)
        {
            char[] separator = new char[] { ' ' };
            return commandString.Split(separator, StringSplitOptions.None);
        }

        public void Initialize()
        {
            
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                return;
            }
            Load();
        }

        public void Load()
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer js = new JsonSerializer();
                    js.Formatting = Formatting.Indented;
                    abonentList = js.Deserialize<List<Abonent>>(jr);
                }
            }
            if (abonentList == null)
            {
                abonentList = new List<Abonent>();
            }  
        }
        public void SavePhoneBook()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            using(StreamWriter sw = new StreamWriter(filePath))
            {
                using(JsonWriter jw = new JsonTextWriter(sw))
                {
                    JsonSerializer js = new JsonSerializer();
                    js.Formatting = Formatting.Indented;
                    js.Serialize(jw, abonentList);
                }
            }
        }

        public void AddAbonent(string name, string phoneNumber)
        { 
            Abonent abonent = new Abonent(name, phoneNumber);
            abonentList.Add(abonent);
        }

        public void ShowAbonents(List<Abonent> sortedList)
        {
            if (sortedList.Count==0)
            {
                Console.WriteLine("Список пуст");
                return;
            }
            Console.WriteLine("Список абонентов:");
            foreach (Abonent abonet in sortedList)
            {
                Console.WriteLine("Имя: {0} Номер телефона: {1}", abonet.Name, abonet.PhoneNumber);
            }
        }

        public List<Abonent> SortList(List<Abonent> list, int first, int last) 
        {
            List<Abonent> sortedList = list;
            var m = sortedList[(last - first) / 2];
            int i = first;
            int j = last;
            while (i <= j)
            {
                while (CompareAbonents(sortedList[i],m)>0 && i <= last) i++;
                while (CompareAbonents(sortedList[j], m)<0 && j >= first)j--;
                if (i <= j)
                {
                    var temp = sortedList[i];
                    var temp1 = sortedList[j];
                    list.RemoveAt(i);
                    list.Insert(i, temp1);
                    list.RemoveAt(j);
                    list.Insert(j,temp);
                    i++; j--;
                }
            }
            if (first < j) SortList(sortedList, first, j);
            if (i < last) SortList(sortedList, i, last);
            return sortedList;
        }
        public int CompareAbonents(Abonent first, Abonent second)
        {
            string f = first.Name;
            string s = second.Name;
            var res = String.Compare(f, s, true);
            return res;
        }


        public void Executor(string[] command)
        {         
            switch (command[0].ToLower())
            {
                case "help":
                    Console.WriteLine(@"Чтобы добавить нового адресата введите команду ""add Имя Номер_телефона""");
                    Console.WriteLine(@"Просмотр отсортированного списка добавленных абонентов командой ""list""");
                    Console.WriteLine(@"Для выхода из приложения ввыедите команду ""exit""");
                    Console.WriteLine(@"Для очистки консоли введите команду ""clear""");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                case "add":
                    if (String.IsNullOrEmpty(command[2]))
                    {
                        Console.WriteLine(@"Ошибка. Для добавления нового адресата введите команду в формате ""add Имя Номер_телефона""");
                    }
                    AddAbonent(command[1], command[2]);
                    SavePhoneBook();
                    Console.WriteLine("done");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                case "list":
                    //ShowAbonents(SortList(abonentList,0,abonentList.Count-1));
                    ShowAbonents(abonentList.OrderBy(o=>o.Name).ToList());
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                case "exit":
                    break;

                case "clear":
                    Console.Clear();
                    Console.WriteLine(@"Введите команду. Для вызова справки введите команду ""help""");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                default:
                    Console.WriteLine(@"Неизвестная комманда");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;
            }
        }
    }
}

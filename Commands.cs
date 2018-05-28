
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
            var m = list[(last + first) / 2];
            int i = first;
            int j = last;
            while (i <= j)
            {
                while (CompareAbonents(list[i], m) < 0) i++;
                while (CompareAbonents(list[j], m) > 0) j--;
                if (i <= j)
                {
                    var temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                    i++; j--;
                }
            }
            if (first < j) SortList(list, first, j);
            if (i < last) SortList(list, i, last);
            return list;
        }
        public int CompareAbonents(Abonent first, Abonent second)
        {
            return string.Compare(first.Name, second.Name, true);
        }


        public void Executor(string[] command)
        {         
            switch (command[0].ToLower())
            {
                case "help":
                    Console.WriteLine(@"For adding new adress type ""add Name Phone_Number""");
                    Console.WriteLine(@"Show adresses type ""list""");
                    Console.WriteLine(@"For exit type ""exit""");
                    Console.WriteLine(@"For console clear type ""clear""");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                case "add":
                    if (String.IsNullOrEmpty(command[2]))
                    {
                        Console.WriteLine(@"Error. For adding new adress type ""add Name Phone_Number""");
                    }
                    AddAbonent(command[1], command[2]);
                    SavePhoneBook();
                    Console.WriteLine("done");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                case "list":
                    ShowAbonents(SortList(abonentList,0,abonentList.Count-1));
                    //ShowAbonents(abonentList.OrderBy(o=>o.Name).ToList());
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                case "exit":
                    break;

                case "clear":
                    Console.Clear();
                    Console.WriteLine(@"Type command. For help type ""help""");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;

                default:
                    Console.WriteLine(@"Unknown command");
                    command = Separate(Console.ReadLine().Trim());
                    Executor(command);
                    return;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook
{
    class Phonebook
    {
        static void Main(string[] args)
        {
            Commands commands = new Commands();
            commands.Initialize();
            Console.WriteLine(@"Type command. For help type ""help""");
            var commandString = Console.ReadLine();
            commands.Executor(commands.Separate(commandString.Trim()));
        }
    }
}

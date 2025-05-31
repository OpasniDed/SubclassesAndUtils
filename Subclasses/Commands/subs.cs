using CommandSystem;
using CommandSystem.Commands.Console;
using LabApi.Events.Arguments.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Subclasses.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class subs : ICommand
    {
        public string Command { get; } = "subs";
        public string Description { get; } = "Посмотреть все подклассы";
        public string[] Aliases { get; } = {"subclasses", "sublist"};
        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            if (args.Count > 0)
            {
                response = "Использование команды: subs";
                return false;
            }

            var sb = new StringBuilder();
            sb.AppendLine("\n</color=#00FFFF>Доступные подклассы:</color>");
            sb.AppendLine("----------------------------------");

            foreach (var subclass in Plugin.plugin.Config.Subclasses)
            {
                sb.AppendLine($"{subclass.SubId} - {subclass.substext}");
            }


            response = sb.ToString();
            return true;
        }
    }
}

using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using System;
using UnityEngine;

namespace Subclasses.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TeslaControl : ICommand
    {
        public string Command { get; } = "TesltaControl";
        public string[] Aliases { get; } = { "TC", "TeslaC", "Tesla" };
        public string Description { get; } = "Управление теслами";
        public bool SanitizeResponse => false;


        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            
            if (args.Count == 0)
            {
                response = "Использование:\n" +
                    "TC enable - Включить тесла ворота\n" +
                    "TC disable - Выключить тесла ворота\n" +
                    "TC trigger <число/default> - Как быстро тесла будет активироваться\n" +
                    "TC cooldown <число|default> - Кулдаун теслы";
                return false;
            }

            switch(args.At(0).ToLower())
            {
                case "enable":
                    Plugin.plugin.TeslaEnabled = true;
                    response = "Теслы включены";
                    return true;
                case "disable":
                    Plugin.plugin.TeslaEnabled = false;
                    response = "Тесла выключены";
                    return true;
                case "trigger":
                    if (args.Count < 2)
                    {
                        response = "Укажите значение: TC trigger <число|default>";
                        return false;
                    }


                    if (args.At(1).ToLower() == "default")
                    {
                        Plugin.plugin.TeslaActivationTime = 0.75f;
                        response = $"Активация тесла ворот вернулось по умолчанию: {Plugin.plugin.TeslaActivationTime}";
                        return true;
                    }

                    if (!float.TryParse(args.At(1), out float value))
                    {
                        response = "Укажите число: TC trigger 50";
                        return false;
                    }

                    if (value > 1f)
                    {
                        response = "Вы не можете поставить время активации больше секунды";
                        return false;
                    }
                    
                    Plugin.plugin.TeslaActivationTime = value;
                    response = $"Время активации теслы установлено на: {Plugin.plugin.TeslaActivationTime}";
                    return true;
                case "cooldown":
                    if (args.Count < 2)
                    {
                        response = "Укажите значение: TC cooldown <число|default>";
                        return false;
                    }

                    if (args.At(1).ToLower() == "default")
                    {
                        Plugin.plugin.TeslaCooldown = 0.75f;
                        response = $"Активация тесла ворот вернулось по умолчанию: {Plugin.plugin.TeslaCooldown}";
                        return true;
                    }

                    if (!float.TryParse(args.At(1), out float cooldownvalue))
                    {
                        response = "Укажите число: TC cooldown 50";
                        return false;
                    }

                    if (cooldownvalue > 5f)
                    {
                        response = "Вы не можете поставить кулдаун больше пяти секунд";
                        return false;
                    }

                    Plugin.plugin.TeslaCooldown = cooldownvalue;
                    response = $"Время кулдауна теслы установлено на: {Plugin.plugin.TeslaCooldown}";
                    return true;
                default:
                    response = "Неизвестная команда, используйте TC для просмотра команд";
                    return false;

            }
           


        }
    }
}
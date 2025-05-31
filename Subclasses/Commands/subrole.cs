using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using LabApi.Events.Arguments.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Subclasses.Commands
{
   

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class subrole : ICommand
    {
        public string Command { get; } = "subrole";
        public string Description { get; } = "Выдать подкласс игроку";
        public string[] Aliases { get; } = {};
        public bool SanitizeResponse => false;


        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (args.Count < 2)
            {
                response = "Использование: subrole <id игрока> <id подкласса>";
                return false;
            }

            if (!int.TryParse(args.At(0), out int playerId))
            {
                response = "Неверный Id игрока";
                return false;
            }

            Player player = Player.Get(playerId);
            if (player == null || player.Nickname == "Dedicated Server")
            {
                response = "Игрок не найден";
                return false;
            }

            if (!int.TryParse(args.At(1), out int subclassId))
            {
                response = "Неверный Id подкласса";
                return false;
            }

            var subclass = Plugin.plugin.Config.Subclasses
                .FirstOrDefault(s => s.SubId == subclassId);

            if (subclass == null)
            {
                response = $"Подкласс с Id {subclassId} не найден";
                return false;
            }

            Plugin.plugin.ManualRoleChanges.Add(player);
            player.Role.Set(subclass.BaseRole, spawnFlags: PlayerRoles.RoleSpawnFlags.None);

            Timing.CallDelayed(0.1f, () =>
            {
                try
                {
                    ApplySubclass(player, subclass);
                }
                catch (Exception e)
                {
                    Log.Error($"Cant give subclass {e}");
                }
                finally
                {
                    Plugin.plugin.ManualRoleChanges.Remove(player);
                }
                
            });

            response = $"Вы сменили подкласс игрока {player.Nickname} на {subclass.substext}";
            return true;
        }

        private void ApplySubclass(Player player, Subclass subclass)
        {
            string RoleName = Plugin.plugin.NameManager.GetRandomName(subclass.BaseRole);
            player.ClearInventory();
            player.Health = subclass.Health;
            player.CustomName = $"[{player.Id}] | {RoleName}";
            player.CustomInfo = subclass.Cinfo;
            player.ClearBroadcasts();
            player.Broadcast(10,
                $"<color={subclass.BroadcastColor}>Вы стали за: {subclass.Name}\n</color>" +
                $"{subclass.Cinfo}");
            foreach (var items in subclass.Items)
                player.AddItem(items);
        }
    }
}

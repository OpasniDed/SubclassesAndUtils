using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments.Components;
using MEC;
using PlayerRoles;
using Subclasses.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions.Must;

namespace Subclasses
{
    public class NameManager
    {


        private readonly Random rand = new Random();
        private readonly Config config;

        public NameManager(Config pluginconfig)
        {
            config = pluginconfig;
        }

        public string GetRandomName(RoleTypeId role)
        {
            switch (role)
            {
                case RoleTypeId.Scientist:
                    if (config.Names.ScientistNames.Any())
                        return config.Names.ScientistNames[rand.Next(config.Names.ScientistNames.Count)];
                    break;
                case RoleTypeId.FacilityGuard:
                    if (config.Names.GuardNames.Any())
                        return config.Names.GuardNames[rand.Next(config.Names.GuardNames.Count)];
                    break;
                case RoleTypeId.NtfCaptain:
                    if (config.Names.NtfNames.Any())
                        return config.Names.NtfNames[rand.Next(config.Names.NtfNames.Count)];
                    break;
                case RoleTypeId.NtfPrivate:
                    if (config.Names.NtfNames.Any())
                        return config.Names.NtfNames[rand.Next(config.Names.NtfNames.Count)];
                    break;
                case RoleTypeId.NtfSergeant:
                    if (config.Names.NtfNames.Any())
                        return config.Names.NtfNames[rand.Next(config.Names.NtfNames.Count)];
                    break;
                case RoleTypeId.NtfSpecialist:
                    if (config.Names.NtfNames.Any())
                        return config.Names.NtfNames[rand.Next(config.Names.NtfNames.Count)];
                    break;
                case RoleTypeId.ClassD:
                    return string.Format(config.Names.ClassDNames, rand.Next(10000));
            }

            return role.ToString();
        }
    }

    public class EventHandlers
    {
        private readonly Plugin plugin;
        private readonly System.Random rand = new Random();
        private readonly NameManager nameManager;
        private static Dictionary<Player, CoroutineHandle> hudCoroutines = new Dictionary<Player, CoroutineHandle>();


        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
            nameManager = new NameManager(plugin.Config);
        }

        public void PlayerVerified(VerifiedEventArgs ev)
        {
            if (ev.Player == null) return;
            ev.Player.CustomName = $"[{ev.Player.Id}] | {ev.Player.Nickname}";


            if (hudCoroutines.TryGetValue(ev.Player, out var oldCroutine))
            {
                Timing.KillCoroutines(oldCroutine);
            }

            hudCoroutines[ev.Player] = Timing.RunCoroutine(ShowHUD(ev.Player));


        }

        private IEnumerator<float> ShowHUD(Player player)
        {
            while (true)
            {
                var color = "white";

                switch (player.Role.Side)
                {
                    case Side.Scp:
                        color = "#C50000";
                        break;
                    case Side.Mtf:
                        color = "#00B7EB";
                        break;
                    case Side.ChaosInsurgency:
                        color = "#228B22";
                        break;
                }
                switch (player.Role.Type)
                {
                    case RoleTypeId.ClassD:
                        color = "#EE7600";
                        break;
                    case RoleTypeId.Scientist:
                        color = "#FAFF86";
                        break;
                    case RoleTypeId.FacilityGuard:
                        color = "#727472";
                        break;
                    case RoleTypeId.Spectator:
                        color = "white";
                        break;
                    case RoleTypeId.Overwatch:
                        color = "#00FFFF";
                        break;

                }
                
                if (!player.IsConnected) yield break;
                player.ShowHint(duration: 0.5f, message: $"<size=25><align=left><pos=-355>{Server.Name}</pos></align></size>\n" +
                    $"<size=25><align=left><pos=-355>-----------------------------------</pos></align></size>\n" +
                    $"<size=25><align=left><pos=-355><color={color}>Ник: {player.CustomName}</color></pos></align></size>\n" +
                    $"<size=25><align=left><pos=-355><color={color}>Описание:</color> {player.CustomInfo}</pos></align></size>\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                yield return Timing.WaitForSeconds(0.4f);
            }
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            if (hudCoroutines.TryGetValue(ev.Player, out var oldCroutine))
            {
                Timing.KillCoroutines(oldCroutine);
                hudCoroutines.Remove(ev.Player);
            }
        }

        public void TeslaTrigger(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.Role.Side == Side.Mtf)
                ev.IsTriggerable = false;
        }

        public void RoleChange(ChangingRoleEventArgs ev)
        {
            if (Plugin.plugin.ManualRoleChanges.Contains(ev.Player))
            {
                Plugin.plugin.ManualRoleChanges.Remove(ev.Player);
                return;
            }
            if (ev.NewRole.GetSide() == Side.Scp)
            {
                PlayerIsSCP(ev.Player);
            }

            if (ev.NewRole == RoleTypeId.Tutorial || ev.NewRole == RoleTypeId.Spectator || ev.NewRole == RoleTypeId.Overwatch)
            {
                ev.Player.CustomName = $"[{ev.Player.Id}] | {ev.Player.Nickname}";
                ev.Player.CustomInfo = "";
                return;
            }
            if (ev.NewRole == RoleTypeId.ClassD)
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    string rolename = nameManager.GetRandomName(ev.NewRole);
                    ev.Player.CustomName = $"[{ev.Player.Id}] | {rolename}";
                    ev.Player.CustomInfo = "Класс-Д";
                });
            }
            Timing.CallDelayed(0.1f, () =>
            {
                string RoleName = nameManager.GetRandomName(ev.NewRole);
                var availableSubclasses = plugin.Config.Subclasses
                    .Where(s => s.BaseRole == ev.Player.Role.Type)
                    .ToList();

                if (!availableSubclasses.Any()) return;

                var selectedSubclass = SelectSubclassWithChance(availableSubclasses);

                ev.Player.Health = selectedSubclass.Health;
                ev.Player.ClearInventory();
                ev.Player.CustomName = $"[{ev.Player.Id}] | {RoleName}";
                ev.Player.CustomInfo = selectedSubclass.Cinfo;
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10,
                    $"<color={selectedSubclass.BroadcastColor}>Вы стали за: {selectedSubclass.Name}\n</color>" +
                    $"{selectedSubclass.Cinfo}");

                foreach (var item in selectedSubclass.Items)
                    ev.Player.AddItem(item);
            });
        }

        private Subclass SelectSubclassWithChance(List<Subclass> subclasses)
        {
            if (subclasses.Count == 1)
                return subclasses[0];

            var availableSubclasses = subclasses.Where(s => CanAssignSubclass(s)).ToList();

            if (!availableSubclasses.Any())
                return subclasses.First(); 

            int totalChance = availableSubclasses.Sum(s => s.Chance);
            int randomValue = rand.Next(0, totalChance);
            int currentChance = 0;

            foreach (var subclass in availableSubclasses)
            {
                currentChance += subclass.Chance;
                if (randomValue < currentChance)
                    return subclass;
            }

            return availableSubclasses.Last();
        }

        private void PlayerIsSCP(Player player)
        {
            if (player.Role.Side == Side.Scp)
            {
                if (player.Role.Type == RoleTypeId.Scp049)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-049";
                }
                else if (player.Role.Type == RoleTypeId.Scp0492)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-049-2";
                }
                else if (player.Role.Type == RoleTypeId.Scp079)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-079";
                }
                else if (player.Role.Type == RoleTypeId.Scp096)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-096";
                }
                else if (player.Role.Type == RoleTypeId.Scp106)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-106";
                }
                else if (player.Role.Type == RoleTypeId.Scp173)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-173";
                }
                else if (player.Role.Type == RoleTypeId.Scp3114)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-3114";
                }
                else if (player.Role.Type == RoleTypeId.Scp939)
                {
                    player.CustomName = $"[{player.Id}] | Аномалия";
                    player.CustomInfo = "SCP-939";
                }
            }
        }

        private bool CanAssignSubclass(Subclass subclass)
        {
            if (subclass.Limit <= 0)
                return true;

            int currentCount = Player.List.Count(p =>
                p.Role == subclass.BaseRole &&
                p.CustomInfo?.Contains(subclass.substext) == true);

            return currentCount < subclass.Limit;
        }
    }
}
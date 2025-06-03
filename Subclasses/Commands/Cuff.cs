using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using System;
using UnityEngine;

namespace Subclasses.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Cuff : ICommand
    {
        public string Command { get; } = "cuff";
        public string[] Aliases { get; } = { "c" };
        public string Description { get; } = "Арестовать игрока";
        public bool SanitizeResponse => false;
        private static float range = 10f;

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            RaycastHit Hit;

            Ray ray = new Ray(player.GameObject.transform.position, player.GameObject.transform.forward);

            if (player is null || player.GameObject == null )
            {
                response = "Что-то пошло не так";
                return false;
            }

            if (player.IsScp == true)
            {
                response = "Вы не можете связать за SCP";
                return false;
            }

            if (!Physics.Raycast(ray, out Hit, range))
            {
                response = "Никого не найдено";
                return false;
            }

            if (player.IsCuffed == true)
            {
                response = "Вы не можете использовать команду пока связаны";
                return false;
            }

            GameObject GotHit = Hit.collider.transform.root.gameObject;
            Player playerhitted = Player.Get(GotHit);

            if (playerhitted.IsScp == true)
            {
                response = "Вы не можете связать SCP";
                return false;
            }

            if (playerhitted == null)
            {
                response = "Никого нет";
                return false;
            }

            if (playerhitted.IsCuffed == true)
            {
                playerhitted.RemoveHandcuffs();
                response = "Игрок развязан";
                return true;
            }

            if (playerhitted == player)
            {
                response = "Ты не можешь связать самого себя";
                return false;
            }

            //playerhitted.Handcuff(player);
            playerhitted.Handcuff();
            playerhitted.DropItems();

            response = "Вы арестовали игрока";
            return true;
        }
    }
}
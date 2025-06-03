using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subclasses.Commands
{


    [CommandHandler(typeof(ClientCommandHandler))]
    public class FacilityControl : ICommand
    {
        public string Command { get; } = "FacilityControl";
        public string Description { get; } = "Управление комплексом";
        public string[] Aliases { get; } = { "FC", "Control" };
        private static bool _cassieIsActive = false;


        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            if (args.Count == 0)
            {
                response = "Использование команды:\n.FC LockGates\n " +
                        "UnlockGates\n" +
                        "scan\n" +
                        "kpplocdown enable/disable";
                return false;
            }

            Player player = Player.Get(sender);
            if (player == null || player.CurrentRoom == null || player.CurrentRoom.Type != RoomType.EzIntercom)
            {
                response = "Вы не в интеркоме";
                return false;
            }
            if (_cassieIsActive)
            {
                response = "C.A.S.S.I.E обрабатывает друго запрос";
                return false;
            }
            bool hasAccess = player.Role.Side == Side.Mtf || player.CurrentItem?.Type == ItemType.KeycardFacilityManager;

            if (!hasAccess)
            {
                _cassieIsActive = true;
                Cassie.Message("<color=#02FFCB>За</color><color=#02d6ac>п</color><color=#02bd97>рос</color> обрабатывается… <color=#ffffff00> pitch_0.5 .g2 .g2 . pitch_0.98 processing  Message . .g6 bell_end </color>", isNoisy: false, isSubtitles: true);
                Timing.CallDelayed(25f, () =>
                {
                    Cassie.Message("<b>Обработка запроса от <color=#ff9900>[ Зоны Содержания <size=0>а<size=25>35 ]...<size=0>............................................................................................................................................................................................................................<split><b>Попытка идентифицировать персонал...<size=0>................................................................................................................................................<split><b>Личность: <color=#8B0000>[ не установлена ]<size=0>...................................................................................................<split><b>Уровень Доступа: <color=#8B0000>[ неверный ]<size=0>..........................................................................................................................................<split><b>Идентификация <color=#8B0000>провалена<size=0>..................................................................................................................................................<split><split><b><color=#ffb841>Закрытие запроса…\r\n<SIZE=0> pitch_1,00 PROCESSING QUERY FROM CONTAINMENT SITE 35 . ATTEMPT TO identify personnel . . person . pitch_0.23 .G5 pitch_0,90 UNAUTHORIZED . ACCESS LEVEL . pitch_0.23 .G5 pitch_1,00 WRONG . . IDENTIFICATION IS FAILED .  . pitch_1,48 .G5 .G5 pitch_1,00 CLOSING QUERY\r\n", isNoisy: false, isSubtitles: true);
                    _cassieIsActive = false;
                });
                response = "<color=red>ОШИБКА АВТОРИЗАЦИИ</color>";
                return false;
            }

            string action = args.At(0).ToLower();

            switch (action)
            {
                case "lockgates":
                case "lock":
                    _cassieIsActive = true;
                   
                    Cassie.Message("<color=#02FFCB>За</color><color=#02d6ac>п</color><color=#02bd97>рос</color> обрабатывается… <color=#ffffff00> pitch_0.5 .g2 .g2 . pitch_0.98 processing  Message . .g6 bell_end </color>", isNoisy: false, isSubtitles: true);
                    Timing.CallDelayed(25f, () =>
                    {
                        Cassie.Message("Запрос принят… Процесс… Персонал <color=green>{Авторизован}</color>... Уровень допуска <color=green>{Принят}</color> Ворота Браво и Ворота Альфа закрыты и заблокированы <color=#ffffff00> Query accepted . processing . . pitch_0.92 .g3  pitch_0.94 .g3 pitch_0.92 .g3 pitch_0.9 .g2 .g4 . pitch_1 personnel . authorized . clearance level accepted . . Gate nato_B and Gate nato_A is now closed and locked down pitch_0.3 .g4 pitch_0.28 .g4  </color>", isNoisy: false, isSubtitles: true);
                        foreach (var gate in Door.List.Where(d => d.Type == DoorType.GateA || d.Type == DoorType.GateB))
                        {
                            gate.IsOpen = false;
                            gate.ChangeLock(DoorLockType.Isolation);
                        }
                        _cassieIsActive = false;
                    });
                    response = "<color=green>Ворота Браво и ворота Альфа были закрыты и заблокированы</color>";
                    return true;
                case "unlockgates":
                case "unlock":
                    _cassieIsActive = true;
                   
                    Cassie.Message("<color=#02FFCB>За</color><color=#02d6ac>п</color><color=#02bd97>рос</color> обрабатывается… <color=#ffffff00> pitch_0.5 .g2 .g2 . pitch_0.98 processing  Message . .g6 bell_end </color>", isNoisy: false, isSubtitles: true);
                    Timing.CallDelayed(25f, () =>
                    {
                        Cassie.Message("Запрос принят… Процесс… Персонал <color=green>{Авторизован}</color>... Уровень допуска <color=green>{Принят}</color> Ворота Браво и Ворота Альфа открыты, продолжайте эвакуацию. <color=#ffffff00> Query accepted . processing . . pitch_0.92 .g3  pitch_0.94 .g3 pitch_0.92 .g3 pitch_0.9 .g2 .g4 . pitch_1 personnel . authorized . clearance level accepted . . Gate nato_B and Gate nato_A is now open . . proceed evacuation pitch_0.80 .g5 .g5 pitch_0.50 .g3 </color>", isNoisy: false, isSubtitles: true);
                        foreach (var gate in Door.List.Where(d => d.Type == DoorType.GateA || d.Type == DoorType.GateB))
                        {
                            gate.ChangeLock(DoorLockType.None);
                        }
                        _cassieIsActive = false;
                    });
                    response = "<color=green>Ворота Браво и ворота Альфа были разблокированы</color>";
                    return true;
                case "scan":
                    _cassieIsActive = true;
                    Cassie.Message("<color=#02FFCB>За</color><color=#02d6ac>п</color><color=#02bd97>рос</color> обрабатывается… <color=#ffffff00> pitch_0.5 .g2 .g2 . pitch_0.98 processing  Message . .g6 bell_end </color>", isNoisy: false, isSubtitles: true);
                    Timing.CallDelayed(25f, () =>
                    {
                        Cassie.Message("Персонал <color=green>{Авторизован}</color> Уровень допуска <color=green>{Принят}</color>. Сканирование комплекса <color=green>{принято}</color>... Сканирование завершится через <color=red>{Время неизвестно}</color> <color=#ffffff00> pitch_0.50 .g3 pitch_0.80 .g5 pitch_1 Personnel . authorized . clearance level accepted . Scanning facility end in tminus pitch_0.85 unknown pitch_0.80 .g5 .g5 pitch_0.50 .g3 </color>", isNoisy: false, isSubtitles: true);
                        Timing.CallDelayed(60f, () =>
                        {
                            Cassie.Message($"Сканирование успешно завершилось. В комплексе <color=green>[{Player.List.Count(p => p.IsAlive)}]</color> людей и <color=red>[{Player.List.Count(p => p.IsAlive && p.Role.Side == Side.Scp)}]</color> SСP объектов. <color=#ffffff00> pitch_0.50 .g3 pitch_0.80 .g5 pitch_1 Scaning successfully completed . {Player.List.Count(p => p.IsAlive)} humans and {Player.List.Count(p => p.IsAlive && p.Role.Side == Side.Scp)} SCPsubjects in facility pitch_0.80 .g5 .g5 pitch_0.50 .g3 </color>", isNoisy: false, isSubtitles: true);

                            _cassieIsActive = false;
                        });
                    });
                    response = "<color=green>Сканирование запрошено</color>";
                    return true;
                case "kpplockdownenable":
                    _cassieIsActive = true;
                    Cassie.Message("<color=#02FFCB>За</color><color=#02d6ac>п</color><color=#02bd97>рос</color> обрабатывается… <color=#ffffff00> pitch_0.5 .g2 .g2 . pitch_0.98 processing  Message . .g6 bell_end </color>", isNoisy: false, isSubtitles: true);
                    Timing.CallDelayed(25f, () =>
                    {
                        Cassie.Message($"Персонал <color=green>{{Авторизован}}</color> Уровень допуска <color=green>{{Принят}}</color> Все КПП заблокированы.  <size=0> pitch_0.50 .g3 pitch_0.80 .g5 pitch_1 Personnel . authorized . clearance level accepted . all checkpoints doors is locked down  pitch_0.3 .g4 pitch_0.28 .g4 </size>\r\n", isNoisy: false, isSubtitles: true);
                        foreach (var kpp in Door.List.Where(d => d.Type == DoorType.CheckpointEzHczA || d.Type == DoorType.CheckpointEzHczB))
                        {
                            kpp.ChangeLock(DoorLockType.Isolation);
                        }
                        _cassieIsActive = false;
                    });
                    response = "<color=green>Локдаун КПП запрошен</color>";
                    return true;
                case "kpplockdowndisable":
                    _cassieIsActive = true;
                    Cassie.Message("<color=#02FFCB>За</color><color=#02d6ac>п</color><color=#02bd97>рос</color> обрабатывается… <color=#ffffff00> pitch_0.5 .g2 .g2 . pitch_0.98 processing  Message . .g6 bell_end </color>", isNoisy: false, isSubtitles: true);
                    Timing.CallDelayed(25f, () =>
                    {
                        Cassie.Message($"Персонал <color=green>{{Авторизован}}</color> Уровень допуска <color=green>{{Принят}}</color> Все КПП Разблокированы.  <size=0> pitch_0.50 .g3 pitch_0.80 .g5 pitch_1 Personnel . authorized . clearance level accepted . all checkpoints doors is opened pitch_0.80 .g5 .g5 pitch_0.50 .g3 </size>\r\n", isNoisy: false, isSubtitles: true);
                        foreach (var kpp in Door.List.Where(d => d.Type == DoorType.CheckpointEzHczA || d.Type == DoorType.CheckpointEzHczB))
                        {
                            kpp.ChangeLock(DoorLockType.None);
                        }
                        _cassieIsActive = false;
                    });
                    response = "<color=green>Снятие локдауна КПП запрошено</color>";
                    return true;
                default:
                    response = "Использование команды:\n.FC LockGates\n " +
                        "UnlockGates\n" +
                        "scan\n" +
                        "kpplocdownenable/kpplocdowndisable";
                    return false;
            }
           
        }

    }
}

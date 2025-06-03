using Exiled.API.Features;
using HarmonyLib;
using LabApi.Events.Arguments.PlayerEvents;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using player = Exiled.Events.Handlers.Player;

namespace Subclasses
{

    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "Sub";
        public override string Name => "Subclasses";
        public override string Author => "OpasniDed";
        public static Plugin plugin;
        public static EventHandlers EventHandler;
        public NameManager NameManager { get; private set; }
        public static RadioAnon radioAnon;
        public HashSet<Exiled.API.Features.Player> ManualRoleChanges { get; } = new HashSet<Exiled.API.Features.Player>();
        public bool TeslaEnabled = true;
        public float TeslaActivationTime = 0.75f;
        public float TeslaCooldown = 1f;




        public override void OnEnabled()
        {

            plugin = this;
            EventHandler = new EventHandlers(this);
            radioAnon = new RadioAnon();
            NameManager = new NameManager(Config);


            player.TriggeringTesla += EventHandler.TeslaTrigger;
            player.ChangingRole += EventHandler.RoleChange;
            player.Verified += EventHandler.PlayerVerified;
            player.Left += EventHandler.OnPlayerLeft;
            player.Transmitting += radioAnon.OnTransmitting;
            player.IntercomSpeaking += radioAnon.OnTransmitting;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            
            Log.Debug($"Загружено {Config.Subclasses.Count} подклассов");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            player.TriggeringTesla -= EventHandler.TeslaTrigger;
            player.ChangingRole -= EventHandler.RoleChange;
            player.Verified -= EventHandler.PlayerVerified;
            player.Left -= EventHandler.OnPlayerLeft;
            player.Transmitting -= radioAnon.OnTransmitting;
            player.IntercomSpeaking -= radioAnon.OnTransmitting;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;


            plugin = null;
            EventHandler = null;
            NameManager = null;
            radioAnon = null;

            base.OnDisabled();
        }

        public void OnRoundStarted()
        {
            Cassie.Message("<split><b>Система <color=#4DFFB8>C.A.S.S.I.E.</color>: [ <i><color=#d03f13>Онлайн</color> и находится в <color=orange>Режиме ожидания</color></i> ]<size=0>............................................................................<split><b>Доброй ночи всему персоналу и добро пожаловать в [ <color=#ff9900>Зону-35</color> ]<size=0>.............................................................................................................<split><b>Внимание всему <color=#7FFF94>медицинскому</color> и <color=#ffb841>научному</color> персоналу находящиеся во <color=#FFE95B>Входной Зоне</color> и <color=#FFB85B>Легкой Зоне Содержания</color>: [ <i>Просим начать вашу\r\nработу</i> ]<size=0>................................................................................................................................<split><split><b>Внимание всем сотрудникам <color=#696969>Службы Безопасности</color>: [ <i>Немедленно пройти к интеркому и ожидать своего <color=#006db9>Начальника </color>или <color=#0066f1>Сержанта</i></color> ]<size=0>....................................................................<split><split><b><color=#7FFF94>Хорошего</color> рабочего дня и <color=#FFB85B>помните</color>...<size=0>........................................<split><b>S - Обезопасить.<size=0>........................<split><b>С – Удержать.<split><b>Р – Сохранить.\r\n<size=0> . . pitch_0,80 jam_080_4 .G2 pitch_1 CASSIE SYSTEM . .G5 . Online and is located Standby mode . . Hello and Welcome to all personnel in Site 30 5 . . Attention to medical . and science personnel located in Entrance and Light Containment Zone . Please get started your job . . yield_01,3 Security services . . jam_27_3 .G4 Immediately report to INTERCOM and wait your Head or Senior . . pitch_2.5 .G5 .G5 pitch_3.0 .G3 pitch_4.5 .G5 .G4 .G3 .G5 pitch_1.10 . . GOOD WORK DAY AND jam_15_2 REMIND Pitch_0.95 jam_27_3 .g5 . Pitch_1.00 . S . Pitch_0.95 Secure . Pitch_1.00 C . PITCH_0.9 Contain . Pitch_1.00 P . PITCH_0.85 jam_20_2 PROTECT . .\r\n", isNoisy: false, isSubtitles: true);
            TeslaCooldown = 1f;
            TeslaActivationTime = 0.75f;
            TeslaEnabled = true;
        }

    }
}

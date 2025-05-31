using Exiled.API.Features;
using LabApi.Events.Arguments.PlayerEvents;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Player = Exiled.Events.Handlers.Player;

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


       

        public override void OnEnabled()
        {

            plugin = this;
            EventHandler = new EventHandlers(this);
            radioAnon = new RadioAnon();
            NameManager = new NameManager(Config);

            Player.TriggeringTesla += EventHandler.TeslaTrigger;
            Player.ChangingRole += EventHandler.RoleChange;
            Player.Verified += EventHandler.PlayerVerified;
            Player.Left += EventHandler.OnPlayerLeft;
            Player.Transmitting += radioAnon.OnTransmitting;
            Player.IntercomSpeaking += radioAnon.OnTransmitting;

            Log.Debug($"Загружено {Config.Subclasses.Count} подклассов");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Player.TriggeringTesla -= EventHandler.TeslaTrigger;
            Player.ChangingRole -= EventHandler.RoleChange;
            Player.Verified -= EventHandler.PlayerVerified;
            Player.Left -= EventHandler.OnPlayerLeft;
            Player.Transmitting -= radioAnon.OnTransmitting;
            Player.IntercomSpeaking -= radioAnon.OnTransmitting;

            plugin = null;
            EventHandler = null;
            NameManager = null;
            radioAnon = null;

            base.OnDisabled();
        }

    }
}

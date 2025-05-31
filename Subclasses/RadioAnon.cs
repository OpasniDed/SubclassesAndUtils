using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Player;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using PluginAPI.Events;

namespace Subclasses
{
    public class RadioAnon
    {

        private readonly Dictionary<Player, string> replacedNicknames = new Dictionary<Player, string>();

        public IEnumerator<float> OnTransmitting(IPlayerEvent ev)
        {
            if (!(ev.Player.IsTransmitting || Intercom.Speaker == ev.Player) || replacedNicknames.ContainsKey(ev.Player)) yield break;

            replacedNicknames[ev.Player] = ev.Player.CustomName;

            string name = "164.55";

            ev.Player.CustomName = name;

            yield return Timing.WaitUntilTrue(() => !ev.Player.IsTransmitting && Intercom.Speaker != ev.Player);

            if (replacedNicknames[ev.Player] == ev.Player.Nickname) ev.Player.CustomName = null;
            else ev.Player.CustomName = replacedNicknames[ev.Player];

            replacedNicknames.Remove(ev.Player);
        }
    }
}

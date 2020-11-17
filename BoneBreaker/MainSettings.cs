using Exiled.API.Features;

namespace BoneBreaker
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(BoneBreaker);
        public SetEvents SetEvents { get; set; }

        public override void OnEnabled()
        {
            SetEvents = new SetEvents();
            Exiled.Events.Handlers.Player.Hurting += SetEvents.OnHurting;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += SetEvents.OnSendingConsoleCommad;
            Exiled.Events.Handlers.Player.MedicalItemUsed += SetEvents.OnMedicalItemUsed;
            Exiled.Events.Handlers.Server.WaitingForPlayers += SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Player.Spawning += SetEvents.OnSpawning;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Hurting -= SetEvents.OnHurting;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= SetEvents.OnSendingConsoleCommad;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= SetEvents.OnMedicalItemUsed;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Player.Spawning -= SetEvents.OnSpawning;
            Log.Info(Name + " off");
        }
    }
}
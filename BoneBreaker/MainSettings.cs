using EXILED;

namespace BoneBreaker
{
    public class MainSettings : Plugin
    {
        public override string getName => nameof(BoneBreaker);
        public SetEvents SetEvents { get; set; }

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Events.PlayerHurtEvent += SetEvents.OnPlayerHurt;
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.PlayerSpawnEvent += SetEvents.OnPlayerSpawn;
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.UsedMedicalItemEvent += SetEvents.OnUsedMedicalItem;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Events.PlayerHurtEvent -= SetEvents.OnPlayerHurt;
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.PlayerSpawnEvent -= SetEvents.OnPlayerSpawn;
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.UsedMedicalItemEvent -= SetEvents.OnUsedMedicalItem;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}
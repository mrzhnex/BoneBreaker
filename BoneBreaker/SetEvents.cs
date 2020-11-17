using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;
using UnityEngine;

namespace BoneBreaker
{
    public class SetEvents
    {
        internal void OnHurting(HurtingEventArgs ev)
        {
            if (ev.DamageType == DamageTypes.Falldown && ev.Target.Team != Team.SCP && ev.Target.Role != RoleType.Tutorial && ev.Target.GameObject.GetComponent<BoneBreakerComponent>() == null)
            {
                if (Global.IsFullRp)
                {
                    ev.Target.GameObject.AddComponent<BoneBreakerComponent>();
                    ev.Target.GameObject.GetComponent<BoneBreakerComponent>().previosPosition = ev.Target.GameObject.transform.position;
                    ev.Target.GameObject.GetComponent<BoneBreakerComponent>().distance = 0.55f;
                    ev.Target.ClearBroadcasts();
                    ev.Target.Broadcast(15, "<color=#ff0000>Вы сломали ногу и не можете нормально ходить</color>", Broadcast.BroadcastFlags.Monospaced);
                }
                else
                {
                    ev.Target.GameObject.AddComponent<BoneBreakerComponent>();
                    ev.Target.GameObject.GetComponent<BoneBreakerComponent>().previosPosition = ev.Target.GameObject.transform.position;
                    ev.Target.GameObject.GetComponent<BoneBreakerComponent>().distance = 1.4f;
                    ev.Target.ClearBroadcasts();
                    ev.Target.Broadcast(15, "<color=#ff0000>Вы сломали ногу и не можете нормально бегать</color>", Broadcast.BroadcastFlags.Monospaced);
                }
            }
        }

        internal void OnSendingConsoleCommad(SendingConsoleCommandEventArgs ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }

            if (ev.Name.ToLower() == "hit")
            {
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._iscp;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<CooldownComponent>() != null)
                {
                    ev.ReturnMessage = Global._cooldown + ev.Player.GameObject.GetComponent<CooldownComponent>().cooldown.ToString();
                    return;
                }
                ev.Player.GameObject.AddComponent<CooldownComponent>();
                ev.ReturnMessage = Global._starthit;
                return;
            }
        }

        internal void OnSpawning(SpawningEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<BoneBreakerComponent>())
                Object.Destroy(ev.Player.GameObject.GetComponent<BoneBreakerComponent>());
        }

        internal void OnMedicalItemUsed(UsedMedicalItemEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<BoneBreakerComponent>() != null)
            {
                if (ev.Item == ItemType.Medkit)
                {
                    if (ev.Player.GameObject.GetComponent<BoneBreakerComponent>().distance == 0.55f)
                    {
                        ev.Player.GameObject.GetComponent<BoneBreakerComponent>().distance = 1.4f;
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(15, "Вы чувствуете, что можете нормально ходить, но бегать явно не сможете", Broadcast.BroadcastFlags.Monospaced);
                        return;
                    }
                }
                else if (ev.Item == ItemType.SCP500)
                {
                    Object.Destroy(ev.Player.GameObject.GetComponent<BoneBreakerComponent>());
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(15, "Вы чувствуете, что можете нормально ходить и бегать", Broadcast.BroadcastFlags.Monospaced);
                    return;
                }
            }
        }

        public void OnRoundStarted()
        {
            Global.can_use_commands = true;
            GameObject.FindWithTag("FemurBreaker").AddComponent<HurtPlayerBoneBreaker>();
        }

        public void OnWaitingForPlayers()
        {
            Global.IsFullRp = true;
            Log.Info(nameof(Global.IsFullRp) + ": " + Global.IsFullRp);
            Global.can_use_commands = false;
            Global.hit_damage = 10;
            Global.distance_to_hit = 2f;
        }
    }
}
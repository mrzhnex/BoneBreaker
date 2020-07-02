using EXILED;
using EXILED.Extensions;
using System.Linq;
using UnityEngine;

namespace BoneBreaker
{
    public class SetEvents
    {
        public void OnCallCommand(ConsoleCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }

            if (ev.Command.ToLower() == "hit")
            {
                if (ev.Player.GetTeam() == Team.SCP)
                {
                    ev.ReturnMessage = Global._iscp;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<CooldownComponent>() != null)
                {
                    ev.ReturnMessage = Global._cooldown + ev.Player.gameObject.GetComponent<CooldownComponent>().cooldown.ToString();
                    return;
                }
                ev.Player.gameObject.AddComponent<CooldownComponent>();
                ev.ReturnMessage = Global._starthit;
                return;
            }

            if (Global.IsFullRp && ev.Command.ToLower() == "healleg")
            {
                if (ev.Player.gameObject.GetComponent<BoneBreakerComponent>() != null)
                {
                    if (ev.Player.GetAllItems().Where(x => x.id == ItemType.Medkit).FirstOrDefault() != default)
                    {
                        if (ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance == 0.55f)
                        {
                            for (int i = 0; i < ev.Player.inventory.items.Count; i++)
                            {
                                if (ev.Player.inventory.items[i].id == ItemType.Medkit)
                                {
                                    ev.Player.inventory.items.Remove(ev.Player.inventory.items[i]);
                                    break;
                                }
                            }
                            ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance = 1.4f;
                            ev.ReturnMessage = "Вы чувствуете, что можете нормально ходить, но бегать явно не сможете";
                            return;
                        }
                        else if (ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance == 1.4f)
                        {
                            ev.ReturnMessage = "Вы не сможете в таких условиях \"вылечить ногу\"...";
                            return;
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = "У вас нет с собой необходимых предметов";
                        return;
                    }
                }
                else
                {
                    ev.ReturnMessage = "У вас не сломаны ноги...";
                    return;
                }
            }
        }

        public void OnUsedMedicalItem(UsedMedicalItemEvent ev)
        {
            if (!Global.IsFullRp && ev.Player.gameObject.GetComponent<BoneBreakerComponent>() != null && (ev.ItemType == ItemType.Medkit || ev.ItemType == ItemType.SCP500))
            {
                if (ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance == 0.55f)
                {
                    ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance = 1.4f;
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(15, "Вы чувствуете, что можете нормально ходить, но бегать явно не сможете", true);
                    return;
                }
                else if (ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance == 1.4f)
                {
                    Object.Destroy(ev.Player.gameObject.GetComponent<BoneBreakerComponent>());
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(15, "Вы чувствуете, что можете нормально ходить и бегать", true);
                    return;
                }
            }
        }

        public void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<BoneBreakerComponent>())
                Object.Destroy(ev.Player.gameObject.GetComponent<BoneBreakerComponent>());
        }

        public void OnPlayerHurt(ref PlayerHurtEvent ev)
        {
            if (ev.DamageType == DamageTypes.Falldown && ev.Player.GetTeam() != Team.SCP && ev.Player.GetRole() != RoleType.Tutorial && ev.Player.gameObject.GetComponent<BoneBreakerComponent>() == null)
            {
                if (Global.IsFullRp)
                {
                    ev.Player.gameObject.AddComponent<BoneBreakerComponent>();
                    ev.Player.gameObject.GetComponent<BoneBreakerComponent>().previosPosition = ev.Player.gameObject.transform.position;
                    ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance = 0.55f;
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(15, "<color=#ff0000>Вы сломали ногу и не можете нормально ходить</color>", true);
                }
                else
                {
                    ev.Player.gameObject.AddComponent<BoneBreakerComponent>();
                    ev.Player.gameObject.GetComponent<BoneBreakerComponent>().previosPosition = ev.Player.gameObject.transform.position;
                    ev.Player.gameObject.GetComponent<BoneBreakerComponent>().distance = 1.4f;
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(15, "<color=#ff0000>Вы сломали ногу и не можете нормально бегать</color>", true);
                }
            }
        }

        public void OnRoundStart()
        {
            Global.can_use_commands = true;
            GameObject.FindWithTag("FemurBreaker").AddComponent<HurtPlayerBoneBreaker>();
        }

        public void OnWaitingForPlayers()
        {
            try
            {
                Global.IsFullRp = Plugin.Config.GetBool("IsFullRp");
            }
            catch (System.Exception ex)
            {
                Log.Info("Catch an exception while getting boolean value from config file: " + ex.Message);
                Global.IsFullRp = false;
            }
            Log.Info(nameof(Global.IsFullRp) + ": " + Global.IsFullRp);
            Global.can_use_commands = false;
            Global.hit_damage = 10;
            Global.distance_to_hit = 2f;
        }
    }
}
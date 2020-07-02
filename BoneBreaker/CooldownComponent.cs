using UnityEngine;
using RemoteAdmin;
using EXILED.Extensions;

namespace BoneBreaker
{
    class CooldownComponent : MonoBehaviour
    {
        public float cooldown = Global.cooldown;
        private float timer = 0f;
        private readonly float timeIsUp = 1f;
        private ReferenceHub Attacker;

        public void Start()
        {
            Attacker = Player.GetPlayer(gameObject);
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                cooldown -= timeIsUp;
                if (cooldown == (Global.cooldown - 1f))
                {
                    if (Physics.Raycast((gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward * 1.01f) + gameObject.transform.position, gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, Global.distance_to_hit))
                    {
                        if (hit.transform.GetComponent<QueryProcessor>() == null)
                        {
                            Attacker.ClearBroadcasts();
                            Attacker.Broadcast(5, "<color=#ff2400>" + Global._notlook + "</color>", true);
                        }
                        else
                        {
                            ReferenceHub p = Player.GetPlayer(hit.transform.GetComponent<QueryProcessor>().PlayerId);
                            if (p.GetTeam() == Team.SCP)
                            {
                                Attacker.ClearBroadcasts();
                                Attacker.Broadcast(5, "<color=#ff2400>" + Global._targetscp + "</color>", true);
                            }
                            else
                            {
                                p.gameObject.GetComponent<Scp173PlayerScript>().CallRpcSyncAudio();
                                p.playerStats.HurtPlayer(new PlayerStats.HitInfo(Global.hit_damage, Attacker.nicknameSync.Network_myNickSync, DamageTypes.Wall, Attacker.GetPlayerId()), p.gameObject);
                                
                                p.ClearBroadcasts();
                                p.Broadcast(10, "<color=#ff0000>*Вы чувствуете резкую боль от удара игрока " + Attacker.nicknameSync.Network_myNickSync + "*</color>", true);
                                Attacker.ClearBroadcasts();
                                Attacker.Broadcast(10, "<color=#ff0000>" + Global._successhit1 + p.nicknameSync.Network_myNickSync + Global._successhit2 + Global.hit_damage + "</color>", true);
                            }
                        }
                    }
                    else
                    {
                        Attacker.ClearBroadcasts();
                        Attacker.Broadcast(5, "<color=#ff2400>" + Global._notlook + "</color>", true);
                    }
                }
            }

            if (cooldown <= 0)
            {
                Destroy(gameObject.GetComponent<CooldownComponent>());
            }
        }
    }
}
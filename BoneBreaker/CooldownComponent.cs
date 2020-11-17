using UnityEngine;
using RemoteAdmin;
using Exiled.API.Features;

namespace BoneBreaker
{
    class CooldownComponent : MonoBehaviour
    {
        public float cooldown = Global.cooldown;
        private float timer = 0f;
        private readonly float timeIsUp = 1f;
        private Player Attacker;

        public void Start()
        {
            Attacker = Player.Get(gameObject);
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
                    if (Physics.Raycast((Attacker.CameraTransform.forward * 1.01f) + gameObject.transform.position, Attacker.CameraTransform.forward, out RaycastHit hit, Global.distance_to_hit))
                    {
                        if (hit.transform.GetComponent<QueryProcessor>() == null)
                        {
                            Attacker.ClearBroadcasts();
                            Attacker.Broadcast(5, "<color=#ff2400>" + Global._notlook + "</color>", Broadcast.BroadcastFlags.Monospaced);
                        }
                        else
                        {
                            Player p = Player.Get(hit.transform.GetComponent<QueryProcessor>().PlayerId);
                            if (p.Team == Team.SCP)
                            {
                                Attacker.ClearBroadcasts();
                                Attacker.Broadcast(5, "<color=#ff2400>" + Global._targetscp + "</color>", Broadcast.BroadcastFlags.Monospaced);
                            }
                            else
                            {
                                p.GameObject.GetComponent<Scp173PlayerScript>().CallRpcSyncAudio();
                                p.Hurt(Global.hit_damage, Attacker, DamageTypes.Wall);
                                
                                p.ClearBroadcasts();
                                p.Broadcast(10, "<color=#ff0000>*Вы чувствуете резкую боль от удара игрока " + Attacker.Nickname + "*</color>", Broadcast.BroadcastFlags.Monospaced);
                                Attacker.ClearBroadcasts();
                                Attacker.Broadcast(10, "<color=#ff0000>" + Global._successhit1 + p.Nickname + Global._successhit2 + Global.hit_damage + "</color>", Broadcast.BroadcastFlags.Monospaced);
                            }
                        }
                    }
                    else
                    {
                        Attacker.ClearBroadcasts();
                        Attacker.Broadcast(5, "<color=#ff2400>" + Global._notlook + "</color>", Broadcast.BroadcastFlags.Monospaced);
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
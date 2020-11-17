using Exiled.API.Features;
using UnityEngine;

namespace BoneBreaker
{
    public class HurtPlayerBoneBreaker : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float timeIsUp = 0.2f;

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > timeIsUp)
            {
                Timer = 0.0f;
                foreach (Player p in Player.List)
                {
                    GameObject gameobj = p.GameObject;
                    if (gameobj.GetComponent<BoneBreakerComponent>() == null)
                        continue;
                    if (Vector3.Distance(gameobj.transform.position, gameobj.GetComponent<BoneBreakerComponent>().previosPosition) >= gameobj.GetComponent<BoneBreakerComponent>().distance)
                    {
                        p.Hurt(gameobj.GetComponent<BoneBreakerComponent>().damage, p, DamageTypes.Wall);

                        p.ClearBroadcasts();
                        p.Broadcast(5, "<color=#ff0000>Вы чувствуете сильную боль. Вероятно, у вас сломана нога</color>", Broadcast.BroadcastFlags.Monospaced);
                    }
                    gameobj.GetComponent<BoneBreakerComponent>().previosPosition = gameobj.transform.position;
                }
            }           
        }
    }
}
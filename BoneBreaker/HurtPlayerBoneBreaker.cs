using EXILED.Extensions;
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
                foreach (ReferenceHub p in Player.GetHubs())
                {
                    GameObject gameobj = p.gameObject;
                    if (gameobj.GetComponent<BoneBreakerComponent>() == null)
                        continue;
                    if (Vector3.Distance(gameobj.transform.position, gameobj.GetComponent<BoneBreakerComponent>().previosPosition) >= gameobj.GetComponent<BoneBreakerComponent>().distance)
                    {
                        p.playerStats.HurtPlayer(new PlayerStats.HitInfo(gameobj.GetComponent<BoneBreakerComponent>().damage, p.nicknameSync.Network_myNickSync, DamageTypes.Wall, p.GetPlayerId()), gameobj);

                        p.ClearBroadcasts();
                        p.Broadcast(5, "<color=#ff0000>Вы чувствуете сильную боль. Вероятно, у вас сломана нога</color>", true);
                    }
                    gameobj.GetComponent<BoneBreakerComponent>().previosPosition = gameobj.transform.position;
                }
            }           
        }
    }
}
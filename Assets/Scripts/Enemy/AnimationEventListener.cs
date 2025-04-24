using UnityEngine;
using UnityEngine.Events;

namespace EnemySpace
{
    public class AnimationEventListener : MonoBehaviour
    {
        public UnityEvent Attack;

        public void EventAttack()
        {
            Attack.Invoke();
        }
    }
}
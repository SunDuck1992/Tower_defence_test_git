using UnityEngine;

namespace PlayerSpace
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public bool IsShooting { get; set; }

        public void PlayMoveAnimation(float value)
        {
            if (IsShooting)
            {
                _animator.SetFloat(AnimationConst.Move, value);
            }
            else
            {
                _animator.SetFloat(AnimationConst.Move, value < 0 ? 1 : value);
            }
        }
    }
}
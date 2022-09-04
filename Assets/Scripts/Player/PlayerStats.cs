using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5;
        public float Speed => _speed;
    }
}

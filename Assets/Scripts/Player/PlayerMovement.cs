using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerStats _playerStats;

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
        }

        public void Move(Vector3 direction)
        {
            if (direction == Vector3.zero) return;
            transform.position += _playerStats.Speed * Time.deltaTime * direction;
        }
    }
}

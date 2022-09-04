using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private Camera _camera;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _camera = Camera.main;
        }

        private void Update()
        {
            UpdateMovement();
            UpdateRotation();

            //Shooting
            if (Input.GetMouseButton(0))
                GetComponent<Weapon>().Shoot();
        }

        private void UpdateMovement()
        {
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            _playerMovement.Move(movement);
        }

        private void UpdateRotation()
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 screenPoint = _camera.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}

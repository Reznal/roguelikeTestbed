using System.Collections.Generic;
using UnityEngine;
using Weapons.ProjectileEffects;

namespace Weapons.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private const float DestructionTime = 12f;
    
        [SerializeField]
        private float _speed;

        private List<AProjectileEffect> _effects = new List<AProjectileEffect>();
        private Transform _transform;
    
        private void Awake()
        {
            _transform = transform;
        
            //Destroy projectile in x seconds if it has not been destroyed
            Destroy(gameObject, DestructionTime / _speed);
        }

        public void SetEffects(List<AProjectileEffect> effects) => _effects = new List<AProjectileEffect>(effects);
        private void Update() => _transform.position += _speed * Time.deltaTime * _transform.right;

        private void OnDestroy()
        {
            foreach (AProjectileEffect effect in _effects) 
                effect.MakeEffect(this);
        }
    }
}

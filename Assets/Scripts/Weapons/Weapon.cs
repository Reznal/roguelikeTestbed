using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Weapons.Attacks;
using Weapons.ProjectileEffects;
using Weapons.Projectiles;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private WeaponScriptable _weapon;
        
        private float _timeBetweenShots;
        private float _nextShot;

        private AWeaponAttack _currentAttack;
    
        //Effects
        [SerializeField]
        private List<AProjectileEffect> _effects;

        private void Awake() => SetWeapon(_weapon);

        public void Shoot()
        {
            if (Time.time < _nextShot) return;
        
            _currentAttack.Attack(this);
            _nextShot = Time.time + _timeBetweenShots;
        }

        public GameObject SpawnBullet(Vector3 direction)
        {
            GameObject obj = Instantiate(_weapon.BulletPrefab, transform.position, quaternion.identity);
        
            //Set projectiles effects if the weapon has any
            if(_effects.Count > 0)
                obj.GetComponent<Projectile>().SetEffects(_effects);
        
        
            return obj;
        }

        public void SetWeapon(WeaponScriptable value)
        {
            _weapon = value;
            SetAttack(_weapon.Attack);
            _timeBetweenShots = 60 / _weapon.ShotsPerMinute;
            
            _effects = new List<AProjectileEffect>(_weapon.Effects);
        }

        public void AddEffect(AProjectileEffect value) => _effects.Add(value);
        public void SetAttack(AWeaponAttack value) => _currentAttack = value;
    }
}
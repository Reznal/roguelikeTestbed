using System;
using UnityEngine;
using Weapons;
using Weapons.Attacks;
using Weapons.ProjectileEffects;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerStats))]
    public class Player : MonoBehaviour
    {
        private Weapon _weapon;

        private void Awake() => _weapon = GetComponent<Weapon>();

        public void ApplyPickup(Pickup pickup)
        {
            switch (pickup.PickupType)
            {
                case PickupType.Weapon:
                    if(pickup.ScriptableValue is WeaponScriptable weaponValue)
                        _weapon.SetWeapon(weaponValue);
                    break;
                case PickupType.Effect:
                    if(pickup.ScriptableValue is AProjectileEffect effectValue)
                        _weapon.AddEffect(effectValue);
                    break;
                case PickupType.Attack:
                    if(pickup.ScriptableValue is AWeaponAttack attackValue)
                        _weapon.SetAttack(attackValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

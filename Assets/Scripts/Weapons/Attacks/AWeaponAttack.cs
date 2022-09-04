using UnityEditor;
using UnityEngine;

namespace Weapons.Attacks
{
    public abstract class AWeaponAttack : ScriptableObject
    {
        public abstract void Attack(Weapon weapon);
    }
}
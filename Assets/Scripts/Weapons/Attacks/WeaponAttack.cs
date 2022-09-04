using UnityEngine;

namespace Weapons.Attacks
{
    [CreateAssetMenu(fileName = "WeaponAttack", menuName = "Attacks/WeaponAttack")]
    public class WeaponAttack : AWeaponAttack
    {
        public override void Attack(Weapon weapon)
        {
            GameObject obj = weapon.SpawnBullet(weapon.transform.right);
            obj.transform.rotation = weapon.transform.rotation;
        }
    }
}
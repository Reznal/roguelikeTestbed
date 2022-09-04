using UnityEngine;

namespace Weapons.Attacks
{
    [CreateAssetMenu(fileName = "TripleWeaponAttack", menuName = "Attacks/TripleWeaponAttack")]
    public class TripleWeaponAttack : AWeaponAttack
    {
        private const float AngleBetweenBullet = 15;
        
        public override void Attack(Weapon weapon)
        {
            Vector3 rotation = weapon.transform.rotation.eulerAngles - new Vector3(0, 0, AngleBetweenBullet);
            
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = weapon.SpawnBullet(weapon.transform.right);
                obj.transform.rotation = Quaternion.Euler(rotation + new Vector3(0, 0, AngleBetweenBullet * i));
            }
        }
    }
}
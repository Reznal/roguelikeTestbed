using UnityEngine;
using Weapons.Projectiles;

namespace Weapons.ProjectileEffects
{
    [CreateAssetMenu(fileName = "ProjectileEffect", menuName = "Effects/ProjectileEffect")]
    public class ProjectileEffect : AProjectileEffect
    {
        public override void MakeEffect(Projectile projectile)
        {
            Debug.Log("Effect caused");
        }
    }
}
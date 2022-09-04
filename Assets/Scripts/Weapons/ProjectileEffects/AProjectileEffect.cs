using UnityEngine;
using Weapons.Projectiles;

namespace Weapons.ProjectileEffects
{
    public abstract class AProjectileEffect : ScriptableObject
    {
        public abstract void MakeEffect(Projectile projectile);
    }
}
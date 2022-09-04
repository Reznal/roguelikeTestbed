using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Weapons.Attacks;
using Weapons.ProjectileEffects;

[CreateAssetMenu(fileName = "New Weapon Scriptable", menuName = "Weapons/Scriptable")]
public class WeaponScriptable : ScriptableObject
{
    [SerializeField]
    private GameObject _bulletPrefab;
    
    [SerializeField]
    private float _shotsPerMinute;
    
    [SerializeField]
    private AWeaponAttack _attack;
    
    [SerializeField]
    private List<AProjectileEffect> _effects;

    public GameObject BulletPrefab => _bulletPrefab;
    public float ShotsPerMinute => _shotsPerMinute;
    public AWeaponAttack Attack => _attack;
    public List<AProjectileEffect> Effects => _effects;
}

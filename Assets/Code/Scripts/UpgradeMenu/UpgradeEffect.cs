using UnityEngine;

[System.Serializable]
public class UpgradeEffect
{
    public enum EffectType
    {
        CriticalChance,
        CriticalDamage,
        Damage,
        BulletPerSecond,
        Range
    }

    public EffectType type;
    public float value; // Art�� miktar� (�rn: +3.0f)
}
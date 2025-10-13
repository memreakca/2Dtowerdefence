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
    public float value; // Artýþ miktarý (örn: +3.0f)
}
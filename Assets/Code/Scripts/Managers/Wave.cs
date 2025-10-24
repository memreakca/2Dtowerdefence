using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class Wave 
{
    public WaveType WaveType;
    public List<WaveIndex> waveIndexes;
}

public enum WaveType
{
    Normal,
    Swarm,
    Boss
}
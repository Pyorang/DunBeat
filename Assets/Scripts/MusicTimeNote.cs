using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicTimeNote", menuName = "ScriptableObject/MusicTimeNote", order = int.MaxValue)]
public class MusicTimeNote : ScriptableObject
{
    public int[] musicBpmInfo;
}

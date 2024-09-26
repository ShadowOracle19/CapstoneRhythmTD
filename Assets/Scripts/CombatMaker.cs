using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptableObjects/EncounterCreator")]
public class CombatMaker : ScriptableObject
{
    public int enemyTotal = 0;
    public int encounterBPM = 0;
    public AudioClip levelSong;
}

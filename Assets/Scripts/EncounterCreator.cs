using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptableObjects/EncounterCreator")]
public class EncounterCreator : ScriptableObject
{
    public TextAsset introDialogue;

    public CombatMaker combatEncounter;

    public TextAsset endDialogue;

}

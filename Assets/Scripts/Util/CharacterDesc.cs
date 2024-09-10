using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDesc", menuName = "ScriptableObject/ChacracterDesc", order = 110)]
public class CharacterDesc : ScriptableObject
{
    public List<string> characterName;
    public List<string> characterDesc;
}
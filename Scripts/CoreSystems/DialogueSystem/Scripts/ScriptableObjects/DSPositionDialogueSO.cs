using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;
    public class DSPositionDialogueSO : DSDialogueSO{
        [field: SerializeField] public List<DSPositionChoiceData> Choices { get; set; }
        [field: SerializeField] public List<string> NewPositions { get; set; }
        public void Initialize(string name, DSDialogueType type, List<DSPositionChoiceData> choices, List<string> newPositions){
            DialogueName = name;
            DialogueType = type;
            Choices = choices;
            NewPositions = newPositions;
        }
    }
}   

using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;
    public class DSMasterDialogueSO : DSDialogueSO{
        [field: SerializeField] public List<DSSingleChoiceData> Choices { get; set; }
        [field: SerializeField] public List<string> InitialPositions { get; set; }
        [field: SerializeField] public List<string> Conversants { get; set; }
        public void Initialize(string name, DSDialogueType type, List<DSSingleChoiceData> choices, List<string> newPosition, List<string> conversants){
            DialogueName = name;
            DialogueType = type;
            Choices = choices;
            Conversants = conversants;
            InitialPositions = newPosition;       
        }
    }
}   

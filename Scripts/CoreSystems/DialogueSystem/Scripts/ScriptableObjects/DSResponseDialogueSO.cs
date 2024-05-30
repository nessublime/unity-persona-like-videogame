using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;
    public class DSResponseDialogueSO : DSDialogueSO{
        [field: SerializeField] public List<DSSingleChoiceData> Choices { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public string Expression { get; set; }
        [field: SerializeField] public string Stat { get; set; }
        [field: SerializeField] public string StatEXP { get; set; }
        public void Initialize(string name, DSDialogueType type, List<DSSingleChoiceData> choices, string text, string expression, string stat, string statEXP){
            DialogueName = name;
            DialogueType = type;
            Choices = choices;
            Text = text;
            Expression = expression;
            Stat = stat;
            StatEXP = statEXP;
        }
    }
}   

using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;
    public class DSMultipleChoiceDialogueSO : DSDialogueSO{
        [field: SerializeField] public List<DSMultiChoiceData> Choices { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public string Conversant { get; set; }
        [field: SerializeField] public string Expression { get; set; }
        public void Initialize(string name, DSDialogueType type, List<DSMultiChoiceData> choices, string text, string conversant, string expression){
            DialogueName = name;
            DialogueType = type;
            Choices = choices;
            Text = text;
            Conversant = conversant;
            Expression = expression;
        }
    }
}   

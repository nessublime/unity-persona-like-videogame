using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;
    public class DSPartyDialogueSO : DSDialogueSO{
        [field: SerializeField] public List<DSPartyChoiceData> Choices { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public string Priority { get; set; }
        [field: SerializeField] public string Expression { get; set; }
        public void Initialize(string name, DSDialogueType type, List<DSPartyChoiceData> choices, string text, string priority, string expression){
            DialogueName = name;
            DialogueType = type;
            Choices = choices;
            Text = text;
            Priority = priority;
            Expression = expression;
        }
    }
}   

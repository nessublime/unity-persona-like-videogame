using System;
using UnityEngine;

namespace DS.Data
{
    using ScriptableObjects;

    [Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }
    }
    [Serializable]
    public class DSSingleChoiceData : DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; } 
    }
    [Serializable]
    public class DSMultiChoiceData : DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; } 
        [field: SerializeField] public string Stat { get; set; }
        [field: SerializeField] public string StatValue { get; set; }
    }
    [Serializable]
    public class DSPartyChoiceData : DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; } 
    }
    [Serializable]
    public class DSPositionChoiceData : DSDialogueChoiceData
    {
        
    }
}
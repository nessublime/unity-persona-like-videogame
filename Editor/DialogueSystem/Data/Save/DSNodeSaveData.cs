using System;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    using Enumerations;

    [Serializable]
    public class DSNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public DSDialogueType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
        [field: SerializeField] public List<DSChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string Stat { get; set; }
        [field: SerializeField] public string StatEXP { get; set; }
        [field: SerializeField] public string Expression { get; set; }
        [field: SerializeField] public string Conversant { get; set; }
        [field: SerializeField] public List<string> Positions { get; set; }
        [field: SerializeField] public List<string> Conversants { get; set; }
        [field: SerializeField] public string Priority { get; set; }
    }
    [Serializable]
    public class DSMasterNodeSaveData : DSNodeSaveData
    {
        //[field: SerializeField] public List<string> InitialPositions { get; set; }
        //[field: SerializeField] public List<string> Conversants { get; set; }
    }
    [Serializable]
    public class DSSingleChoiceSaveData : DSNodeSaveData
    {
        //[field: SerializeField] public string Conversant { get; set; }
        //[field: SerializeField] public string Expression { get; set; }
        //[field: SerializeField] public string Text { get; set; }
    }
    [Serializable]
    public class DSMultipleChoiceSaveData : DSNodeSaveData
    {
        //[field: SerializeField] public string Conversant { get; set; }
        //[field: SerializeField] public string Expression { get; set; }
        //[field: SerializeField] public string Text { get; set; }
    }
    [Serializable]
    public class DSResponseChoiceSaveData : DSNodeSaveData
    {
        //[field: SerializeField] public string Stat { get; set; }
        //[field: SerializeField] public string StatEXP { get; set; }
        //[field: SerializeField] public string Expression { get; set; }
        //[field: SerializeField] public string Text { get; set; }
    }
    [Serializable]
    public class DSPartyNodeSaveData : DSNodeSaveData
    {
        //[field: SerializeField] public string Priority { get; set; }
        //[field: SerializeField] public string Expression { get; set; }
        //[field: SerializeField] public string Text { get; set; }
    }
    [Serializable]
    public class DSPositionNodeSaveData : DSNodeSaveData
    {
        //[field: SerializeField] public List<string> NewPositions { get; set; }
    }
}    
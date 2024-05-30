using System;
using UnityEngine;

namespace DS.Data.Save
{
    [Serializable]
    public class DSChoiceSaveData
    {
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string Stat { get; set; }
        [field: SerializeField] public string StatValue { get; set; }
    
    }
    [Serializable]
    public class DSSingleSaveData : DSChoiceSaveData
    {
        //[field: SerializeField] public string Text { get; set; }
    }
    [Serializable]
    public class DSMultipleSaveData : DSChoiceSaveData
    {
        //[field: SerializeField] public string Text { get; set; }
        //[field: SerializeField] public string Stat { get; set; }
        //[field: SerializeField] public string StatValue { get; set; }
    }
    [Serializable]
    public class DSPartySaveData : DSChoiceSaveData
    {
        //[field: SerializeField] public string Text { get; set; }
    }
    [Serializable]
    public class DSPositionSaveData : DSChoiceSaveData
    {

    }
}
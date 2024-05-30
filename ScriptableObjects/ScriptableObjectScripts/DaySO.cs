using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDaySO", menuName = "ScriptableObject/DaySO")]
public class DaySO : ScriptableObject
{
    [Header("VARIABLES CRONOLOGICAS")]
    public int dayNumber;
    public string dayName;

    [Header("HORARIO")]
    public List<Date> Timetable;
    [System.Serializable]public struct Date{
        public int horaInicial;
        public VirtualSceneSO initialScene;
        public VirtualSceneSO outFreeScene;
    }
}

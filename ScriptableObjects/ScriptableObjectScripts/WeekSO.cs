using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeekSO", menuName = "ScriptableObject/WeekSO")]
public class WeekSO : ScriptableObject
{
    public string weekName;
    public int weekIndex;
    public DaySO[] daySOArray; 
}

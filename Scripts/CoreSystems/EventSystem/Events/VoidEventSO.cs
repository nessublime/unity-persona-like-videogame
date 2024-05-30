using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVoidEvent", menuName = "GameEvent/VoidEvent")]
public class VoidEventSO : BaseGameEventSO<VoidDataStruct>
{
    public void Raise() => Raise(new VoidDataStruct());
}

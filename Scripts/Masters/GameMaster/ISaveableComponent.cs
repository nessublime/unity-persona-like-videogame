using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveableComponent
{
    object SaveState();
    object CreateState();
    void LoadState(object state);
}

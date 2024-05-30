using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseInteractuable : MonoBehaviour, IInteractuable
{
    public virtual async Task<bool> InteractTask(){
        var finished = await Task.Run(() =>{return true;});
        return finished;
    }
}

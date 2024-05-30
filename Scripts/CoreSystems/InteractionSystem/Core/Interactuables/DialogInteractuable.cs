using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using DS.ScriptableObjects;

public class DialogInteractuable : MonoBehaviour, IInteractuable
{

    public DSDialogueContainerSO dialog;

    public void Awake(){
        DSMasterDialogueSO masterDialog = (DSMasterDialogueSO)dialog.UngroupedDialogues[0];
        foreach(string conversant in masterDialog.Conversants){
            //if(){

            //}
        }
    }

    public async Task<bool> InteractTask(){
        UIMaster.ReadDialog(dialog);

        var finished = await Task.Run(() =>{return true;});
        return finished;
    }
}

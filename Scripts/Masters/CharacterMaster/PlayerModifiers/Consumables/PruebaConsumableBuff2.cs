using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PruebaConsumableBuff2 : MonoBehaviour, IBuff
{
    public string buffName;
    PlayerMaster.BuffStruct buffStruct;
    int counter;

    public IEnumerator ApplyBuff(int minutes, Texture2D sprite, string text){

        if(!PlayerMaster.actualBuffs.ContainsKey(buffName)){
            buffStruct = new PlayerMaster.BuffStruct(buffName,sprite,minutes,this);
            PlayerMaster.actualBuffs.Add(buffName,buffStruct);
            PlayerMaster.actualBuffNames.Add(buffName);
            UIMaster.UpdateBuffTopPanel();

            PlayerMaster.playerData.ardidBoost += 1;

            VisualElement panel = UIMaster.UpdateBuffPanel(sprite, minutes, text);
            
            counter = minutes;
            while(counter > 0){
                yield return new WaitForSeconds(60);
                counter--;
                panel.Q<Label>("Buff_Timer").text = counter.ToString();
                buffStruct.duration = counter;
                PlayerMaster.actualBuffs[buffName] = buffStruct;
            }
            PlayerMaster.actualBuffs.Remove(buffName);
            PlayerMaster.actualBuffNames.Remove(buffName);
            UIMaster.UpdateBuffTopPanel();
            panel.AddToClassList("HideDisplay");

            PlayerMaster.playerData.ardidBoost -= 1;
            PlayerMaster.UpdateFinalStats();
            UIMaster.UpdateStatApp();
        }
        else{
            buffStruct.duration = minutes;
            counter = minutes;
        }
    }    
}

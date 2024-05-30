using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using DS.ScriptableObjects;
using DS.Enumerations;
using DS.Data;

public class UIMaster : Singleton<UIMaster>
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    private UIDocument DialogDocument;
    private static VisualElement dialogRoot;

    private UIDocument MobileDocument;
    private static VisualElement mobileRoot;

    private static Dictionary<string,List<Texture2D>> actualConversantSprites = new Dictionary<string,List<Texture2D>>();

    static VisualElement dialogPanel;

    private static bool pressed;

    static DialogUIHelper dialogHelper;

    private static bool smartPhoneShowed = false;
    private static bool smartphoneInApp = false;

    private static VisualElement app1;
    private static VisualElement app2;
    private static VisualElement app3;
    private static VisualElement app4;
    private static VisualElement app5;
    private static VisualElement app6;
    private static VisualElement actualApp;

    public Texture2D prueba;
    public static Texture2D pruebatal;
    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    //***********************************    AWAKE & START    ***********************************//
    protected override void Awake(){
        base.Awake();
        DialogDocument = transform.Find("DialogTreeAssetGO").GetComponent<UIDocument>();
        dialogRoot = DialogDocument.rootVisualElement;
        dialogHelper = transform.Find("DialogTreeAssetGO").GetComponent<DialogUIHelper>();
        MobileDocument = transform.Find("MobileTreeAssetGO").GetComponent<UIDocument>();
        mobileRoot = MobileDocument.rootVisualElement;
        app1 = mobileRoot.Q<VisualElement>("M_Apps_Panel").Q<VisualElement>("App1");
        app2 = mobileRoot.Q<VisualElement>("M_Apps_Panel").Q<VisualElement>("App2");
        app3 = mobileRoot.Q<VisualElement>("M_Apps_Panel").Q<VisualElement>("App3");
        app4 = mobileRoot.Q<VisualElement>("M_Apps_Panel").Q<VisualElement>("App4");
        app5 = mobileRoot.Q<VisualElement>("M_Apps_Panel").Q<VisualElement>("App5");
        app6 = mobileRoot.Q<VisualElement>("M_Apps_Panel").Q<VisualElement>("App6");
        pruebatal = prueba;
        
    }
    protected void OnEnable(){
        GameMaster.DefaultInputActions.UI.Interact.started += ctx => pressed = true;
        GameMaster.DefaultInputActions.Player.SwitchSmartphone.started += ToogleSmartPhone;
    }
    protected void Start(){

    }
    private void Update(){

    }
    private void LateUpdate(){
        pressed = false;
    }
    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//

    public void SceneChange(){
        actualConversantSprites.Clear();
        GameObject Conversants = GameObject.Find("Conversator");
        foreach(DialogInteractuable dialogInteractuable in Conversants.transform.GetComponentsInChildren<DialogInteractuable>()){
            DSMasterDialogueSO masterDialog = (DSMasterDialogueSO)dialogInteractuable.dialog.UngroupedDialogues[0];
            foreach(string conversant in masterDialog.Conversants){
                Debug.Log(conversant);
                if(!actualConversantSprites.ContainsKey(conversant)){
                    List<string> labelsArray = new List<string>();
                    string[] labels = conversant.Split(char.Parse("_"));
                    string nameLabel = "CH_"+labels[0];
                    string typeLabel = "CT_"+labels[1];    
                    labelsArray.Add(nameLabel);
                    labelsArray.Add(typeLabel);
                    List<Texture2D> spriteList = new List<Texture2D>();
                    AsyncOperationHandle<IList<Texture2D>> loadHandle = Addressables.LoadAssetsAsync<Texture2D>(labelsArray, addresable => {
                        spriteList.Add(addresable);
                    },Addressables.MergeMode.Intersection);
                    actualConversantSprites.Add(conversant, spriteList);
                }
            }
        }
    }

    //********************************     SmartPhone METHODS      ******************************//
    public async static void ToogleSmartPhone(InputAction.CallbackContext ctx){
        if(smartPhoneShowed){
            mobileRoot.Q<VisualElement>("Mobile_Panel").AddToClassList("HideMobile");
            smartPhoneShowed = false;
            if(smartphoneInApp){
                actualApp.RemoveFromClassList("ActiveApp");
                smartphoneInApp = false;
            }
            GameMaster.DefaultInputActions.Player.Interact.started -= UseSmartPhoneMainMenu;  
        }else{
            mobileRoot.Q<VisualElement>("Mobile_Panel").RemoveFromClassList("HideMobile");
            smartPhoneShowed = true;
            app1.Focus();
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Interact.started += UseSmartPhoneMainMenu;
        }
    }

    public async static void UseSmartPhoneMainMenu(InputAction.CallbackContext ctx){
        GameMaster.DefaultInputActions.Player.Interact.started -= UseSmartPhoneMainMenu;
        if(app1.focusController.focusedElement == app1){
            smartphoneInApp = true;
            actualApp = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats");
            actualApp.AddToClassList("ActiveApp");
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Return.started += CloseApp;
        }else if(app2.focusController.focusedElement == app2){
            smartphoneInApp = true;
            actualApp = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppOptions");
            actualApp.AddToClassList("ActiveApp");
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Return.started += CloseApp;
        }else if(app3.focusController.focusedElement == app3){
            smartphoneInApp = true;
            actualApp = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppExit");
            actualApp.AddToClassList("ActiveApp");
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Return.started += CloseApp;
        }else if(app4.focusController.focusedElement == app4){
            smartphoneInApp = true;
            actualApp = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppBuffsPassives");
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Buffs").AddToClassList("Tab_BuffSelected");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Buffs_Panel").RemoveFromClassList("HideDisplay");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Passives_Panel").AddToClassList("HideDisplay");
            actualApp.AddToClassList("ActiveApp");
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Return.started += CloseBuffPasiveApp;
            GameMaster.DefaultInputActions.Player.L.started += ChangeBuffPasiveTab;
            GameMaster.DefaultInputActions.Player.R.started += ChangeBuffPasiveTab;
        }else if(app5.focusController.focusedElement == app5){
            smartphoneInApp = true;
            actualApp = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppEquipment");
            actualApp.AddToClassList("ActiveApp");
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Return.started += CloseApp;
        }else if(app6.focusController.focusedElement == app6){
            smartphoneInApp = true;
            actualApp = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppCollectibles");
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Bottles").AddToClassList("Tab_Selected");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Bottles_Panel").RemoveFromClassList("HideDisplay");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Clippers_Panel").AddToClassList("HideDisplay");
            actualApp.AddToClassList("ActiveApp");
            await Task.Delay(200);
            GameMaster.DefaultInputActions.Player.Return.started += CloseCollectibleApp;
            GameMaster.DefaultInputActions.Player.L.started += ChangeCollectibleTab;
            GameMaster.DefaultInputActions.Player.R.started += ChangeCollectibleTab;
            
        }   
    }

    public async static void CloseApp(InputAction.CallbackContext ctx){
        smartphoneInApp = false;
        GameMaster.DefaultInputActions.Player.Return.started -= CloseApp;
        actualApp.RemoveFromClassList("ActiveApp");
        await Task.Delay(200);
        GameMaster.DefaultInputActions.Player.Interact.started += UseSmartPhoneMainMenu;
    }
    public async static void CloseCollectibleApp(InputAction.CallbackContext ctx){
        smartphoneInApp = false;
        GameMaster.DefaultInputActions.Player.Return.started -= CloseCollectibleApp;
        GameMaster.DefaultInputActions.Player.L.started -= ChangeCollectibleTab;
        GameMaster.DefaultInputActions.Player.R.started -= ChangeCollectibleTab;
        actualApp.RemoveFromClassList("ActiveApp");
        await Task.Delay(200);
        GameMaster.DefaultInputActions.Player.Interact.started += UseSmartPhoneMainMenu;
    }
    public async static void CloseBuffPasiveApp(InputAction.CallbackContext ctx){
        smartphoneInApp = false;
        GameMaster.DefaultInputActions.Player.Return.started -= CloseBuffPasiveApp;
        GameMaster.DefaultInputActions.Player.L.started -= ChangeBuffPasiveTab;
        GameMaster.DefaultInputActions.Player.R.started -= ChangeBuffPasiveTab;
        actualApp.RemoveFromClassList("ActiveApp");
        await Task.Delay(200);
        GameMaster.DefaultInputActions.Player.Interact.started += UseSmartPhoneMainMenu;
    }


    public static void UpdateStatApp(){
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat1_Panel").Q<Label>("StatLevel").text = PlayerMaster.GetStat("Erudicion").ToString();
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat2_Panel").Q<Label>("StatLevel").text = PlayerMaster.GetStat("Templanza").ToString();
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat3_Panel").Q<Label>("StatLevel").text = PlayerMaster.GetStat("Ardid").ToString();
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat4_Panel").Q<Label>("StatLevel").text = PlayerMaster.GetStat("Gracia").ToString();
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat5_Panel").Q<Label>("StatLevel").text = PlayerMaster.GetStat("Lucidez").ToString();
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat6_Panel").Q<Label>("StatLevel").text = PlayerMaster.GetStat("Valentia").ToString();

        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat1_Panel").Q<VisualElement>("StatNameProgress_Panel").Q<VisualElement>("StatProgress").Q<VisualElement>("StatProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStatExp("Erudicion"), LengthUnit.Percent); /*Length.Percent(PlayerMaster.GetStatExp("Erudicion"));*/
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat2_Panel").Q<VisualElement>("StatNameProgress_Panel").Q<VisualElement>("StatProgress").Q<VisualElement>("StatProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStatExp("Templanza"), LengthUnit.Percent); /*PlayerMaster.GetStatExp("Templanza");*/
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat3_Panel").Q<VisualElement>("StatNameProgress_Panel").Q<VisualElement>("StatProgress").Q<VisualElement>("StatProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStatExp("Ardid"), LengthUnit.Percent); /*PlayerMaster.GetStatExp("Ardid");*/
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat4_Panel").Q<VisualElement>("StatNameProgress_Panel").Q<VisualElement>("StatProgress").Q<VisualElement>("StatProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStatExp("Gracia"), LengthUnit.Percent); /*PlayerMaster.GetStatExp("Gracia");*/
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat5_Panel").Q<VisualElement>("StatNameProgress_Panel").Q<VisualElement>("StatProgress").Q<VisualElement>("StatProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStatExp("Lucidez"), LengthUnit.Percent);/*PlayerMaster.GetStatExp("Lucidez");*/
        mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppStats").Q<VisualElement>("Stat6_Panel").Q<VisualElement>("StatNameProgress_Panel").Q<VisualElement>("StatProgress").Q<VisualElement>("StatProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStatExp("Valentia"), LengthUnit.Percent);/*PlayerMaster.GetStatExp("Valentia");*/
    }

    public static void UpdateStressTopPanel(){
        mobileRoot.Q<VisualElement>("Top_Panel").Q<VisualElement>("State_Panel").Q<VisualElement>("Battery_Panel").Q<VisualElement>("BatteryBorder").Q<VisualElement>("BatteryProgress").Q<VisualElement>("BatteryProgressBar").Q<VisualElement>().style.width = new Length(PlayerMaster.GetStressBarValue(), LengthUnit.Percent);
        if(PlayerMaster.GetStressBarValue() <= 50){mobileRoot.Q<VisualElement>("Top_Panel").Q<VisualElement>("State_Panel").Q<VisualElement>("Battery_Panel").Q<VisualElement>("BatteryBorder").Q<VisualElement>("BatteryProgress").Q<VisualElement>("BatteryProgressBar").style.backgroundColor = Color.Lerp(Color.red, Color.yellow,((float)2*PlayerMaster.GetStressBarValue())/100f);}
        else{mobileRoot.Q<VisualElement>("Top_Panel").Q<VisualElement>("State_Panel").Q<VisualElement>("Battery_Panel").Q<VisualElement>("BatteryBorder").Q<VisualElement>("BatteryProgress").Q<VisualElement>("BatteryProgressBar").style.backgroundColor = Color.Lerp(Color.yellow, Color.green,((float)2*PlayerMaster.GetStressBarValue())/100f);} 
        mobileRoot.Q<VisualElement>("Top_Panel").Q<VisualElement>("State_Panel").Q<VisualElement>("Battery_Panel").Q<VisualElement>("BatteryBorder").Q<Label>("BatteryLevel").text = PlayerMaster.GetStressLevel().ToString();
    }



    public async static void ChangeCollectibleTab(InputAction.CallbackContext ctx){
        if(actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Bottles").ClassListContains("Tab_Selected")){
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Bottles").RemoveFromClassList("Tab_Selected");
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Clippers").AddToClassList("Tab_Selected");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Bottles_Panel").AddToClassList("HideDisplay");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Clippers_Panel").RemoveFromClassList("HideDisplay");
        }else{
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Clippers").RemoveFromClassList("Tab_Selected");
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Bottles").AddToClassList("Tab_Selected");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Bottles_Panel").RemoveFromClassList("HideDisplay");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Clippers_Panel").AddToClassList("HideDisplay");
        }
    }

    public static void UpdateCollectibleApp(){
        VisualElement panel = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppCollectibles");
        int i = 0;
        foreach(bool bottleCollectible in PlayerMaster.playerData.bottleCollectibles){
            if(bottleCollectible){panel.Q<VisualElement>("Bottles_Panel").ElementAt(i).RemoveFromClassList("NotCollectible");i++;}
        }
        i = 0;
        foreach(bool lighterCollectible in PlayerMaster.playerData.lighterCollectibles){
            if(lighterCollectible){panel.Q<VisualElement>("Clippers_Panel").ElementAt(i).RemoveFromClassList("NotCollectible");i++;}
        }
    }

    public static void UpdateBuffTopPanel(){
        VisualElement notificationPanel = mobileRoot.Q<VisualElement>("Top_Panel").Q<VisualElement>("State_Panel").Q<VisualElement>("Notifications_Panel");
        int listCount = PlayerMaster.actualBuffNames.Count;
        for(int i=0;i<listCount;i++){
            if(i<5){
                if(PlayerMaster.actualBuffNames[i] != null){
                    PlayerMaster.BuffStruct buff = PlayerMaster.actualBuffs[PlayerMaster.actualBuffNames[i]];
                    Texture2D image = buff.image;
                    notificationPanel.ElementAt(i).style.backgroundImage = new StyleBackground(image);
                    if(notificationPanel.ElementAt(i).ClassListContains("HideDisplay")){notificationPanel.ElementAt(i).RemoveFromClassList("HideDisplay");}    
                }
                else{if(!notificationPanel.ElementAt(i).ClassListContains("HideDisplay")){notificationPanel.ElementAt(i).AddToClassList("HideDisplay");}}
            }           
        }
        if(listCount<6){for(int i=listCount;i<6;i++){notificationPanel.ElementAt(i).AddToClassList("HideDisplay");}}
        if(PlayerMaster.actualBuffNames.Count > 5){if(notificationPanel.ElementAt(5).ClassListContains("HideDisplay")){notificationPanel.ElementAt(5).RemoveFromClassList("HideDisplay");}
        }else{notificationPanel.ElementAt(5).AddToClassList("HideDisplay");}
    }
    
    public static VisualElement UpdateBuffPanel(Texture2D image, int time, string buffInfo){
        VisualElement buffPasiveAppPanel = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppBuffsPassives");
        VisualElement buffPanel = new VisualElement();
        bool isEmpty = false; int i = 0;
        while(!isEmpty){
            i++;
            buffPanel = buffPasiveAppPanel.Q<VisualElement>("Content_Panel").Q<VisualElement>("Buffs_Panel").ElementAt(i);
            if(buffPanel.ClassListContains("HideDisplay")){isEmpty = true;buffPanel.RemoveFromClassList("HideDisplay");} 
        }
        buffPanel.Q<VisualElement>("Buff_Icon").style.backgroundImage = new StyleBackground(image);
        buffPanel.Q<Label>("Buff_Timer").text = time.ToString();
        buffPanel.Q<Label>("Buff_Description").text = buffInfo;
        return buffPanel;
    } 

    public static void UpdatePasivePanel(){
        
        VisualElement pasivePanel = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppBuffsPassives").Q<VisualElement>("Content_Panel").Q<VisualElement>("Passives_Panel");
        int listCount = PlayerMaster.actualPasiveNames.Count;
        for(int i=0;i<listCount;i++){
            if(i<9){
                if(PlayerMaster.actualPasiveNames[i] != null){
                    Debug.Log("Entre");
                    PlayerMaster.PasiveStruct pasive = PlayerMaster.actualPasives[PlayerMaster.actualPasiveNames[i]];
                    pasivePanel.ElementAt(i).Q<VisualElement>("Passive_Icon").style.backgroundImage = new StyleBackground(pasive.image);
                    pasivePanel.ElementAt(i).Q<Label>("Passive_Description").text = pasive.text;
                    if(pasivePanel.ElementAt(i).ClassListContains("HideDisplay")){pasivePanel.ElementAt(i).RemoveFromClassList("HideDisplay");}
                }
                else{if(!pasivePanel.ElementAt(i).ClassListContains("HideDisplay")){pasivePanel.ElementAt(i).AddToClassList("HideDisplay");}}
            }
        }
        if(listCount<10){for(int i=listCount;i<9;i++){pasivePanel.ElementAt(i).AddToClassList("HideDisplay");}}
    }

    public async static void ChangeBuffPasiveTab(InputAction.CallbackContext ctx){
        if(actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Buffs").ClassListContains("Tab_BuffSelected")){
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Buffs").RemoveFromClassList("Tab_BuffSelected");
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Passives").AddToClassList("Tab_BuffSelected");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Buffs_Panel").AddToClassList("HideDisplay");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Passives_Panel").RemoveFromClassList("HideDisplay");
        }else{
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Passives").RemoveFromClassList("Tab_BuffSelected");
            actualApp.Q<VisualElement>("Tabs_Panel").Q<VisualElement>("Tab_Buffs").AddToClassList("Tab_BuffSelected");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Buffs_Panel").RemoveFromClassList("HideDisplay");
            actualApp.Q<VisualElement>("Content_Panel").Q<VisualElement>("Passives_Panel").AddToClassList("HideDisplay");
        }
    }



    public static void UpdateEquipationApp(){
        VisualElement equipationPanel = mobileRoot.Q<VisualElement>("OppenedApps_Panel").Q<VisualElement>("AppEquipment");
        equipationPanel.Q<VisualElement>("Special_Panel").style.backgroundImage = new StyleBackground(PlayerMaster.playerData.actualMainAccesory.image);
        equipationPanel.Q<VisualElement>("Jacket_Panel").style.backgroundImage = new StyleBackground(PlayerMaster.playerData.actualMainSuperior.image);
        equipationPanel.Q<VisualElement>("Pants_Panel").style.backgroundImage = new StyleBackground(PlayerMaster.playerData.actualMainInferior.image);
    }



    //**********************************      DIALOG METHODS      *******************************//
    public async static void ReadDialog(DSDialogueContainerSO dialog){
        GameMaster.ChangeActionMap(GameMaster.ActionMap.UI);
        VisualElement panel = new VisualElement();
        VisualElement textPanel = new VisualElement();
        VisualElement AButton = new VisualElement();
        DSMasterDialogueSO masterDialog = (DSMasterDialogueSO)dialog.UngroupedDialogues[0];
        List<string> initialPositions = masterDialog.InitialPositions;
        DSDialogueSO nextDialog = masterDialog.Choices[0].NextDialogue;
        dialogPanel = dialogRoot.Q<VisualElement>("DialogPanel");
        string conversant;
        bool playerResponse;
        pressed = false;
        SetPortraitPositions(initialPositions);
        
        while(nextDialog != null){
            switch(nextDialog.DialogueType){

                case DSDialogueType.SingleChoice:
                    DSSingleChoiceDialogueSO singleNode = (DSSingleChoiceDialogueSO) nextDialog;
                    conversant = singleNode.Conversant;
                    SingleNodeConversation(0,"PortraitPanelL",initialPositions,conversant,panel,textPanel,singleNode);
                    SingleNodeConversation(1,"PortraitPanelLB",initialPositions,conversant,panel,textPanel,singleNode);
                    SingleNodeConversation(2,"PortraitPanelC",initialPositions,conversant,panel,textPanel,singleNode);
                    SingleNodeConversation(3,"PortraitPanelRB",initialPositions,conversant,panel,textPanel,singleNode);
                    SingleNodeConversation(4,"PortraitPanelR",initialPositions,conversant,panel,textPanel,singleNode);
                    AButton = dialogPanel.Q<VisualElement>("AButton");
                    AButton.RemoveFromClassList("Hide");
                    playerResponse = await Task.Run(() => {while(true){if(pressed){return true;}}});
                    await Task.Delay(100);
                    AButton.AddToClassList("Hide");
                    HideTextPanels();
                    nextDialog = singleNode.Choices[0].NextDialogue;
                    break;

                case DSDialogueType.MultipleChoice:
                    DSMultipleChoiceDialogueSO multiNode = (DSMultipleChoiceDialogueSO) nextDialog;
                    conversant = multiNode.Conversant;
                    panel = MultiNodeConversation(0,"PortraitPanelL",initialPositions,conversant,panel,textPanel,multiNode);
                    panel = MultiNodeConversation(1,"PortraitPanelLB",initialPositions,conversant,panel,textPanel,multiNode);
                    panel = MultiNodeConversation(2,"PortraitPanelC",initialPositions,conversant,panel,textPanel,multiNode);
                    panel = MultiNodeConversation(3,"PortraitPanelRB",initialPositions,conversant,panel,textPanel,multiNode);
                    panel = MultiNodeConversation(4,"PortraitPanelR",initialPositions,conversant,panel,textPanel,multiNode);
                    AButton = dialogPanel.Q<VisualElement>("AButton");
                    AButton.RemoveFromClassList("Hide");
                    playerResponse = await Task.Run(() => {while(true){if(pressed){return true;}}});
                    await Task.Delay(100);
                    AButton.AddToClassList("Hide");
                    List<Button> responseButtonList = new List<Button>();
                    List<VisualElement> statPanelList = new List<VisualElement>();
                    VisualElement responsePanel = panel.Q<VisualElement>("ResponsePanel");
                    Button responseButton1 = responsePanel.Q<Button>("ResponseButton1");
                    responseButtonList.Add(responseButton1);
                    Button responseButton2 = responsePanel.Q<Button>("ResponseButton2");
                    responseButtonList.Add(responseButton2);
                    Button responseButton3 = responsePanel.Q<Button>("ResponseButton3");
                    responseButtonList.Add(responseButton3);
                    Button responseButton4 = responsePanel.Q<Button>("ResponseButton4");
                    responseButtonList.Add(responseButton4);
                    VisualElement statPanel1 = responseButton1.Q<VisualElement>("StatBox");
                    statPanelList.Add(statPanel1);
                    VisualElement statPanel2 = responseButton2.Q<VisualElement>("StatBox");
                    statPanelList.Add(statPanel2);
                    VisualElement statPanel3 = responseButton3.Q<VisualElement>("StatBox");
                    statPanelList.Add(statPanel3);
                    VisualElement statPanel4 = responseButton4.Q<VisualElement>("StatBox");
                    statPanelList.Add(statPanel4);
                    int choiceCount = 0;
                    foreach(DSMultiChoiceData data in multiNode.Choices){if(data.Stat != "-"){choiceCount++;}}
                    for(int i = 0;i < choiceCount;i++){responseButtonList[i].RemoveFromClassList("Hide"); responseButtonList[i].text = multiNode.Choices[choiceCount-1-i].Text;}
                    for(int i = 0;i < 4-choiceCount;i++){responseButtonList[i+choiceCount].AddToClassList("Hide"); statPanelList[i+choiceCount].AddToClassList("Hide");}
                    for(int i = 0;i < choiceCount;i++){
                        if(multiNode.Choices[i].Stat[0] != '/'){
                            statPanelList[i].RemoveFromClassList("Hide");
                            statPanelList[i].Q<Label>("StatLevel").text = multiNode.Choices[i].StatValue;
                            VisualElement iconPanel = statPanelList[i].Q<VisualElement>("StatIcon");
                            ChoiceIconColor(iconPanel, multiNode, choiceCount-1-i, statPanelList[i]);
                        }else{statPanelList[i].AddToClassList("Hide");}
                    }
                    await Task.Delay(100);
                    statPanelList[choiceCount-1].Focus();
                    responsePanel.RemoveFromClassList("Hide");
                    playerResponse = await Task.Run(() => {while(true){if(pressed){
                        if(choiceCount == 1){
                            if(statPanel1.focusController.focusedElement == responseButton1){nextDialog = multiNode.Choices[3].NextDialogue;}
                        }else if(choiceCount == 2){
                            if(statPanel1.focusController.focusedElement == responseButton1){nextDialog = multiNode.Choices[3].NextDialogue;}
                            else if(statPanel2.focusController.focusedElement == responseButton2){nextDialog = multiNode.Choices[2].NextDialogue;}
                        }else if(choiceCount == 3){
                            if(statPanel1.focusController.focusedElement == responseButton1){nextDialog = multiNode.Choices[2].NextDialogue;}
                            else if(statPanel2.focusController.focusedElement == responseButton2){nextDialog = multiNode.Choices[1].NextDialogue;}
                            else if(statPanel3.focusController.focusedElement == responseButton3){nextDialog = multiNode.Choices[0].NextDialogue;}
                        }else if(choiceCount == 4){
                            if(statPanel1.focusController.focusedElement == responseButton1){nextDialog = multiNode.Choices[3].NextDialogue;}
                            else if(statPanel2.focusController.focusedElement == responseButton2){nextDialog = multiNode.Choices[2].NextDialogue;}
                            else if(statPanel3.focusController.focusedElement == responseButton3){nextDialog = multiNode.Choices[1].NextDialogue;}
                            else if(statPanel4.focusController.focusedElement == responseButton4){nextDialog = multiNode.Choices[0].NextDialogue;}
                        }
                        return true;
                    }}});
                    await Task.Delay(100);
                    statPanel1.AddToClassList("Hide");
                    statPanel2.AddToClassList("Hide");
                    statPanel3.AddToClassList("Hide");
                    statPanel4.AddToClassList("Hide");
                    responseButton1.AddToClassList("Hide");
                    responseButton2.AddToClassList("Hide");
                    responseButton3.AddToClassList("Hide");
                    responseButton4.AddToClassList("Hide");
                    responsePanel.AddToClassList("Hide");
                    HideTextPanels();
                    break;  

                case DSDialogueType.Response:
                    //NOT IMPLEMETED
                    break;
            }
        }
        dialogPanel.Q<VisualElement>("PortraitPanelR").AddToClassList("Hide");
        dialogPanel.Q<VisualElement>("PortraitPanelRB").AddToClassList("Hide");
        dialogPanel.Q<VisualElement>("PortraitPanelL").AddToClassList("Hide");
        dialogPanel.Q<VisualElement>("PortraitPanelLB").AddToClassList("Hide");
        dialogPanel.Q<VisualElement>("PortraitPanelC").AddToClassList("Hide");
        GameMaster.ChangeActionMap(GameMaster.ActionMap.Player);
    }

    private static void SetPortraitPositions(List<string> positions){
        List<string> panelNameList = new List<string>();
        panelNameList.Add("PortraitPanelL");
        panelNameList.Add("PortraitPanelLB");
        panelNameList.Add("PortraitPanelC");
        panelNameList.Add("PortraitPanelRB");
        panelNameList.Add("PortraitPanelR");
        for(int i = 0;i<5;i++){
            if(positions[i] == "  --  "){VisualElement panel = dialogPanel.Q<VisualElement>(panelNameList[i]); panel.AddToClassList("Hide");}
            else{
                VisualElement panel = dialogPanel.Q<VisualElement>(panelNameList[i]);
                List<Texture2D> expressionList = actualConversantSprites[positions[i]];
                string actualExpressionName = positions[i]+"_Default";
                Texture2D newSprite = null;
                foreach(Texture2D expresion in expressionList){if(expresion.name == actualExpressionName){newSprite = expresion;}}
                panel.Q<VisualElement>("Portrait").style.backgroundImage = newSprite;
                panel.Q<VisualElement>("Portrait").RemoveFromClassList("Hide");
                panel.RemoveFromClassList("Hide");
            }
        }
    }

    public static void SingleNodeConversation(int i, string panelName, List<string> initialPositions, string conversant, VisualElement panel, VisualElement textPanel, DSSingleChoiceDialogueSO singleNode){
        if(initialPositions[i] != conversant){
            VisualElement portrait = dialogPanel.Q<VisualElement>(panelName).Q<VisualElement>("Portrait");
            if(portrait.style.backgroundImage == null){portrait.AddToClassList("Dark");}
        }
        else{
            panel = dialogPanel.Q<VisualElement>(panelName); 
            VisualElement portrait = panel.Q<VisualElement>("Portrait");
            List<Texture2D> expressionList = actualConversantSprites[conversant];
            string expresionName = conversant+"_"+singleNode.Expression;
            foreach(Texture2D expression in expressionList){if(expression.name == expresionName){portrait.style.backgroundImage = expression;}}
            panel.BringToFront();
            if(panelName[panelName.Length-1] == 'B'){textPanel = dialogPanel.Q<VisualElement>(panelName.Substring(0,panelName.Length-1)).Q<VisualElement>("DialogBox"); }
            else{textPanel = panel.Q<VisualElement>("DialogBox");}
            Label text = textPanel.Q<Label>("DialogText");
            text.text = singleNode.Text;
            textPanel.RemoveFromClassList("Hide");
        }
    }

    public static VisualElement MultiNodeConversation(int i, string panelName, List<string> initialPositions, string conversant, VisualElement panel, VisualElement textPanel, DSMultipleChoiceDialogueSO multiNode){
        if(initialPositions[i] != conversant){
            VisualElement portrait = dialogPanel.Q<VisualElement>(panelName).Q<VisualElement>("Portrait");
            if(portrait.style.backgroundImage == null){portrait.AddToClassList("Dark");}
        }
        else{
            panel = dialogPanel.Q<VisualElement>(panelName); 
            VisualElement portrait = panel.Q<VisualElement>("Portrait");
            List<Texture2D> expressionList = actualConversantSprites[conversant];
            string expresionName = conversant+"_"+multiNode.Expression;
            foreach(Texture2D expression in expressionList){if(expression.name == expresionName){portrait.style.backgroundImage = expression;}}
            panel.BringToFront();
            if(panelName[panelName.Length-1] == 'B'){textPanel = dialogPanel.Q<VisualElement>(panelName.Substring(0,panelName.Length-1)).Q<VisualElement>("DialogBox"); }
            textPanel = panel.Q<VisualElement>("DialogBox");
            Label text = textPanel.Q<Label>("DialogText");
            text.text = multiNode.Text;
            textPanel.RemoveFromClassList("Hide");
        }
        return panel;
    }

    private static void ChoiceIconColor(VisualElement panel, DSMultipleChoiceDialogueSO node, int index, VisualElement backPanel){
        if(node.Choices[index].Stat == "V"){panel.style.backgroundImage = dialogHelper.IconoValentia; Color color = new Color(255, 0, 0, 1); backPanel.style.unityBackgroundImageTintColor = color;}
        if(node.Choices[index].Stat == "A"){panel.style.backgroundImage = dialogHelper.IconoArdid; Color color = new Color(113, 113, 113, 1); backPanel.style.unityBackgroundImageTintColor = color;}
        if(node.Choices[index].Stat == "G"){panel.style.backgroundImage = dialogHelper.IconoGracia; Color color = new Color(255, 193, 0, 1); backPanel.style.unityBackgroundImageTintColor = color;}
        if(node.Choices[index].Stat == "T"){panel.style.backgroundImage = dialogHelper.IconoTemplanza; Color color = new Color(0, 255, 187, 1); backPanel.style.unityBackgroundImageTintColor = color;}
        if(node.Choices[index].Stat == "E"){panel.style.backgroundImage = dialogHelper.IconoErudicion; Color color = new Color(144, 0, 255, 1); backPanel.style.unityBackgroundImageTintColor = color;}
        if(node.Choices[index].Stat == "L"){panel.style.backgroundImage = dialogHelper.IconoLucidez; Color color = new Color(255, 255, 255, 1); backPanel.style.unityBackgroundImageTintColor = color;}
    }

    private static void HideTextPanels(){
        dialogPanel.Q<VisualElement>("PortraitPanelR").Q<VisualElement>("DialogBox").AddToClassList("Hide");
        dialogPanel.Q<VisualElement>("PortraitPanelL").Q<VisualElement>("DialogBox").AddToClassList("Hide");
        dialogPanel.Q<VisualElement>("PortraitPanelC").Q<VisualElement>("DialogBox").AddToClassList("Hide");
    }
}


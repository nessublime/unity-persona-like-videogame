using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSMasterNode : DSNode
    {
        public static string[] actualConversants = new string[10];
        //public TextField[] conversantNameText =  new TextField[10];
        public DropdownField[] conversantNameText =  new DropdownField[10];
        public static string[] actualStaticConversants;
        public static List<string> conversants = new List<string>();
        public List<string> initialPositions = new List<string>();
        public List<string> initialConversants = new List<string>();
        int i = 0;
        public Button RFName;
        public Button RBName;
        public Button CName;
        public Button LBName;
        public Button LFName;
        public Button MasterTitle;
        public Button SetData;
        public Foldout conversantFoldout;
        public VisualElement conversantDataContainer;
        public Port choicePort;
        public Button deleteChoiceButton;


        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);
            conversants.Add("");
            DialogueType = DSDialogueType.Master;

            DSSingleSaveData choiceData = new DSSingleSaveData()
            {
                Text = "Start Dialogue"
            };

            Choices.Add(choiceData);
            //inputContainer.Remove(inputPort);
        }

        public override void Draw()
        {
            base.Draw();
            DialogueName = "MasterNode";
            MasterTitle =  DSElementUtility.CreateButton("Master Node", () =>{});
            MasterTitle.AddToClassList("ds-node__xbutton");
            SetData = DSElementUtility.CreateButton("Set Data", () =>{SetConversantNames();SetInitialPositions();});
            SetData.AddToClassList("ds-node__button");

            inputContainer.Remove(inputPort);
            titleContainer.Remove(ExpressionButton);
            titleContainer.Remove(ConversantButton);
            titleContainer.Remove(dialogueNameTextField);
            titleContainer.Insert(0, MasterTitle);
            titleContainer.Insert(1, SetData);
            customDataContainer.Remove(textFoldout);

            conversantFoldout = DSElementUtility.CreateFoldout("Conversants");
            Foldout positionFoldout = DSElementUtility.CreateFoldout("Position");
            conversantDataContainer = new VisualElement();
            VisualElement positionDataContainer = new VisualElement();
            conversantDataContainer.AddToClassList("ds-node__custom-data-container");
            positionDataContainer.AddToClassList("ds-node__custom-data-container");
            
            //Button deleteChoiceButton = DSElementUtility.CreateButton("  --  ", () =>{});


            List<string> conversableNames = new List<string>();
            List<string> conversableType = new List<string>();
            AddressableAssetSettings settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            object labelTable = settings.GetType().GetProperty("labelTable", bindingFlags).GetValue(settings);
            List<string> labelNames = (List<string>)labelTable.GetType().GetProperty("labelNames", bindingFlags).GetValue(labelTable);
            foreach(string label in labelNames){
                if(label[0].ToString() == "C" && label[1].ToString() == "H"){conversableNames.Add(label);}
                else if(label[0].ToString() == "C" && label[1].ToString() == "T"){conversableType.Add(label);}
            }
            List<string> dropDownChoices = new List<string>();
            foreach(string nameLabel in conversableNames){
                foreach(string typeLabel in conversableType){
                    List<string> labels = new List<string>();
                    labels.Add(nameLabel);
                    labels.Add(typeLabel);
                    bool ended = false;
                    AsyncOperationHandle<IList<Sprite>> loadHandle = Addressables.LoadAssetsAsync<Sprite>(labels, addresable => {
                        if(addresable != null){
                            string newName = nameLabel.Remove(0,3);
                            string newType = typeLabel.Remove(0,3);
                            string newLabel = newName+"_"+newType;
                            dropDownChoices.Add(newLabel);

                            if(initialConversants.Count != 0 && !ended){
                                foreach(string name in initialConversants){
                                    if(name == newLabel){
                                        DropdownField dropdownField = DSElementUtility.CreateDropdownField(dropDownChoices, dropDownChoices.Count - 1);
                                        conversantFoldout.Add(dropdownField);
                                        conversantNameText[i] = dropdownField;
                                        i++;
                                    }
                                }
                                ended = true;
                                if(conversableNames[conversableNames.Count-1] == nameLabel && conversableType[conversableType.Count-1] == typeLabel){
                                    Debug.Log("soy ultimo");
                                    deleteChoiceButton = DSElementUtility.CreateButton("-", () =>{
                                        if(i>0){
                                            conversantFoldout.Remove(conversantNameText[i-1]);
                                            conversantNameText[i-1] = null;
                                            i --;
                                        }
                                    });
                                    deleteChoiceButton.AddToClassList("ds-node__button");
                                    conversantFoldout.Add(deleteChoiceButton);
                                }
                            }
                        }
                        
                    }, 
                    Addressables.MergeMode.Intersection);
                }
            }

            /*if(initialConversants.Count != 0){
                i = 0;
                foreach(string name in initialConversants){
                    DropdownField dropdownField = DSElementUtility.CreateDropdownField(dropDownChoices, dropDownChoices.IndexOf(name));
                    conversantNameText[i] = dropdownField;
                    conversantFoldout.Add(dropdownField);
                    i++;
                }
            }*/


            


            Button addChoiceButton = DSElementUtility.CreateButton("+", () =>
            {   
                if(i<8){
                    DropdownField characterDropdownField = DSElementUtility.CreateDropdownField(dropDownChoices, 0);
                    conversantNameText[i] = characterDropdownField;
                    conversantFoldout.Insert(1,characterDropdownField);
                    i ++;
                } 
            });

            if(initialConversants.Count == 0){
                deleteChoiceButton = DSElementUtility.CreateButton("-", () =>
                {
                    if(i>0){
                        conversantFoldout.Remove(conversantNameText[i-1]);
                        conversantNameText[i-1] = null;
                        i --;
                    }
                });
                deleteChoiceButton.AddToClassList("ds-node__button");
                conversantFoldout.Add(deleteChoiceButton);
            }
            //deleteChoiceButton.AddToClassList("ds-node__button");
            addChoiceButton.AddToClassList("ds-node__button");
            
            Button RF = DSElementUtility.CreateButton("RF", () =>{});
            Button RB = DSElementUtility.CreateButton("RB", () =>{});
            Button C = DSElementUtility.CreateButton("C", () =>{});
            Button LB = DSElementUtility.CreateButton("LB", () =>{});
            Button LF = DSElementUtility.CreateButton("LF", () =>{});

            RFName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(RFName.text == conversators[i]){if(i == conversators.Length -1){RFName.text = "  --  ";}else{RFName.text = conversators[i+1];}ended = true;finded = true;}
                            else if(RFName.text == "  --  "){RFName.text = conversators[0];ended = true;finded = true;}
                        }
                    }
                if(!finded){RFName.text = "  --  ";}
                SetInitialPositions();
            });
            RBName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(RBName.text == conversators[i]){if(i == conversators.Length -1){RBName.text = "  --  ";}else{RBName.text = conversators[i+1];}ended = true;finded = true;}
                            else if(RBName.text == "  --  "){RBName.text = conversators[0];ended = true;finded = true;}
                        }
                    }
                if(!finded){RBName.text = "  --  ";}
                SetInitialPositions();    
            });
            CName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(CName.text == conversators[i]){if(i == conversators.Length -1){CName.text = "  --  ";}else{CName.text = conversators[i+1];}ended = true;finded = true;}
                            else if(CName.text == "  --  "){CName.text = conversators[0];ended = true;finded = true;}
                        }
                    }
                if(!finded){CName.text = "  --  ";}
                SetInitialPositions();
            });
            LBName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(LBName.text == conversators[i]){if(i == conversators.Length -1){LBName.text = "  --  ";}else{LBName.text = conversators[i+1];}ended = true;finded = true;}
                            else if(LBName.text == "  --  "){LBName.text = conversators[0];ended = true;finded = true;}
                        }
                    }
                if(!finded){LBName.text = "  --  ";}
                SetInitialPositions();
            });
            LFName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(LFName.text == conversators[i]){if(i == conversators.Length -1){LFName.text = "  --  ";}else{LFName.text = conversators[i+1];}ended = true;finded = true;}
                            else if(LFName.text == "  --  "){LFName.text = conversators[0];ended = true;finded = true;}
                        }
                    }
                if(!finded){LFName.text = "  --  ";}
                SetInitialPositions();
            });

            VisualElement RFVE = new VisualElement();
            VisualElement RBVE = new VisualElement();
            VisualElement CVE = new VisualElement();
            VisualElement LBVE = new VisualElement();
            VisualElement LFVE = new VisualElement();
            RF.AddToClassList("ds-node__positiontextbutton");
            RB.AddToClassList("ds-node__positiontextbutton");
            C.AddToClassList("ds-node__positiontextbutton");
            LB.AddToClassList("ds-node__positiontextbutton");
            LF.AddToClassList("ds-node__positiontextbutton");
            Port PositionName = this.CreatePort();
 

            conversantDataContainer.Add(conversantFoldout);
            conversantFoldout.Insert(0,addChoiceButton);
            Debug.Log(conversantFoldout.childCount);
            //conversantFoldout.Insert(1,deleteChoiceButton);


            RFVE.Add(RF);
            RFVE.Add(RFName);

            RBVE.Add(RB);
            RBVE.Add(RBName);

            CVE.Add(C);
            CVE.Add(CName);

            LBVE.Add(LB);
            LBVE.Add(LBName);

            LFVE.Add(LF);
            LFVE.Add(LFName);

            PositionName.Add(RFVE);
            PositionName.Add(RBVE);
            PositionName.Add(CVE);
            PositionName.Add(LBVE);
            PositionName.Add(LFVE);
            //positionFoldout.Add(PositionName);
            positionDataContainer.Add(PositionName);
            extensionContainer.Add(positionDataContainer);
            extensionContainer.Add(conversantDataContainer);
            




            /* OUTPUT CONTAINER */

            /*foreach (DSSingleSaveData choice in Choices)
            {
                choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }*/
            //Debug.Log(Choices[0]);
            //Choices[0] = Choices[0];
            //DSChoiceSaveData choice = Choices[0];
            //choicePort = this.CreatePort(choice.Text);
            
            //choicePort.userData = choice;

            //outputContainer.Add(choicePort);

            foreach (DSSingleSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort();

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }


            SetInitialPositions();
            RefreshExpandedState();

            
        }
        private void SetConversantNames(){
            initialConversants = new List<string>();
            for(int i = 0;i<10;i++){actualConversants[i] = null;}
            for(int i = 0;i<10;i++){
                if(conversantNameText[i] != null){actualConversants[i] = conversantNameText[i].text; initialConversants.Add(conversantNameText[i].text);}
            }

        }
        private void SetInitialPositions(){
            initialPositions = new List<string>();
            initialPositions.Add(LFName.text);
            initialPositions.Add(LBName.text);
            initialPositions.Add(CName.text);
            initialPositions.Add(RBName.text);
            initialPositions.Add(RFName.text);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;
    
    /*public class DSPositionNode : DSMasterNode
    {
        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DSDialogueType.Position;
        }

        public override void Draw()
        {
            base.Draw();
            inputPort = this.CreatePort("", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

            inputContainer.Add(inputPort);
            inputContainer.Add(inputPort);
            titleContainer.Add(inputPort);
            titleContainer.Remove(MasterTitle);
            titleContainer.Remove(SetData);
            
            titleContainer.Add(dialogueNameTextField);
            inputContainer.Add(inputPort);
            conversantDataContainer.Add(inputPort);
            conversantDataContainer.Remove(conversantFoldout);
            //outputContainer.Remove(choicePort);

            /* OUTPUT CONTAINER 
            
        }
    }*/
    public class DSPositionNode : DSNode{
        VisualElement positionDataContainer = new VisualElement();
        public Button RFName;
        public Button RBName;
        public Button CName;
        public Button LBName;
        public Button LFName;
        public List<string> initialPositions = new List<string>();

        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DSDialogueType.Position;

            DSPositionSaveData choiceData = new DSPositionSaveData()
            {
                //Text = "";
            };
            Choices.Add(choiceData);
            
        }
        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTAINER */
            Button SetData = DSElementUtility.CreateButton("Set Data", () =>{SetInitialPositions();});
            titleContainer.Insert(1, SetData);

            foreach (DSPositionSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort();

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }
            titleContainer.Remove(ExpressionButton);
            titleContainer.Remove(ConversantButton);
            customDataContainer.Remove(textFoldout);

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
                //if(conversators.Length > 1){
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(RFName.text == conversators[i]){
                                if(i == conversators.Length -1){RFName.text = "  --  ";}
                                else{RFName.text = conversators[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else if(RFName.text == "  --  "){
                                RFName.text = "Ness";
                                ended = true;
                                finded = true;
                            }
                            else if(RFName.text == "Ness"){
                                RFName.text = conversators[0];
                                ended = true;
                                finded = true;
                            }
                        }
                    }
                    if(!finded){RFName.text = "  --  ";}
                //}
            });
            RBName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                //if(conversators.Length > 1){
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(RBName.text == conversators[i]){
                                if(i == conversators.Length -1){RBName.text = "  --  ";}
                                else{RBName.text = conversators[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else if(RBName.text == "  --  "){
                                RBName.text = "Ness";
                                ended = true;
                                finded = true;
                            }
                            else if(RBName.text == "Ness"){
                                RBName.text = conversators[0];
                                ended = true;
                                finded = true;
                            }
                        }
                    }
                    if(!finded){RBName.text = "  --  ";}
                //}
            });
            CName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                //if(conversators.Length > 1){
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(CName.text == conversators[i]){
                                if(i == conversators.Length -1){CName.text = "  --  ";}
                                else{CName.text = conversators[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else if(CName.text == "  --  "){
                                CName.text = "Ness";
                                ended = true;
                                finded = true;
                            }
                            else if(CName.text == "Ness"){
                                CName.text = conversators[0];
                                ended = true;
                                finded = true;
                            }
                        }
                    }
                    if(!finded){CName.text = "  --  ";}
                //}
            });
            LBName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                //if(conversators.Length > 1){
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(LBName.text == conversators[i]){
                                if(i == conversators.Length -1){LBName.text = "  --  ";}
                                else{LBName.text = conversators[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else if(LBName.text == "  --  "){
                                LBName.text = "Ness";
                                ended = true;
                                finded = true;
                            }
                            else if(LBName.text == "Ness"){
                                LBName.text = conversators[0];
                                ended = true;
                                finded = true;
                            }
                        }
                    }
                    if(!finded){LBName.text = "  --  ";}
                //}
            });
            LFName = DSElementUtility.CreateButton("  --  ", () =>{
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){noNull++;}}
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}}
                bool ended = false;
                bool finded = false;
                //if(conversators.Length > 1){
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(LFName.text == conversators[i]){
                                if(i == conversators.Length -1){LFName.text = "  --  ";}
                                else{LFName.text = conversators[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else if(LFName.text == "  --  "){
                                LFName.text = "Ness";
                                ended = true;
                                finded = true;
                            }
                            else if(LFName.text == "Ness"){
                                LFName.text = conversators[0];
                                ended = true;
                                finded = true;
                            }
                        }
                    }
                    if(!finded){LFName.text = "  --  ";}
                //}
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
            customDataContainer.Add(positionDataContainer);



            RefreshExpandedState();
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

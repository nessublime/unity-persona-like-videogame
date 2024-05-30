using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSResponseNode : DSNode
    {
        private VisualElement expVE = new VisualElement();
        public Button aButton;
        public TextField EXPTextField;


        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position){

            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DSDialogueType.Response;

            DSSingleSaveData choiceData = new DSSingleSaveData()
            {
                Text = "New Dialog"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            Button ResponseButton = DSElementUtility.CreateButton("Response", () =>{});

            aButton = DSElementUtility.CreateButton("Gracia", () =>{
                if(aButton.text == "Gracia"){aButton.text = "Ardid";}
                else if(aButton.text == "Ardid"){aButton.text = "Gracia";}
            });

            List<string> labelsArray = new List<string>();
            string nameLabel = "CH_Ness";
            string typeLabel = "CT_Default";    
            labelsArray.Add(nameLabel);
            labelsArray.Add(typeLabel);
            AsyncOperationHandle<IList<Sprite>> loadHandle = Addressables.LoadAssetsAsync<Sprite>(labelsArray, addresable => {
                    if(addresable != null){
                        Debug.Log(addresable.name);
                        expressionArray.Add(addresable.name.Split(char.Parse("_"))[2]);
                    }
                    }, 
                    Addressables.MergeMode.Intersection);




            EXPTextField = DSElementUtility.CreateTextField("Exp", null, callback => {});

            ResponseButton.AddToClassList("ds-node__responsebutton");

            EXPTextField.AddToClassList("ds-node__button");

            mainContainer.Insert(1, ResponseButton);
            titleContainer.Remove(ConversantButton);
            mainContainer.Insert(2, EXPTextField);
            EXPTextField.Insert(0, aButton);
 

            /* OUTPUT CONTAINER */

            foreach (DSSingleSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
    
}    

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

    public class DSNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        protected DSGraphView graphView;
        private Color defaultBackgroundColor;

        public Button ExpressionButton;
        public Button ConversantButton;
        public TextField dialogueNameTextField;
        public Port inputPort;

        public VisualElement customDataContainer;
        public Foldout textFoldout;
        public List<string> expressionArray = new List<string>();

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public virtual void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            DialogueName = nodeName;
            Choices = new List<DSChoiceSaveData>();
            Text = "Dialogue text.";

            SetPosition(new Rect(position, Vector2.zero));

            graphView = dsGraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */

            dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField) callback.target;

                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.NameErrorsAmount;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = target.value;

                    graphView.AddUngroupedNode(this);

                    return;
                }

                DSGroup currentGroup = Group;

                graphView.RemoveGroupedNode(this, Group);

                DialogueName = target.value;

                graphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            
            List<string> labelsArray = new List<string>();
            ConversantButton = DSElementUtility.CreateButton("--", () =>{

                ExpressionButton.text = "--";
                expressionArray = new List<string>();
                labelsArray = new List<string>();
                int noNull = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){
                    if(DSMasterNode.actualConversants[i] != null){noNull++;}
                }
                string[] conversators = new string[noNull];
                int j = 0;
                for(int i = 0;i<=DSMasterNode.actualConversants.Length - 1;i++ ){
                    if(DSMasterNode.actualConversants[i] != null){conversators[j] = DSMasterNode.actualConversants[i];j++;}
                }
                bool ended = false;
                bool finded = false;
                if(conversators.Length > 1){
                    for(int i = 0;i<conversators.Length;i++){
                        if(!ended){
                            if(ConversantButton.text == conversators[i]){
                                if(i == conversators.Length -1){ConversantButton.text = "--";}
                                else{ConversantButton.text = conversators[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else{
                                ConversantButton.text = conversators[0];
                            }
                        }
                    }
                    if(!finded){ConversantButton.text = conversators[0];}
                }
                //Aqui
                string[] labels = ConversantButton.text.Split(char.Parse("_"));
                string nameLabel = "CH_"+labels[0];
                string typeLabel = "CT_"+labels[1];

                
                labelsArray.Add(nameLabel);
                labelsArray.Add(typeLabel);
                AsyncOperationHandle<IList<Sprite>> loadHandle = Addressables.LoadAssetsAsync<Sprite>(labelsArray, addresable => {
                    if(addresable != null){
                        Debug.Log(addresable.name);
                        expressionArray.Add(addresable.name.Split(char.Parse("_"))[2]);
                    }
                    }, 
                    Addressables.MergeMode.Intersection);

                //Debug.Log(typeLabel);
            });


            bool ended;
            ExpressionButton = DSElementUtility.CreateButton("--", () =>{
                bool ended = false;
                bool finded = false;
                if(expressionArray.Count >= 1){
                    for(int i = 0;i<expressionArray.Count;i++){
                        if(!ended){
                            if(ExpressionButton.text == expressionArray[i]){
                                if(i == expressionArray.Count -1){ExpressionButton.text = expressionArray[0];}
                                else{ExpressionButton.text = expressionArray[i+1];}
                                ended = true;
                                finded = true;
                            }
                            else{}
                        }
                    }
                    if(!finded){ExpressionButton.text = expressionArray[0];}
                }
            });

 


            titleContainer.Insert(2, ExpressionButton);
            titleContainer.Insert(1, ConversantButton);


            /* INPUT CONTAINER */

            inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */

            customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = DSElementUtility.CreateTextArea(Text, null, callback => Text = callback.newValue);

            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            //Port inputPort = (Port) inputContainer.Children().First();

            //return !inputPort.connected;
            return false;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
    }
}
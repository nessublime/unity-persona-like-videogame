using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSPartyNode : DSNode
    {
        //private string multiChoiceText = "/";

        public DSChoiceSaveData choiceData1;
        public DSChoiceSaveData choiceData2;
        public DSChoiceSaveData choiceData3;
        public DSChoiceSaveData choiceData4;

        //public Port[] choicesPorts = new Port[4];


        private Button[] statRequisiteButton = new Button[4];
        private Button[] statNumberButton = new Button[4];
        private TextField[] textField = new TextField[4];
        //private Button statNumberButton = null;
        public Button PriorityButton;

        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DSDialogueType.Party;

            choiceData1 = new DSPartySaveData(){Text = "New Choice"};
            Choices.Add(choiceData1);
            choiceData2 = new DSPartySaveData(){Text = "New Choice"};
            Choices.Add(choiceData2);
            choiceData3 = new DSPartySaveData(){Text = "New Choice"};
            Choices.Add(choiceData3);
            choiceData4 = new DSPartySaveData(){Text = "New Choice"};
            Choices.Add(choiceData4);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */

            /* OUTPUT CONTAINER */
            int i = 0;
            foreach (DSPartySaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice, statRequisiteButton[i], statNumberButton[i], textField[i], i);
                outputContainer.Add(choicePort);
                i++;
            }
            PriorityButton = DSElementUtility.CreateButton("  --  ", () =>{
                if(PriorityButton.text == "  --  "){PriorityButton.text = "Herranz";}
                else if(PriorityButton.text == "Herranz"){PriorityButton.text = "Ander";}
                else if(PriorityButton.text == "Ander"){PriorityButton.text = "Sergio";}
                else if(PriorityButton.text == "Sergio"){PriorityButton.text = "  --  ";}
            });
            titleContainer.Remove(ConversantButton); 
            titleContainer.Insert(1,PriorityButton);

            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData, Button thisButton, Button thisButton2, TextField choiceTextField, int i)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            DSPartySaveData choiceData = (DSPartySaveData) userData;

            /*choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });*/
            string name = "";
            if(i == 0){name = "Herranz";}else if(i == 1){name = "Ander";}else if(i == 2){name = "Sergio";}else{name = "Ness";}
            thisButton = DSElementUtility.CreateButton(name, () =>{});
            choiceData.Text = thisButton.text;

            /*choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );*/
            


            //choicePort.Add(choiceTextField);   
            choicePort.Add(thisButton);  
            //      
            
            return choicePort;
        }
    }
}

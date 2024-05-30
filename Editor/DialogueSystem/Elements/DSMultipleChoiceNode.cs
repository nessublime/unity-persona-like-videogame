using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSMultipleChoiceNode : DSNode
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
        public string[] statString = new string[4];
        //private Button statNumberButton = null;

        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DSDialogueType.MultipleChoice;

            choiceData1 = new DSMultipleSaveData(){Text = "New Choice"};
            Choices.Add(choiceData1);
            choiceData2 = new DSMultipleSaveData(){Text = "New Choice"};
            Choices.Add(choiceData2);
            choiceData3 = new DSMultipleSaveData(){Text = "New Choice"};
            Choices.Add(choiceData3);
            choiceData4 = new DSMultipleSaveData(){Text = "New Choice"};
            Choices.Add(choiceData4);
            //Debug.Log(Choices[0].Text);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */

            /* OUTPUT CONTAINER */
            int i = 0;
            foreach (DSMultipleSaveData choice in Choices)
            {
                //Debug.Log(choice.Text);
                Port choicePort = CreateChoicePort(choice, statRequisiteButton[i], statNumberButton[i], textField[i]);
                //Debug.Log(choice.Stat);
                outputContainer.Add(choicePort);
                i++;
            }

            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData, Button thisButton, Button thisButton2, TextField choiceTextField)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            DSMultipleSaveData choiceData = (DSMultipleSaveData) userData;
            //Debug.Log(choiceData.Text);
            if(choiceData.Stat == null){choiceData.Stat = "/";}

            thisButton = DSElementUtility.CreateButton(choiceData.Stat, () =>
            {
                if(choiceTextField.text == ""){thisButton.text = "-";thisButton2.text = "-";}
                else{
                    if(thisButton.text == "-"){thisButton.text = "/";thisButton2.text = "/";}
                    else if(thisButton.text == "/"){thisButton.text = "E";thisButton2.text = "0";}
                    else if(thisButton.text == "E"){thisButton.text = "T";}
                    else if(thisButton.text == "T"){thisButton.text = "A";}
                    else if(thisButton.text == "A"){thisButton.text = "G";}
                    else if(thisButton.text == "G"){thisButton.text = "L";}
                    else if(thisButton.text == "L"){thisButton.text = "V";}
                    else if(thisButton.text == "V"){thisButton.text = "/";thisButton2.text = "/";}
                }
                choiceData.Stat = thisButton.text;
                choiceData.StatValue = thisButton2.text;
                //Debug.Log(choiceData.Stat);
            });
            if(choiceData.StatValue == null){choiceData.StatValue = "/";}
            thisButton2 = DSElementUtility.CreateButton(choiceData.StatValue, () =>
            {
                if(choiceTextField.text == ""){thisButton.text = "-";thisButton2.text = "-";}
                else{
                    if(thisButton.text == "/"){thisButton2.text = "/";}
                    else{
                        if(thisButton2.text == "-"){thisButton.text = "/";thisButton2.text = "/";}
                        else if(thisButton2.text == "0"){thisButton2.text = "1";}
                        else if(thisButton2.text == "1"){thisButton2.text = "2";}
                        else if(thisButton2.text == "2"){thisButton2.text = "3";}
                        else if(thisButton2.text == "3"){thisButton2.text = "4";}
                        else if(thisButton2.text == "4"){thisButton2.text = "5";}
                        else if(thisButton2.text == "5"){thisButton2.text = "6";}
                        else if(thisButton2.text == "6"){thisButton2.text = "7";}
                        else if(thisButton2.text == "7"){thisButton2.text = "8";}
                        else if(thisButton2.text == "8"){thisButton2.text = "9";}
                        else if(thisButton2.text == "9"){thisButton2.text = "0";}
                    }
                }
                choiceData.Stat = thisButton.text;
                choiceData.StatValue = thisButton2.text;
            });

            
            thisButton2.AddToClassList("ds-node__button");
            thisButton.AddToClassList("ds-node__button");

            choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            

            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(thisButton2);
            choicePort.Add(thisButton);
            
            
            return choicePort;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Utilities
{
    using Data;
    using Data.Save;
    using Elements;
    using ScriptableObjects;
    using Windows;
    using Enumerations;

    public static class DSIOUtility
    {
        private static DSGraphView graphView;

        private static string graphFileName;
        private static string containerFolderPath;

        private static List<DSNode> nodes;
        private static List<DSGroup> groups;

        private static Dictionary<string, DSDialogueGroupSO> createdDialogueGroups;
        private static Dictionary<string, DSDialogueSO> createdDialogues;

        private static Dictionary<string, DSGroup> loadedGroups;
        private static Dictionary<string, DSNode> loadedNodes;

        public static void Initialize(DSGraphView dsGraphView, string graphName)
        {
            graphView = dsGraphView;

            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphName}";

            nodes = new List<DSNode>();
            groups = new List<DSGroup>();

            createdDialogueGroups = new Dictionary<string, DSDialogueGroupSO>();
            createdDialogues = new Dictionary<string, DSDialogueSO>();

            loadedGroups = new Dictionary<string, DSGroup>();
            loadedNodes = new Dictionary<string, DSNode>();
        }

        public static void Save()
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();

            DSGraphSaveDataSO graphData = CreateAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");

            graphData.Initialize(graphFileName);

            DSDialogueContainerSO dialogueContainer = CreateAsset<DSDialogueContainerSO>(containerFolderPath, graphFileName);

            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveGroups(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (DSGroup group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, dialogueContainer);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToGraph(DSGroup group, DSGraphSaveDataSO graphData)
        {
            DSGroupSaveData groupData = new DSGroupSaveData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };

            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToScriptableObject(DSGroup group, DSDialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;

            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DSDialogueGroupSO dialogueGroup = CreateAsset<DSDialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

            dialogueGroup.Initialize(groupName);

            createdDialogueGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DSDialogueSO>());

            SaveAsset(dialogueGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, DSGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (DSNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                    continue;
                }

                ungroupedNodeNames.Add(node.DialogueName);
            }

            UpdateDialoguesChoicesConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToGraph(DSNode node, DSGraphSaveDataSO graphData)
        {
            List<DSChoiceSaveData> choices = CloneNodeChoices(node); 
            DSNodeSaveData nodeData = FillNodeData(node);
            
            graphData.Nodes.Add(nodeData);
            //graphData.Nodes.Add(FillNodeData(node));
        }
        private static DSNodeSaveData FillNodeData(DSNode node){
            switch(node.DialogueType){
                default:
                    DSNodeSaveData nodeData0 = new DSNodeSaveData(){};
                    return nodeData0;
                case DSDialogueType.SingleChoice:
                    DSSingleChoiceSaveData nodeData1 = new DSSingleChoiceSaveData()
                    {
                        ID = node.ID,
                        GroupID = node.Group?.ID,
                        Name = node.DialogueName,
                        Choices = node.Choices,
                        DialogueType = node.DialogueType,
                        Text = node.Text,
                        Conversant = node.ConversantButton.text,
                        Expression = node.ExpressionButton.text,
                        Position = node.GetPosition().position
                    };
                    return nodeData1;
                case DSDialogueType.MultipleChoice:
                    DSMultipleChoiceNode multiNode = (DSMultipleChoiceNode)node;
                    Debug.Log(multiNode.Choices[0].NodeID);
                    Debug.Log(multiNode.Choices[0].Text);
                    DSMultipleChoiceSaveData nodeData2 = new DSMultipleChoiceSaveData()
                    {
                        ID = node.ID,
                        GroupID = node.Group?.ID,
                        Name = node.DialogueName,
                        Choices = multiNode.Choices,
                        DialogueType = node.DialogueType,
                        Text = node.Text,
                        Conversant = node.ConversantButton.text,
                        Expression = node.ExpressionButton.text,
                        Position = node.GetPosition().position
                    };
                    return nodeData2;
                case DSDialogueType.Response:
                    DSResponseNode responseNode = (DSResponseNode)node;
                    DSResponseChoiceSaveData nodeData3 = new DSResponseChoiceSaveData()
                    {
                        ID = node.ID,
                        GroupID = node.Group?.ID,
                        Name = node.DialogueName,
                        Choices = node.Choices,
                        DialogueType = node.DialogueType,
                        Text = node.Text,
                        Expression = node.ExpressionButton.text,
                        Stat = responseNode.aButton.text,
                        StatEXP = responseNode.EXPTextField.text,
                        Position = node.GetPosition().position
                    };
                    return nodeData3;
                case DSDialogueType.Party:
                    DSPartyNode partyNode = (DSPartyNode)node;
                    DSPartyNodeSaveData nodeData4 = new DSPartyNodeSaveData()
                    {
                        ID = node.ID,
                        GroupID = node.Group?.ID,
                        Name = node.DialogueName,
                        Choices = node.Choices,
                        DialogueType = node.DialogueType,
                        Text = node.Text,
                        Priority = partyNode.PriorityButton.text,
                        Expression = node.ExpressionButton.text,
                        Position = node.GetPosition().position
                    };
                    return nodeData4;
                case DSDialogueType.Position:
                    
                    DSPositionNode positionNode = (DSPositionNode)node;
                    DSPositionNodeSaveData nodeData5 = new DSPositionNodeSaveData()
                    {
                        ID = node.ID,
                        GroupID = node.Group?.ID,
                        Name = node.DialogueName,
                        Choices = node.Choices,
                        DialogueType = node.DialogueType,
                        Position = node.GetPosition().position,
                        Positions = positionNode.initialPositions
                    };
                    return nodeData5;
                case DSDialogueType.Master:
                    DSMasterNode masterNode = (DSMasterNode)node;
                    DSMasterNodeSaveData nodeData6 = new DSMasterNodeSaveData()
                    {
                        ID = node.ID,
                        GroupID = node.Group?.ID,
                        Name = node.DialogueName,
                        Choices = node.Choices,
                        DialogueType = node.DialogueType,
                        Position = node.GetPosition().position,
                        Positions = masterNode.initialPositions,
                        Conversants = masterNode.initialConversants
                    };
                    return nodeData6;
            }
        }
        private static void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer)
        {
            switch(node.DialogueType){


                case(DSDialogueType.SingleChoice) :
                    DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode) node;
                    if (node.Group != null){
                        DSSingleChoiceDialogueSO singleChoiceDialogue1 = CreateAsset<DSSingleChoiceDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                        dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], singleChoiceDialogue1);
                        singleChoiceDialogue1.Initialize(
                            singleChoiceNode.DialogueName,
                            singleChoiceNode.DialogueType,
                            ConvertSingleNodeChoicesToDialogueChoices(singleChoiceNode),
                            singleChoiceNode.Text,
                            singleChoiceNode.ConversantButton.text,
                            singleChoiceNode.ExpressionButton.text
                        );
                        createdDialogues.Add(node.ID, singleChoiceDialogue1);
                        SaveAsset(singleChoiceDialogue1);
                        break;
                    }else{
                        DSSingleChoiceDialogueSO singleChoiceDialogue2 = CreateAsset<DSSingleChoiceDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                        dialogueContainer.UngroupedDialogues.Add(singleChoiceDialogue2);
                        singleChoiceDialogue2.Initialize(
                            singleChoiceNode.DialogueName,
                            singleChoiceNode.DialogueType,
                            ConvertSingleNodeChoicesToDialogueChoices(singleChoiceNode),
                            singleChoiceNode.Text,
                            singleChoiceNode.ConversantButton.text,
                            singleChoiceNode.ExpressionButton.text
                        );
                        createdDialogues.Add(node.ID, singleChoiceDialogue2);
                        SaveAsset(singleChoiceDialogue2);
                        break;
                    }


                case(DSDialogueType.MultipleChoice):
                    DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode) node;
                    if (node.Group != null){
                        DSMultipleChoiceDialogueSO multipleChoiceDialogue1 = CreateAsset<DSMultipleChoiceDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                        dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], multipleChoiceDialogue1);
                        multipleChoiceDialogue1.Initialize(
                            multipleChoiceNode.DialogueName,
                            multipleChoiceNode.DialogueType,
                            ConvertMultiNodeChoicesToDialogueChoices(multipleChoiceNode),
                            multipleChoiceNode.Text,
                            multipleChoiceNode.ConversantButton.text,
                            multipleChoiceNode.ExpressionButton.text
                        );
                        createdDialogues.Add(node.ID, multipleChoiceDialogue1);
                        SaveAsset(multipleChoiceDialogue1);
                        break;
                    }else{
                        DSMultipleChoiceDialogueSO multipleChoiceDialogue2 = CreateAsset<DSMultipleChoiceDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                        dialogueContainer.UngroupedDialogues.Add(multipleChoiceDialogue2);
                        multipleChoiceDialogue2.Initialize(
                            multipleChoiceNode.DialogueName,
                            multipleChoiceNode.DialogueType,
                            ConvertMultiNodeChoicesToDialogueChoices(multipleChoiceNode),
                            multipleChoiceNode.Text,
                            multipleChoiceNode.ConversantButton.text,
                            multipleChoiceNode.ExpressionButton.text
                        );
                        createdDialogues.Add(node.ID, multipleChoiceDialogue2);
                        SaveAsset(multipleChoiceDialogue2);
                        break;
                    }


                case(DSDialogueType.Response):
                    DSResponseNode responseNode = (DSResponseNode) node;
                    if (node.Group != null){
                        DSResponseDialogueSO responseDialogue1 = CreateAsset<DSResponseDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                        dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], responseDialogue1);
                        responseDialogue1.Initialize(
                            responseNode.DialogueName,
                            responseNode.DialogueType,
                            ConvertResponseNodeChoicesToDialogueChoices(responseNode),
                            responseNode.Text,
                            responseNode.ExpressionButton.text,
                            responseNode.aButton.text,
                            responseNode.EXPTextField.text
                        );
                        createdDialogues.Add(node.ID, responseDialogue1);
                        SaveAsset(responseDialogue1);
                        break;                        
                    }else{
                        DSResponseDialogueSO responseDialogue2 = CreateAsset<DSResponseDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                        dialogueContainer.UngroupedDialogues.Add(responseDialogue2);
                        responseDialogue2.Initialize(
                            responseNode.DialogueName,
                            responseNode.DialogueType,
                            ConvertResponseNodeChoicesToDialogueChoices(responseNode),
                            responseNode.Text,
                            responseNode.ExpressionButton.text,
                            responseNode.aButton.text,
                            responseNode.EXPTextField.text
                        );  
                        createdDialogues.Add(node.ID, responseDialogue2);
                        SaveAsset(responseDialogue2);
                        break;  
                    }
                    

                case(DSDialogueType.Party):
                    DSPartyNode partyNode = (DSPartyNode) node;
                    if (node.Group != null){
                        DSPartyDialogueSO partyDialogue1 = CreateAsset<DSPartyDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                        dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], partyDialogue1);
                        partyDialogue1.Initialize(
                            partyNode.DialogueName,
                            partyNode.DialogueType,
                            ConvertPartyNodeChoicesToDialogueChoices(partyNode),
                            partyNode.Text,
                            partyNode.ExpressionButton.text,
                            partyNode.PriorityButton.text
                        );
                        createdDialogues.Add(node.ID, partyDialogue1);
                        SaveAsset(partyDialogue1);
                        break;
                    }else{
                        DSPartyDialogueSO partyDialogue2 = CreateAsset<DSPartyDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                        dialogueContainer.UngroupedDialogues.Add(partyDialogue2);
                        partyDialogue2.Initialize(
                            partyNode.DialogueName,
                            partyNode.DialogueType,
                            ConvertPartyNodeChoicesToDialogueChoices(partyNode),
                            partyNode.Text,
                            partyNode.ExpressionButton.text,
                            partyNode.PriorityButton.text
                        );
                        createdDialogues.Add(node.ID, partyDialogue2);
                        SaveAsset(partyDialogue2);
                        break;
                    }
                    
                    
                case(DSDialogueType.Position):
                    DSPositionNode positionNode = (DSPositionNode) node;
                    if (node.Group != null){
                        DSPositionDialogueSO positionDialogue1 = CreateAsset<DSPositionDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                        dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], positionDialogue1);
                        positionDialogue1.Initialize(
                            positionNode.DialogueName,
                            positionNode.DialogueType,
                            ConvertPositionNodeChoicesToDialogueChoices(positionNode),
                            positionNode.initialPositions
                        );
                        createdDialogues.Add(node.ID, positionDialogue1);
                        SaveAsset(positionDialogue1);
                        break;
                    }else{
                        DSPositionDialogueSO positionDialogue2 = CreateAsset<DSPositionDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                        dialogueContainer.UngroupedDialogues.Add(positionDialogue2);
                        positionDialogue2.Initialize(
                            positionNode.DialogueName,
                            positionNode.DialogueType,
                            ConvertPositionNodeChoicesToDialogueChoices(positionNode),
                            positionNode.initialPositions
                        );
                        createdDialogues.Add(node.ID, positionDialogue2);
                        SaveAsset(positionDialogue2);
                        break;
                    }


                case(DSDialogueType.Master):
                    DSMasterNode masterNode = (DSMasterNode) node;
                    if (node.Group != null){
                        DSMasterDialogueSO masterDialogue1 = CreateAsset<DSMasterDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                        dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], masterDialogue1);
                        masterDialogue1.Initialize(
                            masterNode.DialogueName,
                            masterNode.DialogueType,
                            ConvertMasterNodeChoicesToDialogueChoices(masterNode),
                            masterNode.initialPositions,
                            masterNode.initialConversants
                        );
                        createdDialogues.Add(node.ID, masterDialogue1);
                        SaveAsset(masterDialogue1);
                        break;
                    }else{
                        DSMasterDialogueSO masterDialogue2 = CreateAsset<DSMasterDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                        dialogueContainer.UngroupedDialogues.Add(masterDialogue2);
                        masterDialogue2.Initialize(
                            masterNode.DialogueName,
                            masterNode.DialogueType,
                            ConvertMasterNodeChoicesToDialogueChoices(masterNode),
                            masterNode.initialPositions,
                            masterNode.initialConversants
                        );
                        createdDialogues.Add(node.ID, masterDialogue2);
                        SaveAsset(masterDialogue2);
                        break;
                    } 
            }    
        }
        private static List<DSSingleChoiceData> ConvertSingleNodeChoicesToDialogueChoices(DSNode node){
            List<DSSingleChoiceData> dialogueChoices = new List<DSSingleChoiceData>();
            foreach (DSChoiceSaveData nodeChoice in node.Choices){
                DSSingleSaveData nodeChoice1 = (DSSingleSaveData) nodeChoice;
                DSSingleChoiceData choiceData1 = new DSSingleChoiceData(){
                    Text = nodeChoice1.Text
                };
                dialogueChoices.Add(choiceData1);
            }
            return dialogueChoices;
        }
        private static List<DSMultiChoiceData> ConvertMultiNodeChoicesToDialogueChoices(DSNode node){
            List<DSMultiChoiceData> dialogueChoices = new List<DSMultiChoiceData>();
            int i = 0;
            foreach (DSChoiceSaveData nodeChoice in node.Choices){
                DSMultipleSaveData nodeChoice2 = (DSMultipleSaveData) nodeChoice;
                DSMultiChoiceData choiceData2 = new DSMultiChoiceData(){
                    Text = nodeChoice2.Text,
                    Stat = nodeChoice2.Stat,
                    StatValue = nodeChoice2.StatValue
                };
                dialogueChoices.Add(choiceData2);
                i++;
            }
            return dialogueChoices;
        }
        private static List<DSSingleChoiceData> ConvertResponseNodeChoicesToDialogueChoices(DSNode node){
            List<DSSingleChoiceData> dialogueChoices = new List<DSSingleChoiceData>();
            foreach (DSChoiceSaveData nodeChoice in node.Choices){
                DSSingleSaveData nodeChoice3 = (DSSingleSaveData) nodeChoice;
                DSSingleChoiceData choiceData3 = new DSSingleChoiceData(){
                    Text = nodeChoice3.Text
                };
                dialogueChoices.Add(choiceData3);
            }
            return dialogueChoices;
        }
        private static List<DSPartyChoiceData> ConvertPartyNodeChoicesToDialogueChoices(DSNode node){
            List<DSPartyChoiceData> dialogueChoices = new List<DSPartyChoiceData>();
            foreach (DSChoiceSaveData nodeChoice in node.Choices){
                DSPartySaveData nodeChoice3 = (DSPartySaveData) nodeChoice;
                DSPartyChoiceData choiceData4 = new DSPartyChoiceData(){
                    Text = nodeChoice3.Text
                };
                dialogueChoices.Add(choiceData4);
            }
            return dialogueChoices;
        }
        private static List<DSPositionChoiceData> ConvertPositionNodeChoicesToDialogueChoices(DSNode node){
            List<DSPositionChoiceData> dialogueChoices = new List<DSPositionChoiceData>();
            foreach (DSChoiceSaveData nodeChoice in node.Choices){
                DSPositionSaveData nodeChoice3 = (DSPositionSaveData) nodeChoice;
                DSPositionChoiceData choiceData5 = new DSPositionChoiceData(){
                };
                dialogueChoices.Add(choiceData5);
            }
            return dialogueChoices;
        }

        private static List<DSSingleChoiceData> ConvertMasterNodeChoicesToDialogueChoices(DSNode node){
            List<DSSingleChoiceData> dialogueChoices = new List<DSSingleChoiceData>();
            Debug.Log(node.Choices[0]);
            foreach (DSChoiceSaveData nodeChoice in node.Choices){
                DSSingleSaveData nodeChoice3 = (DSSingleSaveData) nodeChoice;
                DSSingleChoiceData choiceData6 = new DSSingleChoiceData(){
                    Text = nodeChoice3.Text
                };
                dialogueChoices.Add(choiceData6);
            }
            return dialogueChoices;
        }


        /*private static List<T> ConvertNodeChoicesToDialogueChoices<T>(DSNode node)
        {
            foreach (DSChoiceSaveData nodeChoice in node.Choices)
            {
                switch(node.DialogueType){
                    case DSDialogueType.SingleChoice:
                        DSSingleSaveData nodeChoice1 = (DSSingleSaveData) nodeChoice;
                        DSSingleChoiceData choiceData1 = new DSSingleChoiceData(){
                            Text = nodeChoice1.Text
                        };
                        dialogueChoices.Add(choiceData1);
                        break;
                    case DSDialogueType.MultipleChoice:
                        DSMultipleSaveData nodeChoice2 = (DSMultipleSaveData) nodeChoice;
                        DSMultiChoiceData choiceData2 = new DSMultiChoiceData(){
                            Text = nodeChoice2.Text,
                            Stat = nodeChoice2.Stat,
                            StatValue = nodeChoice2.StatValue
                        };
                        dialogueChoices.Add(choiceData2);
                        break;
                    case DSDialogueType.Response:
                        DSSingleSaveData nodeChoice3 = (DSSingleSaveData) nodeChoice;
                        DSSingleChoiceData choiceData3 = new DSSingleChoiceData(){
                            Text = nodeChoice3.Text
                        };
                        dialogueChoices.Add(choiceData3);
                        break;
                    case DSDialogueType.Party:
                        DSPartySaveData nodeChoice4 = (DSPartySaveData) nodeChoice;
                        DSPartyChoiceData choiceData4 = new DSPartyChoiceData(){
                            Text = nodeChoice4.Text
                        };
                        dialogueChoices.Add(choiceData4);
                        break;
                    case DSDialogueType.Position:
                        DSPositionChoiceData choiceData5 = new DSPositionChoiceData(){
                        };
                        dialogueChoices.Add(choiceData5);
                        break;
                    case DSDialogueType.Master:
                        DSSingleSaveData nodeChoice6 = (DSSingleSaveData) nodeChoice;
                        DSPartyChoiceData choiceData6 = new DSPartyChoiceData()
                        {
                            Text = nodeChoice6.Text
                        };
                        dialogueChoices.Add(choiceData6);
                        break;
                }        
            }

            return dialogueChoices;
        }*/
        /*public static List<T> GetTypedList<T>(){
            List<T> newList = new List<T>();
            return newList;
        }*/
        private static void UpdateDialoguesChoicesConnections()
        {
            foreach (DSNode node in nodes)
            {
                switch(node.DialogueType){

                    case(DSDialogueType.SingleChoice):
                        DSSingleChoiceDialogueSO dialogue1 = (DSSingleChoiceDialogueSO)createdDialogues[node.ID];
                        for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex){
                            DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                            if (string.IsNullOrEmpty(nodeChoice.NodeID)){continue;}
                            dialogue1.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                            SaveAsset(dialogue1);
                        }
                        break;
                    case(DSDialogueType.MultipleChoice):
                        DSMultipleChoiceDialogueSO dialogue2 = (DSMultipleChoiceDialogueSO)createdDialogues[node.ID];
                        for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex){
                            DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                            if (string.IsNullOrEmpty(nodeChoice.NodeID)){continue;}
                            dialogue2.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                            SaveAsset(dialogue2);
                        }
                        break;
                    case(DSDialogueType.Response):
                        DSResponseDialogueSO dialogue3 = (DSResponseDialogueSO)createdDialogues[node.ID];
                        for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex){
                            DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                            if (string.IsNullOrEmpty(nodeChoice.NodeID)){continue;}
                            dialogue3.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                            SaveAsset(dialogue3);
                        }
                        break;
                    case(DSDialogueType.Party):
                        DSPartyDialogueSO dialogue4 = (DSPartyDialogueSO)createdDialogues[node.ID];
                        for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex){
                            DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                            if (string.IsNullOrEmpty(nodeChoice.NodeID)){continue;}
                            dialogue4.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                            SaveAsset(dialogue4);
                        }
                        break;
                    case(DSDialogueType.Position):
                        DSPositionDialogueSO dialogue5 = (DSPositionDialogueSO)createdDialogues[node.ID];
                        for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex){
                            DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                            if (string.IsNullOrEmpty(nodeChoice.NodeID)){continue;}
                            dialogue5.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                            SaveAsset(dialogue5);
                        }
                        break;
                    case(DSDialogueType.Master):
                        DSMasterDialogueSO dialogue6 = (DSMasterDialogueSO)createdDialogues[node.ID];
                        for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex){
                            DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                            if (string.IsNullOrEmpty(nodeChoice.NodeID)){continue;}
                            dialogue6.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                            SaveAsset(dialogue6);
                        }
                        break;
                }
            }
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DSGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DSGraphSaveDataSO graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        public static void Load()
        {
            DSGraphSaveDataSO graphData = LoadAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"Assets/Editor/DialogueSystem/Graphs/{graphFileName}\".\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            DSEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<DSGroupSaveData> groups)
        {
            foreach (DSGroupSaveData groupData in groups)
            {
                DSGroup group = graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<DSNodeSaveData> nodes)
        {
            foreach (DSNodeSaveData nodeData in nodes)
            {
                List<DSChoiceSaveData> choices = CloneNodeChoices(nodeData);

                DSNode node = graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false);


                node.ID = nodeData.ID;
                node.Choices = choices;
                //Debug.Log(node.Choices[0].Stat);
                switch(node.DialogueType){
                    default:
                        break;
                    case DSDialogueType.SingleChoice:
                        DSSingleChoiceNode singleNode = (DSSingleChoiceNode) node;
                        singleNode.Text = nodeData.Text;
                        break;
                    case DSDialogueType.MultipleChoice:
                        DSMultipleChoiceNode multiNode = (DSMultipleChoiceNode) node;
                        multiNode.Text = nodeData.Text;
                        break;
                    case DSDialogueType.Response:
                        DSResponseNode responseNode = (DSResponseNode) node;
                        responseNode.Text = nodeData.Text;
                        break;
                    case DSDialogueType.Party:
                        DSPartyNode partyNode = (DSPartyNode) node;
                        partyNode.Text = nodeData.Text;
                        break;
                    case DSDialogueType.Master:
                        DSMasterNode masterNode = (DSMasterNode) node;
                        masterNode.initialConversants = nodeData.Conversants;
                        break;
                    case DSDialogueType.Position:
                        DSPositionNode positionNode = (DSPositionNode) node;
                        break;
                }
                node.Draw();

                graphView.AddElement(node);
                //Debug.Log(node.outputContainer.Children().NodeID);
                loadedNodes.Add(node.ID, node);
                switch(node.DialogueType){
                    default:
                        break;
                    case DSDialogueType.SingleChoice:
                        DSSingleChoiceNode singleNode = (DSSingleChoiceNode) node;
                        node.ExpressionButton.text = nodeData.Expression;
                        node.ConversantButton.text = nodeData.Conversant;
                        break;
                    case DSDialogueType.MultipleChoice:
                        DSMultipleChoiceNode multiNode = (DSMultipleChoiceNode) node;
                        node.ExpressionButton.text = nodeData.Expression;
                        node.ConversantButton.text = nodeData.Conversant;

                        break;
                    case DSDialogueType.Response:
                        DSResponseNode responseNode = (DSResponseNode) node;
                        responseNode.aButton.text = nodeData.Stat;
                        responseNode.EXPTextField.value = nodeData.StatEXP; 
                        node.ExpressionButton.text = nodeData.Expression;
                        break;
                    case DSDialogueType.Party:
                        DSPartyNode partyNode = (DSPartyNode) node;
                        partyNode.PriorityButton.text = nodeData.Priority;
                        node.ExpressionButton.text = nodeData.Expression;
                        break;
                    case DSDialogueType.Master:
                        DSMasterNode masterNode = (DSMasterNode) node;
                        masterNode.RFName.text = nodeData.Positions[4];
                        masterNode.RBName.text = nodeData.Positions[3];
                        masterNode.CName.text = nodeData.Positions[2];
                        masterNode.LBName.text = nodeData.Positions[1];
                        masterNode.LFName.text = nodeData.Positions[0];
                        
                        break;
                    case DSDialogueType.Position:
                        DSPositionNode positionNode = (DSPositionNode) node;
                        positionNode.RFName.text = nodeData.Positions[4];
                        positionNode.RBName.text = nodeData.Positions[3];
                        positionNode.CName.text = nodeData.Positions[2];
                        positionNode.LBName.text = nodeData.Positions[1];
                        positionNode.LFName.text = nodeData.Positions[0];
                        break;
                }
                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                DSGroup group = loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }
        
        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, DSNode> loadedNode in loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    DSChoiceSaveData choiceData = (DSChoiceSaveData) choicePort.userData;
                    //Debug.Log("entro");

                    if (string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        
                        continue;
                    }
                    //Debug.Log("entro2");
                    DSNode nextNode = loadedNodes[choiceData.NodeID];

                    Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                    Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        private static void CreateDefaultFolders()
        {
            CreateFolder("Assets/Editor/DialogueSystem", "Graphs");

            CreateFolder("Assets", "DialogueSystem");
            CreateFolder("Assets/DialogueSystem", "Dialogues");

            CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }

        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(DSGroup);

            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is DSNode node)
                {
                    nodes.Add(node);

                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    DSGroup group = (DSGroup) graphElement;

                    groups.Add(group);

                    return;
                }
            });
        }

        public static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static List<DSChoiceSaveData> CloneNodeChoices(DSNodeSaveData node)
        {
            List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

            foreach (DSChoiceSaveData choice in node.Choices)
            {
                switch(node.DialogueType){

                    case DSDialogueType.SingleChoice:
                        DSSingleSaveData choiceData1 = new DSSingleSaveData()
                        {
                            NodeID = choice.NodeID
                        };
                        choices.Add(choiceData1);
                        break;
                    case DSDialogueType.MultipleChoice:
                        DSMultipleSaveData choiceData2 = new DSMultipleSaveData()
                        {
                            NodeID = choice.NodeID,
                            Text = choice.Text,
                            Stat = choice.Stat,
                            StatValue = choice.StatValue
                        };
                        choices.Add(choiceData2);
                        break;
                    case DSDialogueType.Response:
                        DSSingleSaveData choiceData3 = new DSSingleSaveData()
                        {
                            NodeID = choice.NodeID
                        };
                        choices.Add(choiceData3);
                        break;
                    case DSDialogueType.Party:
                        DSPartySaveData choiceData4 = new DSPartySaveData()
                        {
                            NodeID = choice.NodeID
                        };
                        choices.Add(choiceData4);
                        break;
                    case DSDialogueType.Position:
                        DSPositionSaveData choiceData5 = new DSPositionSaveData()
                        {
                            NodeID = choice.NodeID
                        };
                        choices.Add(choiceData5);
                        break;
                    case DSDialogueType.Master:
                        DSSingleSaveData choiceData6 = new DSSingleSaveData()
                        {
                            //Text = choice.Text,
                            NodeID = choice.NodeID
                        };
                        choices.Add(choiceData6);
                        break;
                }
            }
            return choices;
        }

        private static List<DSChoiceSaveData> CloneNodeChoices(DSNode node)
        {
            List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

            foreach (DSChoiceSaveData choice in node.Choices)
            {
                switch(node.DialogueType){

                    case DSDialogueType.SingleChoice:
                        DSSingleSaveData choiceData1 = new DSSingleSaveData()
                        {
                            //Rellenar
                        };
                        choices.Add(choiceData1);
                        break;
                    case DSDialogueType.MultipleChoice:
                        DSMultipleSaveData choiceData2 = new DSMultipleSaveData()
                        {
                            //Rellenar
                        };
                        choices.Add(choiceData2);
                        break;
                    case DSDialogueType.Response:
                        DSSingleSaveData choiceData3 = new DSSingleSaveData()
                        {
                            //Rellenar
                        };
                        choices.Add(choiceData3);
                        break;
                    case DSDialogueType.Party:
                        DSPartySaveData choiceData4 = new DSPartySaveData()
                        {
                            //Rellenar
                        };
                        choices.Add(choiceData4);
                        break;
                    case DSDialogueType.Position:
                        DSPositionSaveData choiceData5 = new DSPositionSaveData()
                        {
                            //Rellenar
                        };
                        choices.Add(choiceData5);
                        break;
                    case DSDialogueType.Master:
                        DSChoiceSaveData choiceData6 = new DSChoiceSaveData()
                        {
                            //Text = choice.Text,
                            //NodeID = choice.NodeID
                        };
                        choices.Add(choiceData6);
                        break;
                }
            }
            return choices;
        }
    }
}
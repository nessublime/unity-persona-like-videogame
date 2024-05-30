using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : Singleton<SceneMaster>, ISaveableComponent
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    private static VirtualSceneSO actualVirtualScene;
    
    private GameMaster.PlayMode actualPlayMode;

    //********************************    SAVEABLE ATTRIBUTES    ********************************//
    private int actualDay;
    private int actualDate;

    //*********************************    EVENT ATTRIBUTES    **********************************//
    [SerializeField] private VoidEventSO sceneLoadedEvent;
    
    //*********************************        STRUCTURE        *********************************//
    
    
    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    //***********************************    AWAKE & START    ***********************************//
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){sceneLoadedEvent.Raise();}

    protected override void Awake(){
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        actualPlayMode = GameObject.Find("GameMaster").GetComponent<GameMaster>().actualPlayMode;
    }

    protected void Start(){
        switch(actualPlayMode){
            case GameMaster.PlayMode.DEFAULT: LoadNextScene(actualDate);
                break;
            case GameMaster.PlayMode.TEST: sceneLoadedEvent.Raise();
                break;
            case GameMaster.PlayMode.CUSTOM: LoadNextScene(actualDate);
                break;
        }
    }

    //**************************************    UPDATE    ***************************************//
    protected void Update(){

    }
    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//
    //************************************    EVENT METODS     **********************************//
    public static void LoadNextScene(string nextSceneName){SceneManager.LoadScene(nextSceneName);}
    public static void LoadNextScene(int nextSceneIndex){SceneManager.LoadScene(nextSceneIndex);}

    //*******************************     SAVE SYSTEM METHODS     *******************************//
    public object CreateState(){return GameObject.Find("GameMaster").GetComponent<GameMaster>().customSaveFileData.sceneMasterSaveData;}

    public object SaveState(){
        return new SaveData(){
            actualDay = this.actualDay,
            actualDate = this.actualDate
        };
    }

    public void LoadState(object state){
        var saveData = (SaveData)state;
        actualDay = saveData.actualDay;
        actualDate = saveData.actualDate;
    }

    [Serializable] public struct SaveData{
        public int actualDay;
        public int actualDate;
    }
}








/*
//ATTRIBUTES

    //private static int lastDaySleepHour = -1;//Sleep Mecanic NotImplemented
    //private static bool inFreeScene = false;
    
    //private UIMaster UIM;
    //[SerializeField] private List<WeekSO> Weeks = new List<WeekSO>();
    //private static TimeDataStruct lastTime;
    //private static TimeDataStruct actualTime;

    //private static int hourCount;
    //private static int actualDateDuration;
    //private static int actualDateInitialHour;

    //[SerializeField] private VoidEventSO dayChangeEvent;
    //[SerializeField] private TimeEventSO timeUpdateEvent;
    //private static TimeEventSO staticTimeUpdateEvent;

    //private static int nextPlayerSpawnerIndex;


//STRUCT
    [System.Serializable] public struct TimeDataStruct{
        public int hour;
        public int minute;
    }
    /*[System.Serializable] public struct SceneChangeRequestStruct{
        public VirtualSceneSO nextScene;
        public int nextDollycamIndex;
        public int nextPlayerSpawnerIndex;
    }*/


// TIME METHODS

    /*public static void ConsumeTime(int newMinute){
        int actualMinutes = actualTime.minute + newMinute;
        if(actualMinutes >= 60){actualTime.hour++;actualTime.minute = actualMinutes-60;}
        else{actualTime.minute = actualMinutes;}
        staticTimeUpdateEvent.Raise(actualTime);
    }
    public static string TimeDataStructToString(TimeDataStruct timeStruct){
        string str1; string str2; string output;
        if(timeStruct.hour < 10){str1 = "0" + timeStruct.hour.ToString();} else{str1 = timeStruct.hour.ToString();}
        if(timeStruct.minute < 10){str2 = "0" + timeStruct.minute.ToString();} else{str2 = timeStruct.minute.ToString();}
        output = str1+":"+str2;
        return output;
    }
    public static float TimeDataStructToFloat(TimeDataStruct timeStruct){
        float hora;
        hora = (float)timeStruct.hour + (float)(timeStruct.minute)/60;
        return hora;
    }
    private void CheckTimeChange(){
        if(actualTime.hour!=lastTime.hour && actualTime.minute!=lastTime.minute){
            OnTimeUpdate();
            UIM.UpdateTimeInfo(actualTime);
            lastTime = actualTime;
        }
    }*/


    /*public void LoadNextDayEvent(SceneChangeRequestStruct data){
        //actualDay++;
        //hourCount = 0;
        //SetActualVirtualScene(ReadTimetable());
        CameraMaster.SetNextDollyTrackIndex(data.nextDollycamIndex); 
        LoadNextScene(actualVirtualScene.sceneIndex);
        //UIM.UpdateMainInfo(actualVirtualScene.sceneName,actualTime);
        //OnDayChange(); Mirar a ver
    }*/


    /*private async void CheckFreeSceneState(){
        if(inFreeScene){if(TimeDataStructToFloat(actualTime) == GetActualDateEndingHour()){
            bool response = await UIM.ShowAlert(endFreeSceneData); 
            inFreeScene = false;
            SceneChangeRequestStruct nextDateStruct = new SceneChangeRequestStruct();
            LoadNextDateEvent(nextDateStruct);
        }}
    }

    
    
    /*public void LoadNextDateEvent(SceneChangeRequestStruct data){
        //SetNextPlayerSpawnerIndex(data.nextPlayerSpawnerIndex);
        CameraMaster.SetNextDollyTrackIndex(data.nextDollycamIndex);
        LoadNextScene(actualVirtualScene.sceneIndex);
    }

    public void LoadNextSceneEvent(SceneChangeRequestStruct data){
        //SetNextPlayerSpawnerIndex(data.nextPlayerSpawnerIndex);
        //SetActualVirtualScene(data.nextScene);
        CameraMaster.SetNextDollyTrackIndex(data.nextDollycamIndex);
        LoadNextScene(actualVirtualScene.sceneIndex);
    }*/
    
    
    
    
    //public static void SetActualDate(int dateIndex){actualDate = dateIndex;}
    //public static int GetActualDate(){return actualDate;} 

    //public static void SetActualDay(int day){actualDay = day;}
    //public static int GetActualDay(){return actualDay;}

    //public static void SetActualWeek(int week){actualWeek = week;}
    //public static void SetNextWeek(){actualWeek++;}
    //public static int GetActualWeek(){return actualWeek;}
    //public static void SetActualTime(int newHour,int newMinute){actualTime.hour=newHour;actualTime.minute=newMinute;}
    //public static TimeDataStruct GetActualTime(){return actualTime;}

    //public static void SetActualHour(int newHour){actualTime.hour = newHour;}
    //public static int GetActualHour(){return actualTime.hour;}

    //public static void SetActualMinute(int newMinute){actualTime.minute = newMinute;}
    //public static int GetActualMinute(){return actualTime.minute;}
    //public static int GetActualDateDuration(){return actualDateDuration;}
    //public static int GetActualDateInitialHour(){return actualDateInitialHour;}
    //public static int GetActualDateEndingHour(){return actualDateInitialHour + actualDateDuration;}
    
    /*public static void SetNextPlayerSpawnerIndex(int index){nextPlayerSpawnerIndex = index;}
    public static int GetActualPlayerSpawnerIndex(){return nextPlayerSpawnerIndex;}
    public static void SetActualVirtualScene(VirtualSceneSO scene){actualVirtualScene = scene;}
    public static VirtualSceneSO GetActualVirtualScene(){return actualVirtualScene;}*/
    
    
    /*private VirtualSceneSO ReadTimetable(){bool end = false;
        VirtualSceneSO nextDateInitialScene = null;
        while(!end){
            var nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
            if(nextDate.initialScene.sceneName == "DayStart"){
                //Reescribir entero
                int initWakeHour = nextDate.horaInicial;
                var breakfastScene = nextDate.outFreeScene;
                hourCount++;
                nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                while(nextDate.initialScene.sceneName == "DayStart"){
                    breakfastScene = nextDate.outFreeScene;
                    actualDate = hourCount;
                    hourCount++;
                    nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                }
                nextDateInitialScene = breakfastScene;
                int sleepTime = initWakeHour - lastDaySleepHour;
                if(sleepTime>=8 || initWakeHour==nextDate.horaInicial-1){SetActualTime(initWakeHour,0);}
                else{
                    while(sleepTime<8 && initWakeHour!=nextDate.horaInicial-1){
                        initWakeHour++;
                        sleepTime++;
                        SetActualTime(initWakeHour,0);
                    }
                }
                end = true;
                //******************************
                //Null limita la hora inicial 
                //Free limita la hora final 
                /
                //inFreeScene = true;
                actualDateInitialHour = nextDate.horaInicial;
                var breakfastScene = nextDate.outFreeScene;
                actualDate = hourCount;
                actualDateDuration = 1;
                hourCount++;
                nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                while(nextDate.initialScene.sceneName == "DayStart"){
                    breakfastScene = nextDate.outFreeScene;
                    actualDateDuration++;
                    hourCount++;
                    nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                }
                while(nextDate.initialScene.sceneName == "Free"){
                    actualDateDuration++;
                }
                SetActualTime(actualDateInitialHour,0);
                nextDateInitialScene = breakfastScene;
                end = true;






            }
            else if(nextDate.initialScene.sceneName == "Free"){
                //inFreeScene = true;                
                actualDateInitialHour = nextDate.horaInicial;
                SetActualTime(actualDateInitialHour,0);
                nextDateInitialScene = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount-1].outFreeScene;
                actualDateDuration = 1;
                actualDate = hourCount;
                hourCount++;
                nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                while(nextDate.initialScene.sceneName == "Free"){
                    hourCount++;
                    actualDateDuration++;
                    nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                }
                end = true;
            }
            else if(nextDate.initialScene.sceneName == "Null"){
                hourCount++;
            }
            else{
                SetActualTime(nextDate.horaInicial,0);
                nextDateInitialScene = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount].initialScene;
                actualDateDuration = 1;
                actualDate = hourCount;
                hourCount++;
                nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                while(nextDate.initialScene.sceneName == "Null"){
                    hourCount++;
                    actualDateDuration++;
                    nextDate = Weeks[actualWeek].daySOArray[actualDay].Timetable[hourCount];
                }
                end = true;
            }
        }
        return nextDateInitialScene;
    }
    
    //private void OnDayChange(){dayChangeEvent.Raise();} Mirar.. otro evento?
    //public void OnTimeUpdate(){timeUpdateEvent.Raise(actualTime);}


    */

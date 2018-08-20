using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{

    public GameState currentGameData;
    static GameStateManager Singleton;

    public static GameStateManager GetInstance()
    {
        return Singleton;
    }

    private void Awake()
    {
        if(Singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/player.sav", FileMode.Create, FileAccess.Write);
        bf.Serialize(stream, currentGameData);
        stream.Close();
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/player.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/player.sav", FileMode.Open, FileAccess.Read);
            currentGameData = (GameState)bf.Deserialize(stream);
            stream.Close();

            GoToLevel(currentGameData.sceneIndex);
        }
    }

    //Button/trigger functions
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public void NewGame()
    {
        currentGameData = new GameState(10, 10, 10, 1, 1);
        GoToLevel(currentGameData.sceneIndex);
    }

    public void IncreaseBody()
    {
        currentGameData.pcBody += 5;
    }

}

[Serializable]
public class GameState
{
    public float pcBody;
    public float pcArms;
    public float pcTech;
    public int equippedWeaponID;
    public int sceneIndex;

    public GameState(float baseBody = 10, float baseArms = 10, float baseTech = 10, int baseWepID = 0, int scInd = 1)
    {
        pcBody = baseBody;
        pcArms = baseArms;
        pcTech = baseTech;
        equippedWeaponID = baseWepID;
        sceneIndex = scInd;
    }

    /*
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("pc_Body", pcBody);
        info.AddValue("pc_Speed", pcSpeed);
        info.AddValue("pc_Mind", pcMind);
        info.AddValue("pc_itemIDs", itemIDs);
        info.AddValue("pc_eqWepID", equippedWeaponID);
        info.AddValue("pc_eqArmID", equippedArmorID);
        info.AddValue("pc_sceneIndex", sceneIndex);
    }

    public GameState(SerializationInfo info, StreamingContext context)
    {
        pcBody = (float)info.GetValue("pc_Body", typeof(float));
        pcSpeed = (float)info.GetValue("pc_Speed", typeof(float));
        pcMind = (float)info.GetValue("pc_Mind", typeof(float));
        itemIDs = (int)info.GetValue("pc_itemIDs", typeof(int));
        equippedWeaponID = (int)info.GetValue("pc_eqWepID", typeof(int));
        equippedArmorID = (int)info.GetValue("pc_eqArmID", typeof(int));
        sceneIndex = (int)info.GetValue("pc_sceneIndex", typeof(int));
    }
    */
}

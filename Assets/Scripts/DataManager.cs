using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class DataManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    private string saveFilePath;
    [SerializeField] public string playerName;
    


    // Start is called before the first frame update
    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/playerData.Json";
    }

    // Update is called once per frame
    void Update()
    {
        playerName = nameInputField.text;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;

    }

    [SerializeField]
    public void SaveData()
    {
        PlayerData data = new PlayerData();
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            data.playerName = playerName;
        }


    }
}

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
    public MainManager mainManager;
    public static DataManager Instance;
    public int bestScore;

    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // destruye duplicados si los hay
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/playerData.Json";
    }

    // Update is called once per frame
    void Update()
    {
        
        playerName = nameInputField.text;
        
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
        public int bestScore;

    }

    [SerializeField]
    public void SaveData()
    {
        PlayerData data = new PlayerData();
        data.playerName = playerName;
        data.bestScore = bestScore;

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
            data.bestScore = bestScore;
        }


    }
}

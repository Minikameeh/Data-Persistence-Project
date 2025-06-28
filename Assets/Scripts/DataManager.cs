using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class DataManager : MonoBehaviour
{
    private TMP_InputField nameInputField;
    private string saveFilePath;
    public string playerName;
    public int score;


    // Start is called before the first frame update
    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/playerData.Json";
    }

    // Update is called once per frame
    void Update()
    {
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
    
    [SerializeField]
    public void SaveData()
    {
       
        

    }

}

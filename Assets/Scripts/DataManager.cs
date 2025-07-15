using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class DataManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI bestScoreTextMenu;
    private string saveFilePath;
    [SerializeField] public GameObject ErasePoPUp;
    [SerializeField] public GameObject LeaderBoardScreen;
    [SerializeField] public Button startButton;
    [SerializeField] public string playerName;
    public MainManager mainManager;
    public static DataManager Instance;
    public int activePlayerIndex = -1;
    public int bestScore;
    public PlayerList playerList = new PlayerList();

    private void Awake()
    {

        saveFilePath = Application.persistentDataPath + "/playerData.Json";

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();

            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {  
        
        ErasePoPUp.SetActive(false);
        saveFilePath = Application.persistentDataPath + "/playerData.Json";
        LoadData();

       
    }

   
    // Update is called once per frame
    void Update()
    {
        if (nameInputField != null)
        {
            playerName = nameInputField.text;
            bestScoreTextMenu.text = "Best Score: " + playerName + " : " + bestScore;
        } 
       
    }
   
    

    public void StartGame()
    {
        SaveData();
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
        SaveData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    [System.Serializable]
    public class Player
    {
        public string playerName;
        public int bestScore;
    }
   [System.Serializable]
    public class PlayerList
    {
        public Player[] players = new Player[20];
    }



    [SerializeField]
    public void SaveData()
    {
        // Asegurarse de que nameInputField no es null
        if (nameInputField == null)
        {
            GameObject inputFieldObj = GameObject.Find("Player Name");
            if (inputFieldObj != null)
            {
                nameInputField = inputFieldObj.GetComponent<TMP_InputField>();
            }
        }

        // Si sigue siendo null, abortar el guardado
        if (nameInputField == null)
        {
            Debug.LogError("No se encontró el TMP_InputField 'Player Name'. No se puede guardar.");
            return;
        }

        // Leer el texto del input field
        string newName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("El nombre del jugador está vacío. No se guardará nada.");
            return;
        }

        int existingIndex = -1;

        // Buscar si ya existe ese jugador
        for (int i = 0; i < playerList.players.Length; i++)
        {
            if (playerList.players[i].playerName == newName)
            {
                existingIndex = i;
                break;
            }
        }

        if (existingIndex != -1)
        {
            // Actualizar score solo si es mayor
            if (bestScore > playerList.players[existingIndex].bestScore)
            {
                playerList.players[existingIndex].bestScore = bestScore;
            }

            activePlayerIndex = existingIndex;
        }
        else
        {
            // Buscar hueco vacío
            for (int i = 0; i < playerList.players.Length; i++)
            {
                if (string.IsNullOrEmpty(playerList.players[i].playerName))
                {
                    playerList.players[i].playerName = newName;
                    playerList.players[i].bestScore = bestScore;
                    activePlayerIndex = i;
                    break;
                }
            }
        }

        // Guardar en archivo JSON
        string json = JsonUtility.ToJson(playerList, prettyPrint: true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Datos guardados correctamente para el jugador: " + newName);
    }



    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            playerList = JsonUtility.FromJson<PlayerList>(json);

            // Asegúrate de que ningún elemento sea null
            for (int i = 0; i < playerList.players.Length; i++)
            {
                if (playerList.players[i] == null)
                    playerList.players[i] = new Player();
            }
        }
        else
        {
            // Si no existe el archivo, inicializa una lista nueva
            playerList = new PlayerList();
            for (int i = 0; i < playerList.players.Length; i++)
            {
                playerList.players[i] = new Player();
            }
        }


    }

   

    public void DeleteAllData()
    {
        // Borra el archivo si existe
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("All Players Data Erased");
        }

        // Reinicia la lista en memoria
        playerList = new PlayerList();
        for (int i = 0; i < playerList.players.Length; i++)
        {
            playerList.players[i] = new Player();
        }

        // Puedes guardar el estado limpio si quieres
        SaveData();
    }
    
    public List<Player> GetSortedPlayers()
    {
        return playerList.players
            .Where(p => !string.IsNullOrEmpty(p.playerName))
            .OrderByDescending(p => p.bestScore)
            .ToList();
    }
   
    
    

}




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
    [SerializeField] public Button eraseButton;
    [SerializeField] public Button eraseButtonYes;
    [SerializeField] public Button eraseButtonNo;
    [SerializeField] public Button leaderBoardButtonScreen;
    [SerializeField] public Button leaderBoardButtonQuit;
    [SerializeField] public string playerName;
    public MainManager mainManager;
    public static DataManager Instance;
    public int activePlayerIndex = -1;
    public int bestScore;
    public PlayerList playerList = new PlayerList();

    private void Awake()
    {
        
        // Verifica si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
            LoadData();
        }
        else
        {
            Destroy(gameObject); // destruye duplicados si los hay
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        ReconnectUI();
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
        eraseButton.onClick.AddListener(EraseDataPopUp);
        eraseButtonYes.onClick.AddListener(EraseYesButton);
        eraseButtonNo.onClick.AddListener(EraseNoButton);
        leaderBoardButtonScreen.onClick.AddListener(LeaderBoardScreenPopUp);






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
        string newName = nameInputField.text;
        int existingIndex = -1;
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
            // Si el jugador ya existe, actualizar solo su score si es mayor
            if (bestScore > playerList.players[existingIndex].bestScore)
            {
                playerList.players[existingIndex].bestScore = bestScore;
            }

            activePlayerIndex = existingIndex;
        }
        else
        {
            // Si no existe, buscar un hueco vacío y crear uno nuevo
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

        // Guardar a disco
        string json = JsonUtility.ToJson(playerList, prettyPrint: true);
        File.WriteAllText(saveFilePath, json);
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
    
    public void EraseDataPopUp()
    {
        ErasePoPUp.SetActive(true);
    }
    
    public void EraseNoButton()
    {
       
        ErasePoPUp.SetActive(false);
    }

    public void EraseYesButton()
    {
        DeleteAllData();
        ErasePoPUp.SetActive(false);
    }

    public void LeaderBoardScreenPopUp()
    {
        LeaderBoardScreen.SetActive(true);
        leaderBoardButtonScreen.gameObject.SetActive(false);
    }
    public void LeaderBoardScreenQuit()
    {
        LeaderBoardScreen.SetActive(false);
        leaderBoardButtonScreen.gameObject.SetActive(true);
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
    private void ReconnectUI()
    {
        nameInputField = GameObject.Find("Player Name")?.GetComponent<TMP_InputField>();
        bestScoreTextMenu = GameObject.Find("Best Score Menu Text")?.GetComponent<TextMeshProUGUI>();
        startButton = GameObject.Find("Start Button")?.GetComponent<Button>();
        eraseButton = GameObject.Find("Erase Data Button")?.GetComponent<Button>();
        eraseButtonYes = GameObject.Find("Yes")?.GetComponent<Button>();
        eraseButtonNo = GameObject.Find("No")?.GetComponent<Button>();
        leaderBoardButtonScreen = GameObject.Find("LeaderBoard Button")?.GetComponent<Button>();
        leaderBoardButtonQuit = GameObject.Find("LeaderBoardQuitButton")?.GetComponent<Button>();

        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (eraseButton != null)
            eraseButton.onClick.AddListener(EraseDataPopUp);

        if (eraseButtonYes != null)
            eraseButtonYes.onClick.AddListener(EraseYesButton);

        if (eraseButtonNo != null)
            eraseButtonNo.onClick.AddListener(EraseNoButton);

        if (leaderBoardButtonScreen != null)
            leaderBoardButtonScreen.onClick.AddListener(LeaderBoardScreenPopUp);

        if (leaderBoardButtonQuit != null)
            leaderBoardButtonQuit.onClick.AddListener(LeaderBoardScreenQuit);
    }
    
    

}




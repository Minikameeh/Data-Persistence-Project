using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUiHandler : MonoBehaviour
{
    public GameObject dataConteiner;
    public DataManager dataManager;
    public GameObject erasePoPUp;
    public GameObject LeaderBoardScreen;
    public GameObject leaderBoardButtonScreen;

    private void Start()
    {
        dataConteiner = GameObject.Find("DataManager");
        dataManager = dataConteiner.GetComponent<DataManager>();
    }
    
    public void StartGameFromMenu()
    {
        if (dataManager != null)
        {
            dataManager.StartGame();
            Debug.Log("has pulsado el boton start");

        }
        else
        {
            Debug.LogError("No se encontró el DataManager.");
        }
    }




    public void ExitGame()
    {
            

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
        
    }
    
    public void EraseDataPopUp()
    {
        erasePoPUp.SetActive(true);
    }
    
    public void EraseNoButton()
    {
        erasePoPUp.SetActive(false);
    }
    
    public void EraseYesButton()
    {
      dataManager.DeleteAllData();
      erasePoPUp.SetActive(false);
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









}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public GameObject dataContainer;
    public DataManager dataManager;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public int m_Points;
    
    private bool m_GameOver = false;


    


    // Start is called before the first frame update
    void Start()
    {
        
        dataContainer = GameObject.Find("DataManager");
        dataManager = dataContainer.gameObject.GetComponent<DataManager>();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
       
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            var dm = DataManager.Instance;
            int index = dm.activePlayerIndex;
            if (index >= 0 && index < dm.playerList.players.Length)
            {
                // Si m_Points es mayor que el bestScore actual, actualizar
                if (m_Points > dm.playerList.players[index].bestScore)
                {
                    dm.playerList.players[index].bestScore = m_Points;
                    dm.SaveData();
                }
                bestScoreText.text = "Best Score: " +
                    dm.playerList.players[index].playerName + " : " +
                    dm.playerList.players[index].bestScore;
            }

            DataManager.Instance.playerList.players[DataManager.Instance.activePlayerIndex].bestScore = m_Points;
            DataManager.Instance.SaveData();
            bestScoreText.text = "Best Score:" + dataManager.playerList.players[index].playerName + ":" + dataManager.playerList.players[index].bestScore;

            // Reiniciar escena con Space, por ejemplo.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

           
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}

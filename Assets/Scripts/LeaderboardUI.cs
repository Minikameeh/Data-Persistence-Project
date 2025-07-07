using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LeaderboardUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerRowPrefab;
    public Transform contentParent;

    void Start()
    {
        var players = DataManager.Instance.playerList.players
            .Where(p => !string.IsNullOrEmpty(p.playerName))
            .OrderByDescending(p => p.bestScore);
        Debug.Log("Players encontrados: " + players.Count());

        foreach (var player in players)
        {
            Debug.Log("Creando fila para: " + player.playerName + " con score " + player.bestScore);

            var row = Instantiate(playerRowPrefab, contentParent);
            var texts = row.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 2)
            {
                texts[0].text = player.playerName;
                texts[1].text = player.bestScore.ToString();
            }
            else
            {
                Debug.LogWarning("El prefab no tiene suficientes TMP_Text.");
            }
        }
    }
}

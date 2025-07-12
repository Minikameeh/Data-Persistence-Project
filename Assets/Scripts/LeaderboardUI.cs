using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LeaderboardUI : MonoBehaviour
{
    public Transform contentParent; // El Content del ScrollView (padre de PlayerStats)

    void Start()
    {
        var players = DataManager.Instance.playerList.players;

        // Ordena los jugadores con nombre por bestScore descendente
        var sortedPlayers = System.Linq.Enumerable
            .Where(players, p => !string.IsNullOrEmpty(p.playerName))
            .OrderByDescending(p => p.bestScore)
            .ToList();

        // Recorremos las 10 filas existentes
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform row = contentParent.GetChild(i);

            TMP_Text positionText = row.GetChild(0).GetComponentInChildren<TMP_Text>();
            TMP_Text nameText = row.GetChild(1).GetComponentInChildren<TMP_Text>();
            TMP_Text scoreText = row.GetChild(2).GetComponentInChildren<TMP_Text>();

            if (i < sortedPlayers.Count)
            {
                var player = sortedPlayers[i];
                positionText.text = (i + 1).ToString();
                nameText.text = player.playerName;
                scoreText.text = player.bestScore.ToString();
            }
            else
            {
                // Si no hay tantos jugadores, dejamos las filas vacías
                positionText.text = "-";
                nameText.text = "VACÍO";
                scoreText.text = "-";
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BestPlayerText : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    void Start()
    {
        MostrarMejorJugador();
    }

    void MostrarMejorJugador()
    {
        if (DataManager.Instance == null || DataManager.Instance.playerList == null)
        {
            bestScoreText.text = "Best Score : --- : 0";
            return;
        }

        var players = DataManager.Instance.playerList.players;

        var bestPlayer = players
            .Where(p => !string.IsNullOrEmpty(p.playerName))
            .OrderByDescending(p => p.bestScore)
            .FirstOrDefault();

        if (bestPlayer != null)
        {
            bestScoreText.text = $"Best Score : {bestPlayer.playerName} : {bestPlayer.bestScore}";
        }
        else
        {
            bestScoreText.text = "Best Score : --- : 0";
        }
    }
}

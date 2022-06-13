using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WInnerPrefab : MonoBehaviour
{
    public TextMeshProUGUI winnerName;
    public TextMeshProUGUI winnerPoints;
    public TextMeshProUGUI winnerPosition;

    public void UpdateWinner(PlayerOffline player, int position)
    {
        winnerName.text = player.name;
        winnerPoints.text = player.totalPoints.ToString();
        winnerPosition.text = string.Format("{0}.", position.ToString());
    }
}

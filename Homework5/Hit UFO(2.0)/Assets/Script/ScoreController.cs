using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private Dictionary<Color, int> scoreTable = new Dictionary<Color, int>();
    private int score;

    private void Start()
    {
        score = 0;
        scoreTable.Add(Color.white, 1);
        scoreTable.Add(Color.yellow, 2);
        scoreTable.Add(Color.red, 3);
    }

    public void Count(GameObject UFO)
    {
        score += scoreTable[UFO.GetComponent<UFOObject>().GetUFOColor()];
    }

    public int GetScore()
    {
        return score;
    }

    public void Reset()
    {
        score = 0;
    }
}

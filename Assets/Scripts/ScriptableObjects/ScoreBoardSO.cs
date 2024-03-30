using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHighScoreList", menuName = "High Score List", order = 51)]
public class ScoreBoardSO : ScriptableObject
{
    public int highScore;
    public List<string> scores = new List<string>();
}
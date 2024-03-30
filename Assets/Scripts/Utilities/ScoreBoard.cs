using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject scoreSlot;

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string text in GameManager.Instance.scoreBoardSO.scores)
        {
            var x = Instantiate(scoreSlot, transform.position, transform.rotation, transform);
            x.transform.localScale = new Vector3 (1,1,1);
            x.GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}
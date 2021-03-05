using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLobbyController : MonoBehaviour
{
    public SceneController.Game game;
    public GameObject panelScore;
    public TextMeshProUGUI txtScore;
    

    // Start is called before the first frame update
    void Start()
    {
        string score = Global.Score;
        if(score == "" || score == null)
        {
            panelScore.SetActive(false);
        }
        else
        {
            txtScore.text = score;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

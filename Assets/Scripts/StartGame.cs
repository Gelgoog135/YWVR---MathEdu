using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void btnclick(int i)
    {
        new SceneController((SceneController.Game)i).ChangeToGameLobby();
    }
}

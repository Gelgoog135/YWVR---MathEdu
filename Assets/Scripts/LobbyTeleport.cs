using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTeleport:MonoBehaviour
{
    public Transform player;
   public void ToRoof()
    {
        player.position  = new Vector3(0,51,-25);
    }
    public void ToGround()
    {
        player.position = new Vector3(4,16,-25);
    }
}

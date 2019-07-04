using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;
    public float playerSpeed = 5;
    public float jumpForce = 20;
    
    void Start()
    {
        player = Instantiate(player);
        transform.position = new Vector2(0, 1);
    }
    void Update()
    {
        player.transform.Translate (Vector3.right * playerSpeed * Time.deltaTime); 
    }

    public void JumpUp()
    {
        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1);
    }
    
    public void JumpDown()
    {
        player.transform.position = new Vector2(player.transform.position.x, 1);
    }
}

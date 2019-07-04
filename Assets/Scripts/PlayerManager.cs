using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;
    public float paylerSpeed = 5;

    void Start()
    {
        player = Instantiate(player);
    }
    void Update()
    {
        player.transform.Translate (Vector3.right * paylerSpeed * Time.deltaTime); 
    }

    public void Jump()
    {
        Debug.Log("jump");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactable
{
    public enum obstacleType { none, jump, kill, trampoline };

    public obstacleType type = obstacleType.none;
}
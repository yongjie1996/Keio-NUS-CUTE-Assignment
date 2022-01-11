using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int level;
    public int lives;
    public int score;
    public float[] playerPosition;

    public SaveData(GameManager gameManager)
    {
        this.level = gameManager.level;
        this.lives = gameManager.lives;
        this.score = gameManager.score;

        this.playerPosition = new float[3];
        this.playerPosition[0] = gameManager.player.transform.position.x;
        this.playerPosition[1] = gameManager.player.transform.position.y;
        this.playerPosition[2] = gameManager.player.transform.position.z;
    }
}

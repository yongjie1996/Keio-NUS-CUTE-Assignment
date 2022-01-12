using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SaveData object containing variables that is saved from the game.
/// Saved variables includes game level, lives, score and player position.
/// </summary>
/// <see cref="SaveSystem"/> on how the variables are saved.

[System.Serializable]
public class SaveData
{
    public int level;
    public int lives;
    public int score;
    public float[] playerPosition;

    /// <summary>
    /// Constructor for SaveData Class.
    /// </summary>
    /// <param name="gameManager">Takes GameManager class for relevant variables to save as game progress.</param>
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

# Keio-NUS CUTE Assignment
This is an Asteroid game made by Lim Yong Jie as an assignment to be handed for job application for Keio-NUS CUTE.

The player of the game may either use WASD and mouse button or Arrow Keys and spacebar to move and shoot.

The player will start with 3 lives and at Level 1.

The game will have Level 1 to 10 and progresses up by one level for every 1000 score earned.

During the game, asteroids will spawn from outside the game boundaries and move into the general direction towards the player, where the game boundaries are.

Each Asteroid of a different size from big, medium, and small asteroids, giving different values of score of 25, 50 or 100 when destroyed by the player.
The bigger to medium asteroids will also split upon destruction if they are able to meet the minimum size of the asteroid (twice as big as the smallest asteroid possible).
If the asteroid hits the player, they will lose 1 live from their Lives count, and then respawn with 2 seconds of invulnerability.

Upon level progression, the player will be able to shoot at a faster rate, but the asteroids will also move faster.

The player will also be able to bring up a Pause menu by hitting the Esc button, which has 4 menu buttons of Resume, Save, Load and Exit.

The player will be able to Resume the game by either hitting the Esc button again while in the Pause menu or the Resume button.

The Save button will save the game progress and retain the Level, Score, Lives and player's in-game position and it will be saved into a file named "asteroid.save".
The Load button will load the above mentioned previously saved progress from the same file and load up the above variables into the game.

If the player hits the Quit button, the game will close.

Once the player scores 10000 points or above, the game will end and shows the player that they have won, and they will be able to play the game again by pressing the Replay button.

The player will lose the game once they lose all their lives and they will also be able to play the game again with the same button.

Upon hitting the replay button, the game will reset all progress and start the game again with the original lives count and at Level 1.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    LevelController levelController;
    const int maxLevels = 5;
    private int[] maxRooms = new int[maxLevels] { 4, 5, 6, 8, 10 };

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && LevelController.aliveEnemies == 0)
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(getNextSceneName(currentSceneName));
    }

    string getNextSceneName(string currentName)
    {
        if (currentName == "Entrance") return "Level1.1";

        var splitResult = currentName.Split('.');
        int currentLevel = int.Parse(splitResult[0].Last().ToString());
        int currentRoom = int.Parse(splitResult.Last());

        int nextLevel = 1;
        int nextRoom = (currentRoom + 1) % maxRooms[currentLevel];
        nextLevel = (nextRoom == 0) ? currentLevel + 1 : currentLevel;
        nextRoom = (nextRoom == 0) ? 1 : nextRoom;

        string nextSceneName = $"Level{nextLevel}.{nextRoom}";
        return nextSceneName;
    }
}

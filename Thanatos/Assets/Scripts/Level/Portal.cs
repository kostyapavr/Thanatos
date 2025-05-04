using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool isExit;
    const int maxLevels = 5;
    public Sprite openSprite;
    private int[] maxRooms = new int[maxLevels] { 4, 5, 6, 8, 10 };

    private bool isClosed = true;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Entrance") OpenPortal();
        else if (isExit) LevelController.enemyDeathEvent.AddListener(EnemyDeathEvent);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isExit && (LevelController.aliveEnemies == 0 || GameObject.FindGameObjectsWithTag("Enemy").Length <= 0))
        {
            OpenPortal();
        }

        if (collider.CompareTag("Player") && !isClosed)
        {
            if (SceneManager.GetActiveScene().name == "Final_Boss") ShowEndPanel(collider.GetComponent<Player>());
            else LoadNextLevel();
        }
    }

    public virtual void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Boss"))
        {
            if (LevelController.playerBoostHp > 0) LevelController.playerBoostHp = 0;
            if (LevelController.playerHasLavaBoots) LevelController.playerHasLavaBoots = false;
        }
        SceneManager.LoadScene(getNextSceneName(currentSceneName));
    }

    string getNextSceneName(string currentName)
    {
        if (currentName == "Entrance") return "Level1.1";
        if (currentName == "Level1.4") return "Level1_Boss";
        if (currentName == "Level1_Boss") return "Level2.1";
        if (currentName == "Level2.4") return "Level2_Boss";
        if (currentName == "Level2_Boss") return "Level3.1";
        if (currentName == "Level3.4") return "Level3_Boss";
        if (currentName == "Level3_Boss") return "Level4.1";
        if (currentName == "Level4.3") return "Level4_Boss";
        if (currentName == "Level4_Boss") return "Final_Boss";

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

    void EnemyDeathEvent()
    {
        Invoke("CheckEnemies", 0.1f);
    }

    void CheckEnemies()
    {
        if (LevelController.aliveEnemies == 0)
        {
            OpenPortal();
        }
    }

    public void OpenPortal()
    {
        isClosed = false;
        if (openSprite != null) GetComponent<SpriteRenderer>().sprite = openSprite;
    }

    void ShowEndPanel(Player p)
    {
        p.ShowEndPanel();
    }
}

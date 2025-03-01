using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyPortal : Portal
{
    public override void LoadNextLevel()
    {
        LevelController.isNormalDifficulty = false;
        base.LoadNextLevel();
    }
}

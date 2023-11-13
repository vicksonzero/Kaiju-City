using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EditorCools;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ArcadeObjective : MonoBehaviour
{
    public bool startGameOnCreate = false;
    public bool gameStarted = false;

    [Header("Kill enemies")]
    public string enemyCountChannel = "Enemy";

    public int totalEnemies = 30;
    public int killEnemies = 3;
    public int killedEnemies = 0;

    [Header("Time limit")]
    public float timeLimitInMinutes = 5f;

    public float gameStartTime = -1;
    public float gameOverTime = -1;
    public float gameTime = -1;

    [Header("Protect buildings")]
    public string buildingCountChannel = "Building";

    public int protectBuildings = 20;
    public int totalBuildings;

    public int leftBuildings;
    public int lostBuildings;

    [Header("Player")]
    public Player player;

    public float playerHealth;
    private Health _playerHealth;


    public TextMeshProUGUI objectiveLabel;

    [Tooltip(
        "Placeholders: \\n %killedEnemies% %killEnemies% %totalEnemies% %timeLeft% %leftBuildings% %lostBuildings% %protectBuildings%")]
    public string template =
        "Time left: %timeLeft%\\nEliminate enemies: %killedEnemies% / %totalEnemies%\\nDon't lose %protectBuildings% buildings (%lostBuildings% lost)";


    public GameOverScreen gameOverScreen;

    [CanBeNull]
    private Tween _objectiveLabelTimer;


    [Button()]
    public void CalculateRequiredKillCount()
    {
        totalEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerHealth = player.GetComponent<Health>();
        _playerHealth.HealthUpdated += (hp, hpMax) =>
        {
            playerHealth = hp;
            CheckLoseConditions();
        };
        totalBuildings = EntityCounter.Inst.GetCount(buildingCountChannel);
        EntityCounter.Inst.AddListener(buildingCountChannel,
            true,
            (count) =>
            {
                lostBuildings = totalBuildings - count;
                leftBuildings = count - (totalBuildings - protectBuildings);
                CheckLoseConditions();
            });

        EntityCounter.Inst.AddListener(enemyCountChannel,
            true,
            (count) =>
            {
                killedEnemies = totalEnemies - count;
                CheckWinConditions();
            });

        if (startGameOnCreate)
        {
            DOVirtual.DelayedCall(1, StartGame);
        }
    }

    public void CheckWinConditions()
    {
        if (!gameStarted) return;
        if (killedEnemies >= killEnemies)
        {
            WinGame($"killedEnemies '{killedEnemies}' >= killEnemies '{killEnemies}'");
        }
    }

    public void CheckLoseConditions()
    {
        if (!gameStarted) return;
        if (_playerHealth.ShouldDie())
        {
            GameOver("_playerHealth.ShouldDie");
        }

        if (lostBuildings >= protectBuildings)
        {
            GameOver("lostBuildings >= protectBuildings");
        }
    }

    [Button()]
    public void StartGame()
    {
        _objectiveLabelTimer = DOVirtual.DelayedCall(0.02f, UpdateObjectiveLabel).SetLoops(-1);
        gameStartTime = Time.time;
        gameOverTime = gameStartTime + timeLimitInMinutes * 60f;
        gameStarted = true;
    }

    public void StopGame()
    {
        _objectiveLabelTimer?.Kill();
        gameTime = Time.time - gameStartTime;
        gameStarted = false;
    }


    private void WinGame(string reason)
    {
        Debug.Log($"WinGame, reason = {reason}");
        _playerHealth.canDie = false;
        if (gameOverScreen) gameOverScreen.Show(true);
        StopGame();
    }

    private void GameOver(string reason)
    {
        Debug.Log($"GameOver, reason = {reason}");
        if (gameOverScreen) gameOverScreen.Show(false);
        StopGame();
    }

    // Update is called once per frame
    void UpdateObjectiveLabel()
    {
        var timeLeft = gameOverTime - Time.time;
        var timeLeftString =
            $"{Mathf.Floor(timeLeft / 60):00}:{Mathf.Floor(timeLeft % 60):00}.{(timeLeft - Math.Truncate(timeLeft)) * 1000:000}";
        objectiveLabel.text = template
                .Replace("%killedEnemies%", $"{killedEnemies}")
                .Replace("%killEnemies%", $"{killEnemies}")
                .Replace("%totalEnemies%", $"{totalEnemies}")
                .Replace("%timeLeft%", $"{timeLeftString}")
                .Replace("%leftBuildings%", $"{leftBuildings}")
                .Replace("%lostBuildings%", $"{lostBuildings}")
                .Replace("%protectBuildings%", $"{protectBuildings}")
                .Replace("\\n", "\n")
            ;
    }
}
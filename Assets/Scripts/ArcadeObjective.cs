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

    [Header("Evacuation")]
    public float evacuationTimeInMinutes = 3f;

    public float evacuationTimer = -1;
    public int evacuationProtectBuildings = 20;

    [Tooltip(
        "Placeholders: \\n %killedEnemies% %killEnemies% %totalEnemies% %timeLeft% %leftBuildings% %lostBuildings% %protectBuildings%")]
    public string evacuationTemplate =
        "Evacuation complete in: %timeLeft%\\nDon't lose %protectBuildings% buildings (%lostBuildings% lost)";

    [Header("Boss Spawning")]
    [Tooltip("In seconds")]
    public float bossSpawnAfter = 60;

    public float bossTvDuration = 10;

    [Tooltip("To save time linking up stuff, let's just keep the boss spawned but deactivated")]
    public Transform bossObject;

    public Transform bossSpawnPoint;
    public RectTransform bossHpBar;

    [Header("Fight Boss")]
    [Tooltip(
        "Placeholders: \\n %killedEnemies% %killEnemies% %totalEnemies% %timeLeft% %leftBuildings% %lostBuildings% %protectBuildings%")]
    public string fightBossTemplate = "Fight the spider alien";

    public GameOverScreen gameOverScreen;
    public KaijuTv tv;

    [Header("BGM")]
    public AudioSource introBgm;

    public AudioSource bossBgm;
    public float bossBgmIntroLength = 16f;
    public AudioSource giantBgm;


    [CanBeNull]
    private Tween _objectiveLabelTimer;

    private Tween _spawnBossTimer;


    [Button()]
    public void CalculateRequiredKillCount()
    {
        totalEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
    }

    private void Awake()
    {
        bossObject.gameObject.SetActive(false);
        bossHpBar.gameObject.SetActive(false);
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

        FindObjectOfType<Henshin>().HenshinChanged += isGiant =>
        {
            if (isGiant)
            {
                bossBgm.DOFade(0, 1);
                introBgm.DOFade(0, 1);
                if (giantBgm.isPlaying) giantBgm.Stop();
                giantBgm.Play();
                giantBgm.DOFade(1, 1);
            }
            else
            {
                bossBgm.DOFade(1, 1);
                giantBgm.DOFade(0, 1);
            }
        };

        if (startGameOnCreate)
        {
            DOVirtual.DelayedCall(1, StartGame);
        }
    }

    public void CheckWinConditions()
    {
        if (!gameStarted) return;
        // if (killedEnemies >= killEnemies)
        // {
        //     WinGame($"killedEnemies '{killedEnemies}' >= killEnemies '{killEnemies}'");
        // }
        if (!bossObject)
        {
            WinGame($"Boss killed");
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
                //
            });


        _objectiveLabelTimer = DOVirtual.DelayedCall(0.02f, UpdateObjectiveLabel).SetLoops(-1);
        _spawnBossTimer = DOVirtual.DelayedCall(bossSpawnAfter, ActivateBoss);
        gameStartTime = Time.time;
        // gameOverTime = gameStartTime + timeLimitInMinutes * 60f;
        evacuationTimer = gameStartTime + evacuationTimeInMinutes * 60f;

        _spawnBossTimer = DOVirtual.DelayedCall(bossSpawnAfter - bossBgmIntroLength, StartBossBgm);

        gameStarted = true;
    }

    private void StartBossBgm()
    {
        introBgm.DOFade(0, 3);
        if (!bossBgm.isPlaying)
        {
            bossBgm.Play();
            bossBgm.DOFade(1, 3).From(0);
            // bossBgm.transform.SetParent(bossObject, true);
            // bossBgm.transform.DOLocalMove(Vector3.zero, 5);
        }
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
        var timeLeft = evacuationTimer - Time.time;
        var timeLeftString =
            $"{Mathf.Floor(timeLeft / 60):00}:{Mathf.Floor(timeLeft % 60):00}.{(timeLeft - Math.Truncate(timeLeft)) * 1000:000}";
        objectiveLabel.text = evacuationTemplate
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

    [Button()]
    void CheatStartBoss()
    {
        StartBossBgm();
        ActivateBoss();
    }

    void ActivateBoss()
    {
        if (bossObject.gameObject.activeSelf) return;


        bossObject.gameObject.SetActive(true);
        bossHpBar.gameObject.SetActive(true);
        tv.ShowBoss(bossTvDuration);

        var jump = bossObject.GetComponent<SpiderJump>();
        jump.JumpTo(bossSpawnPoint.position, FindObjectOfType<Player>(false).transform);
    }

    public void OnBossDeathAnimationFinished()
    {
        CheckWinConditions();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EditorCools;
using JetBrains.Annotations;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ArcadeObjective : MonoBehaviour
{
    public bool startGameOnCreate = false;

    [FormerlySerializedAs("gameStarted")]
    public bool gameIsRunning = false;

    [Header("Kill enemies")]
    public bool finishedKillEnemies = false;

    public string enemyCountChannel = "Enemy";

    public int totalEnemies = 30;
    public int killEnemies = 3;
    public int killedEnemies = 0;

    public float killEnemiesTime;

    [Tooltip(PlaceHolderTooltip)]
    public string killedEnemiesTemplate =
        "You have defeated %killedEnemies% / %totalEnemies% Wasps in %killEnemiesTime%.";

    [Button()]
    public void CalculateRequiredKillCount()
    {
        totalEnemies = FindObjectsByType<WaspEnemy>(FindObjectsSortMode.None).Length;
    }

    [Header("Game Time")]
    public float gameStartTime = -1;

    public float gameOverTime = -1;
    public float gameTime = -1;

    // public float debugScaledTime;

    [Header("Protect buildings")]
    public bool finishedProtectBuildings = false;

    public string buildingCountChannel = "Building";

    public int protectBuildings = 20;
    public int totalBuildings;

    public int leftBuildings;
    public int lostBuildings;
    public int lostBuildingsEvac;


    [Button()]
    public void CalculateTotalBuildings()
    {
        totalBuildings = FindObjectsByType<Building>(FindObjectsSortMode.None).Length;
    }

    [Header("Player")]
    public Player player;

    [FormerlySerializedAs("playerHealth")]
    public float lastPlayerHealth;

    public float damageTaken;
    public float damageHealed;
    private Health _playerHealth;

    public TextMeshProUGUI objectiveLabel;

    [Header("Evacuation")]
    public bool finishedEvacuation = false;

    public float evacuationTimeInMinutes = 3f;

    public float evacuationTimer = -1;
    public int evacuationProtectBuildings = 20;

    [Tooltip(PlaceHolderTooltip)]
    public string evacuationTemplate =
        "Evacuation complete in: %timeLeft%\\nDon't lose %protectBuildings% buildings (%lostBuildings% lost)";


    [Header("Collect Repair Packs")]
    [Tooltip(PlaceHolderTooltip)]
    public int collectRepairPacks;

    public int collectedRepairPacks;
    public float collectRepairPacksTime;

    public bool finishedCollectRepairPacks;

    public string collectRepairPacksTemplate =
        "You have collected %collectedRepairPacks% repair packs in %collectRepairPacksTime%";

    [Button()]
    public void CalculateRequiredRepairPacks()
    {
        collectRepairPacks = FindObjectsByType<RepairPack>(FindObjectsSortMode.None).Length;
    }

    [Header("Boss Spawning")]
    [Tooltip("In seconds")]
    public float bossSpawnAfter = 60;

    public float bossTvDuration = 10;

    [Tooltip("To save time linking up stuff, let's just keep the boss spawned but deactivated")]
    public Transform bossObject;

    public Transform bossSpawnPoint;
    public RectTransform bossHpBar;

    [Header("Fight Boss")]
    [Tooltip(PlaceHolderTooltip)]
    public string fightBossTemplate = "Fight the spider alien";

    public int bossKilled = 0;
    public float bossKillTime;

    public GameOverScreen gameOverScreen;
    public KaijuTv tv;

    [Header("BGM")]
    public AudioSource introBgm;

    public AudioSource bossBgm;
    public float bossBgmIntroLength = 16f;
    public AudioSource giantBgm;

    [Header("Random Stat")]
    public float energyCollected;

    public int henshinCount;
    public float henshinTimeTotal;
    private float _henshinStartTime = -1;


    [CanBeNull]
    private Tween _objectiveLabelTimer;

    private Tween _spawnBossTimer;
    private NoticePanel _noticePanel;


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
            {
                var isFirstTime = lastPlayerHealth == 0;
                var diff = hp - lastPlayerHealth;
                lastPlayerHealth = hp;

                if(isFirstTime) return;
                if (diff < 0) damageTaken += -diff;
                if (diff > 0) damageHealed += diff;
            }
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

        player.GetComponent<ThirdPersonTankController>().canControlMovement = false;

        if (startGameOnCreate)
        {
            DOVirtual.DelayedCall(3f, StartGame, false);
        }


        _noticePanel = FindObjectOfType<NoticePanel>();
        _noticePanel.ShowMessage(NoticePanel.NoticeType.Ready, 2f);
    }

    private void Update()
    {
        // debugScaledTime = Time.time;
        if (gameIsRunning && !finishedEvacuation && Time.time >= evacuationTimer)
        {
            finishedProtectBuildings = true;
            finishedEvacuation = true;
            _noticePanel.ShowMessage(NoticePanel.NoticeType.EvacComplete);
            DOVirtual.DelayedCall(3, () =>
            {
                if (!gameIsRunning) return;
                _noticePanel.ShowMessage(NoticePanel.NoticeType.NewObjective);
            }, false);
        }
    }

    public void CheckWinConditions()
    {
        if (!gameIsRunning) return;
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
    }

    [Button()]
    public void StartGame()
    {
        FindObjectOfType<PersistStats>().ResetStats();
        player.GetComponent<ThirdPersonTankController>().canControlMovement = true;
        totalBuildings = EntityCounter.Inst.GetCount(buildingCountChannel);
        EntityCounter.Inst.AddListener(buildingCountChannel,
            true,
            (count) =>
            {
                if (!finishedEvacuation) lostBuildingsEvac = totalBuildings - count;
                lostBuildings = totalBuildings - count;
                leftBuildings = count - (totalBuildings - protectBuildings);

                if (!gameIsRunning) return;
                if (finishedEvacuation) return;
                if (lostBuildings >= protectBuildings)
                {
                    GameOver("We have lost too many buildings.");
                }
            });

        EntityCounter.Inst.AddListener(enemyCountChannel,
            true,
            (count) =>
            {
                killedEnemies = totalEnemies - count;

                if (!gameIsRunning) return;
                if (count <= 0)
                {
                    finishedKillEnemies = true;
                    killEnemiesTime = Time.time - gameStartTime;
                    _noticePanel.ShowMessage(NoticePanel.NoticeType.EnemyKilled, 3f,
                        ApplyTemplate(killedEnemiesTemplate));
                }
            });


        _objectiveLabelTimer = DOVirtual.DelayedCall(0.02f, UpdateObjectiveLabel, false).SetLoops(-1);
        _spawnBossTimer = DOVirtual.DelayedCall(bossSpawnAfter, ActivateBoss, false);
        gameStartTime = Time.time;
        // gameOverTime = gameStartTime + timeLimitInMinutes * 60f;
        evacuationTimer = gameStartTime + evacuationTimeInMinutes * 60f;

        _spawnBossTimer = DOVirtual.DelayedCall(bossSpawnAfter - bossBgmIntroLength, StartBossBgm, false);

        gameIsRunning = true;
    }

    private void StartBossBgm()
    {
        if (!gameIsRunning) return;
        introBgm.DOFade(0, 3);
        if (giantBgm.isPlaying)
            giantBgm.DOFade(0, 3);
        if (!bossBgm.isPlaying)
        {
            bossBgm.Play();
            bossBgm.DOFade(1, 3).From(0);
            // bossBgm.transform.SetParent(bossObject, true);
            // bossBgm.transform.DOLocalMove(Vector3.zero, 5);
        }


        _noticePanel.ShowMessage(NoticePanel.NoticeType.Warning, bossBgmIntroLength);
    }

    public void StopGame()
    {
        if (!gameIsRunning) return;
        _objectiveLabelTimer?.Kill();
        gameTime = Time.time - gameStartTime;
        OnHenshinEnd();
        FindObjectOfType<PersistStats>().SaveStats(this);
        gameIsRunning = false;
    }


    private void WinGame(string reason)
    {
        if (!gameIsRunning) return;
        Debug.Log($"WinGame, reason = {reason}");
        _playerHealth.canDie = false;
        // if (gameOverScreen) gameOverScreen.Show(true);
        _noticePanel.ShowMessage(NoticePanel.NoticeType.MissionAccomplished, 1000000);
        StopGame();
    }

    private void GameOver(string reason)
    {
        if (!gameIsRunning) return;
        Debug.Log($"GameOver, reason = {reason}");
        // if (gameOverScreen) gameOverScreen.Show(false);
        _noticePanel.ShowMessage(NoticePanel.NoticeType.MissionFailed, 1000000, reason);
        StopGame();
    }

    // Update is called once per frame
    void UpdateObjectiveLabel()
    {
        if (!finishedEvacuation)
        {
            objectiveLabel.text = ApplyTemplate(evacuationTemplate);
        }
        else
        {
            objectiveLabel.text = fightBossTemplate;
        }
    }


    private const string PlaceHolderTooltip =
        "Placeholders: \\n %killedEnemies% %killEnemies% %totalEnemies% %killEnemiesTime% %timeLeft% %leftBuildings% %lostBuildings% %protectBuildings%";

    private string ApplyTemplate(string str)
    {
        var timeLeft = evacuationTimer - Time.time;
        // Debug.Log($"ApplyTemplate {timeLeft}");
        return str
                .Replace("%killedEnemies%", $"{killedEnemies}")
                .Replace("%killEnemies%", $"{killEnemies}")
                .Replace("%killEnemiesTime%", $"{FormatTime(killEnemiesTime)}")
                .Replace("%totalEnemies%", $"{totalEnemies}")
                .Replace("%timeLeft%", $"{FormatTime(timeLeft)}")
                .Replace("%leftBuildings%", $"{leftBuildings}")
                .Replace("%lostBuildings%", $"{lostBuildings}")
                .Replace("%protectBuildings%", $"{protectBuildings}")
                .Replace("%collectRepairPacks%", $"{collectRepairPacks}")
                .Replace("%collectedRepairPacks%", $"{collectedRepairPacks}")
                .Replace("%collectRepairPacksTime%", $"{FormatTime(collectRepairPacksTime)}")
                .Replace("\\n", "\n")
            ;
    }

    private static string FormatTime(float timeLeft)
    {
        return
            $"{Mathf.Floor(timeLeft / 60):00}:{Mathf.Floor(timeLeft % 60):00}.{(timeLeft - Math.Truncate(timeLeft)) * 1000:000}";
    }

    [Button()]
    void CheatStartBoss()
    {
        StartBossBgm();
        ActivateBoss();
    }

    void ActivateBoss()
    {
        if (!gameIsRunning) return;
        if (bossObject.gameObject.activeSelf) return;


        bossObject.gameObject.SetActive(true);
        bossHpBar.gameObject.SetActive(true);
        tv.ShowBoss(bossTvDuration);

        var jump = bossObject.GetComponent<SpiderJump>();
        jump.JumpTo(bossSpawnPoint.position, FindObjectOfType<Player>(false).transform);
    }

    public void OnBossDeathAnimationFinished()
    {
        if (!gameIsRunning) return;
        bossKilled++;
        bossKillTime = (Time.time - gameStartTime) - bossSpawnAfter;
        CheckWinConditions();
    }

    public void OnPlayerDeathAnimationFinished()
    {
        if (!gameIsRunning) return;
        GameOver("You have exploded.");
    }

    public void OnRepairPackCollected()
    {
        if (!gameIsRunning) return;
        if (finishedCollectRepairPacks) return;
        collectedRepairPacks += 1;
        Debug.Log($"collectedRepairPacks: {collectedRepairPacks}");
        if (collectedRepairPacks == collectRepairPacks)
        {
            finishedCollectRepairPacks = true;
            collectRepairPacksTime = Time.time - gameStartTime;

            _noticePanel.ShowMessage(NoticePanel.NoticeType.RepairPacksCollected, 3f,
                ApplyTemplate(collectRepairPacksTemplate));
        }
    }

    public void OnEnergyCollected(float amount)
    {
        if (!gameIsRunning) return;
        energyCollected += amount;
    }

    public void OnHenshinStart()
    {
        if (!gameIsRunning) return;
        _henshinStartTime = Time.time;
        henshinCount++;
    }

    public void OnHenshinEnd()
    {
        if (_henshinStartTime < 0) return;
        henshinTimeTotal += Time.time - _henshinStartTime;

        _henshinStartTime = -1;
    }
}
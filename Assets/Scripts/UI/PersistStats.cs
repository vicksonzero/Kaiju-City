using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ResetStats()
    {
        PlayerPrefs.SetFloat("Stats.GameTime", -1);
        PlayerPrefs.SetFloat("Stats.DamageTaken", -1);
        PlayerPrefs.SetFloat("Stats.DamageHealed", -1);

        PlayerPrefs.SetInt("Stats.RepairPacksCollected", -1);
        PlayerPrefs.SetFloat("Stats.RepairPacksCollectTime", -1);

        PlayerPrefs.SetInt("Stats.WaspsKilled", -1);
        PlayerPrefs.SetFloat("Stats.WaspsKillTime", -1);

        PlayerPrefs.SetInt("Stats.BossKilled", -1);
        PlayerPrefs.SetFloat("Stats.BossKillTime", -1);

        PlayerPrefs.SetInt("Stats.BuildingsLostEvac", -1);
        PlayerPrefs.SetInt("Stats.BuildingsLostTotal", -1);

        PlayerPrefs.SetFloat("Stats.EnergyCollected", -1);
        PlayerPrefs.SetInt("Stats.HenshinCount", -1);
        PlayerPrefs.SetFloat("Stats.HenshinTimeTotal", -1);
    }

    public void SaveStats(ArcadeObjective objective)
    {
        PlayerPrefs.SetFloat("Stats.GameTime", objective.gameTime);
        PlayerPrefs.SetFloat("Stats.DamageTaken", objective.damageTaken);
        PlayerPrefs.SetFloat("Stats.DamageHealed", objective.damageHealed);

        PlayerPrefs.SetInt("Stats.RepairPacksCollected", objective.collectedRepairPacks);
        PlayerPrefs.SetFloat("Stats.RepairPacksCollectTime",
            objective.finishedCollectRepairPacks
                ? objective.collectRepairPacksTime
                : -1);

        PlayerPrefs.SetInt("Stats.WaspsKilled", objective.killedEnemies);
        PlayerPrefs.SetFloat("Stats.WaspsKillTime", objective.finishedKillEnemies
            ? objective.killEnemiesTime
            : -1);

        PlayerPrefs.SetInt("Stats.BossKilled", objective.bossKilled);
        PlayerPrefs.SetFloat("Stats.BossKillTime", objective.bossKilled > 0
            ? objective.bossKillTime
            : -1);

        PlayerPrefs.SetInt("Stats.BuildingsLostEvac", objective.lostBuildingsEvac);
        PlayerPrefs.SetInt("Stats.BuildingsLostTotal", objective.lostBuildings);

        PlayerPrefs.SetFloat("Stats.EnergyCollected", objective.energyCollected);
        PlayerPrefs.SetInt("Stats.HenshinCount", objective.henshinCount);
        PlayerPrefs.SetFloat("Stats.HenshinTimeTotal", objective.henshinCount > 0
            ? objective.henshinTimeTotal
            : -1);
    }
}
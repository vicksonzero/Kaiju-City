
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 晶片 idea:
// Stats: 攻擊力，射程、追尾性、彈速、連射、冷卻、屬性、特殊狀態、自動維修、省電省彈、自動發射。
// 佔位但係唔係直接 buff 武器本身都得？
// 移速、轉向速度、炮塔有效角度、增加鎖定數、減少操作相撞懲罰、自動救火

// 必殺：
// 全彈發射、宇宙射線、騎士踢、終極斬（一文字、十字）


// All upgrades can stack, but stacking doesn't always give more effects
public enum UpgradeType
{
    KineticDmg, // increase base damage
    PlasmaDmg, // 能量屬性 increase base damage
    BeamDmg, // 照射屬性 increase base damage
    ExplosiveDmg, // 爆破屬性 increase base damage
    FlameDmg, // 燃燒屬性 increase base damage
    AttackRange, // percentage increase
    Homing, // 
    BulletSpeed, // percentage increase
    Rapid, // seconds per bullet
    Cooldown, // seconds
    AmmoReuse, // percentage proc
    AutoShoot, // on / off
}
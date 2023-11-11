using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class DisplayListRepository : MonoBehaviour
{
    [CanBeNull]
    private static DisplayListRepository _inst;

    public static DisplayListRepository Inst => _inst ??= FindObjectOfType<DisplayListRepository>();

    [FormerlySerializedAs("bulletsDisplayList")]
    public Transform bulletDisplayList;
    public Transform enemyDisplayList;
    [FormerlySerializedAs("effectsDisplayList")]
    public Transform effectDisplayList;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADeathHandler : MonoBehaviour
{
    public abstract void Die(Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse);
}

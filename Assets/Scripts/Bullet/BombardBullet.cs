using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BombardBullet : MonoBehaviour
{
    public float height = 10f;

    public Vector3 targetPosition;

    public float bulletDuration = 3f;

    public LayerMask damageTheseLayers;
    public Transform explosionPrefab;
    public Transform effectDisplayList;
    public Bullet bulletPrefab;
    public Transform bulletDisplayList;
    public Transform bulletModel;
    public ParticleSystem ps;

    public Transform targetMarker;

    public float kineticDamage;
    public float kickImpulse = 0.3f;

    private Vector3 startingPosition;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void Play(Vector3 pos)
    {
        startingPosition = transform.position;
        targetPosition = pos;
        targetMarker.position = targetPosition;
        targetMarker.rotation = Quaternion.identity;

        var midPoint = (targetPosition + startingPosition) / 2;
        midPoint.y = startingPosition.y + height;

        var seq = DOTween.Sequence();
        seq.AppendCallback(() => bulletModel.LookAt(midPoint, Vector3.up));
        seq.Append(bulletModel.DOMoveX(midPoint.x, bulletDuration / 2f)
            .SetOptions(AxisConstraint.X)
            .SetEase(Ease.Linear));
        seq.Join(bulletModel.DOMoveY(midPoint.y, bulletDuration / 2f)
            .SetOptions(AxisConstraint.Y)
            .SetEase(Ease.OutQuad));
        seq.Join(bulletModel.DOMoveZ(midPoint.z, bulletDuration / 2f)
            .SetOptions(AxisConstraint.Z)
            .SetEase(Ease.Linear));
        seq.AppendCallback(() => bulletModel.LookAt(targetPosition, Vector3.up));
        seq.Append(bulletModel.DOMoveX(targetPosition.x, bulletDuration / 2f)
            .SetOptions(AxisConstraint.X)
            .SetEase(Ease.Linear));
        seq.Join(bulletModel.DOMoveY(targetPosition.y, bulletDuration / 2f)
            .SetOptions(AxisConstraint.Y)
            .SetEase(Ease.InQuad));
        seq.Join(bulletModel.DOMoveZ(targetPosition.z, bulletDuration / 2f)
            .SetOptions(AxisConstraint.Z)
            .SetEase(Ease.Linear));
        seq.AppendCallback(SpawnBullet);
        seq.AppendCallback(() => ps.Stop());
        seq.AppendCallback(() => ps.transform.SetParent(transform));
        seq.AppendCallback(() => Destroy(targetMarker.gameObject));
        seq.AppendCallback(() => Destroy(bulletModel.gameObject));

        seq.Play();
        ps.Play();
    }

    private void SpawnBullet()
    {
        var explosion = Instantiate(explosionPrefab,
            targetPosition, Quaternion.identity, effectDisplayList);

        var aoeBullet = Instantiate(bulletPrefab,
            targetPosition, Quaternion.identity, bulletDisplayList);
        aoeBullet.kineticDamage = kineticDamage;
        aoeBullet.damageTheseLayers = damageTheseLayers;
    }
}
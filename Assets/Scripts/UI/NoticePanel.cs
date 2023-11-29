using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EditorCools;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class NoticePanel : MonoBehaviour
{
    public enum NoticeType
    {
        Ready,
        EvacComplete,
        EnemyKilled,
        RepairPacksCollected,
        Warning,
        MissionAccomplished,
        MissionFailed,
        NewObjective
    }

    public float bgEnterTime = 0.3f;
    public int messageFlashTimes = 1;
    public float messageFlashInterval = 0.3f;

    public RectTransform bg;
    public RectTransform readyMessage;
    public RectTransform evacCompleteMessage;
    public RectTransform enemyKilledMessage;
    public TextMeshProUGUI enemyKilledSubtitle;
    public RectTransform repairPacksCollectedMessage;
    public TextMeshProUGUI repairPacksCollectedSubtitle;
    public RectTransform warningMessage;
    public RectTransform missionAccomplishedMessage;
    public RectTransform missionFailedMessage;
    public RectTransform newObjectiveMessage;

    [CanBeNull]
    private Tween _seq;


    // Start is called before the first frame update
    void Start()
    {
        HideAllMessages(true);
    }


    public void HideAllMessages(bool withBg = false)
    {
        if (withBg) bg.gameObject.SetActive(false);
        readyMessage.gameObject.SetActive(false);
        evacCompleteMessage.gameObject.SetActive(false);
        warningMessage.gameObject.SetActive(false);
        missionAccomplishedMessage.gameObject.SetActive(false);
        missionFailedMessage.gameObject.SetActive(false);
        newObjectiveMessage.gameObject.SetActive(false);

        enemyKilledMessage.gameObject.SetActive(false);
        repairPacksCollectedMessage.gameObject.SetActive(false);
    }

    public void ShowMessage(NoticeType type, float duration = 3f, string subtitle = "")
    {
        HideAllMessages();
        bg.gameObject.SetActive(true);
        var message = type switch
        {
            NoticeType.Ready => readyMessage,
            NoticeType.EvacComplete => evacCompleteMessage,
            NoticeType.EnemyKilled => enemyKilledMessage,
            NoticeType.RepairPacksCollected => repairPacksCollectedMessage,
            NoticeType.Warning => warningMessage,
            NoticeType.MissionAccomplished => missionAccomplishedMessage,
            NoticeType.MissionFailed => missionFailedMessage,
            NoticeType.NewObjective => newObjectiveMessage,
        };

        switch (type)
        {
            case NoticeType.EnemyKilled:
                enemyKilledSubtitle.text = subtitle;
                break;
            case NoticeType.RepairPacksCollected:
                repairPacksCollectedSubtitle.text = subtitle;
                break;
        }

        _seq?.Kill();

        var showHideSeq = DOTween.Sequence()
                .Append(DOVirtual.DelayedCall(messageFlashInterval, () => message.gameObject.SetActive(false)))
                .Append(DOVirtual.DelayedCall(messageFlashInterval, () => message.gameObject.SetActive(true)))
            ;
        _seq = DOTween.Sequence()
                .Join(bg.DOAnchorPos(Vector2.zero, bgEnterTime).From(Vector2.left * 1920))
                .AppendCallback(() => message.gameObject.SetActive(true))
                .Append(showHideSeq.SetLoops(messageFlashTimes))
                .Insert(0, DOVirtual.DelayedCall(duration, () => HideAllMessages(true)))
                .Play()
            ;
    }

    [Button()]
    public void Test()
    {
        ShowMessage(NoticeType.Ready);
    }
}
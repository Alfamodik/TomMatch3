using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GemsForRewarded : MonoBehaviour
{
    [SerializeField] private int _amount;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(ShowRewarded);
        YG2.onRewardAdv += AddGems;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ShowRewarded);
        YG2.onRewardAdv -= AddGems;
    }

    private void ShowRewarded()
    {
        YG2.RewardedAdvShow("AddGems");
    }

    private void AddGems(string obj)
    {
        InitScript.Instance.AddGems(_amount);
    }
}

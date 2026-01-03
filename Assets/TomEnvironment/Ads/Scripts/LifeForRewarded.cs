using UnityEngine;
using UnityEngine.UI;
using YG;

public class LifeForRewarded : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(ShowRewarded);
        YG2.onRewardAdv += AddLife;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ShowRewarded);
        YG2.onRewardAdv -= AddLife;
    }

    private void ShowRewarded()
    {
        YG2.RewardedAdvShow("AddLife");
    }

    private void AddLife(string obj)
    {
        InitScript.Instance.AddLife(1);
    }
}

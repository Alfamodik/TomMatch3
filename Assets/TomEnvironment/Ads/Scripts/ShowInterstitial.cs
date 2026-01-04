using UnityEngine;
using UnityEngine.UI;
using YG;

public class ShowInterstitial : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake() => _button?.onClick.AddListener(Show);

    private void OnDestroy() => _button?.onClick.RemoveListener(Show);

    public void Show()
    {
        if (YG2.isTimerAdvCompleted)
            YG2.InterstitialAdvShow();
    }
}

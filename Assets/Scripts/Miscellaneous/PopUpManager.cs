using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PopUpManager : MonoBehaviour
{
    [Header("Normal Pop-Up")]
    [SerializeField] private TemporaryObjects normalPopUp;
    [SerializeField] private TextMeshProUGUI normalPopUpMessage;

    [Header("Covers")]
    [SerializeField] private GameObject loadingCover;
    [SerializeField] private TextMeshProUGUI loadingCoverMessage;

    [Header("Persistent Objects")]
    [SerializeField] private string[] persistentObjectNames;    

    private string popUpReloadSceneName;

    public static PopUpManager Instance { get; private set; }

    public string insufficientMoneyMessage = "You don't have enough money to buy this item.";
    public string tooManyACtionsMessage = "Too many actions. Please slow down.";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    #region Normal Pop-Ups
    private void NormalPopUp(string message, float duration)
    {
        normalPopUp.gameObject.SetActive(false);

        normalPopUpMessage.text = message;
        normalPopUp.destroyAfter = duration;

        normalPopUp.gameObject.SetActive(true);
    }

    public void ShowNormalCustom(string message, float duration = 3)
    {
        NormalPopUp(message, duration);
    }

    public void ShowInsufficientMoney(float duration = 3)
    {
        NormalPopUp(insufficientMoneyMessage, duration);
    }

    public void ShowTooManyActions(float duration = 3)
    {
        normalPopUp(tooManyActionsMessage, duration);
    }
    #endregion

    #region Loading Cover
    public void ShowLoadingCover(bool toggle, string message = null)
    {
        loadingCover.SetActive(toggle);

        loadingCoverMessage.gameObject.SetActive(!string.IsNullOrEmpty(message));
        loadingCoverMessage.text = message;
    }
    #endregion
}
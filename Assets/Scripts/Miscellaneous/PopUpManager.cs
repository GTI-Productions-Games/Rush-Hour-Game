using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PopUpManager : MonoBehaviour
{
    [Header("Normal Pop-Up")]
    [SerializeField] private TemporaryObjects normalPopUp;
    [SerializeField] private TextMeshProUGUI normalPopUpMessage;

    [Header("Covers")]
    [SerializeField] private GameObject loadingCover;
    [SerializeField] private TextMeshProUGUI loadingCoverMessage;
    [SerializeField] private GameObject loadingCoverFull;
    [SerializeField] private TextMeshProUGUI loadingCoverFullMessage;

    [Header("Monologue")]
    [SerializeField] private GameObject monologueObject;
    [SerializeField] private TextMeshProUGUI monologueTextUI;
    [SerializeField] private GameObject pressSpaceToContinue;
    [SerializeField] private float typingSpeed = 0.1f;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPanel;

    public static PopUpManager Instance { get; private set; }

    public string insufficientMoneyMessage = "You don't have enough money to buy this item.";
    public string tooManyActionsMessage = "Too many actions. Please slow down.";

    private bool inMonologue = false;
    private bool proceedPressed;
    private bool isTyping;

    private void Awake()
    {
        Instance = this;
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
        NormalPopUp(tooManyActionsMessage, duration);
    }
    #endregion

    #region Loading Cover
    public void ShowLoadingCover(bool toggle, string message = null)
    {
        loadingCover.SetActive(toggle);

        loadingCoverMessage.gameObject.SetActive(!string.IsNullOrEmpty(message));
        loadingCoverMessage.text = message;
    }

    public void ShowLoadingCoverFull(bool toggle, string message = null)
    {
        loadingCoverFull.SetActive(toggle);

        loadingCoverFull.gameObject.SetActive(!string.IsNullOrEmpty(message));
        loadingCoverFullMessage.text = message;
    }    
    #endregion

    #region Monologue
    public void StartMonologue(string[] monologueLines)
    {
        if (inMonologue)
        {
            return;
        }

        GameManager.Instance.ToggleMovementOvverride(false);
        GameManager.Instance.ToggleGameTimer(false);

        inMonologue = true;
        StartCoroutine(MonologueSequence(monologueLines));
    }

    public IEnumerator StartMonologueCoroutine(string[] monologueLines)
    {
        if (!inMonologue)
        {
            GameManager.Instance.ToggleMovementOvverride(false);
            GameManager.Instance.ToggleGameTimer(false);

            inMonologue = true;
            yield return StartCoroutine(MonologueSequence(monologueLines));
        }
        else
        {
            yield return null;
        }        
    }

    private IEnumerator MonologueSequence(string[] monologueLines)
    {
        for (int i = 0; i < monologueLines.Length; i++)
        {
            yield return StartCoroutine(FlashMonologueOnScreen(monologueLines[i]));

            while (!proceedPressed)
            {
                yield return null;
            }

            proceedPressed = false;
        }

        ResetMonologue();
        inMonologue = false;

        GameManager.Instance.ToggleMovementOvverride(true);
        GameManager.Instance.ToggleGameTimer(true);
    }

    private IEnumerator FlashMonologueOnScreen(string text)
    {
        ResetMonologue();

        monologueObject.SetActive(true);
        monologueTextUI.text = "";

        isTyping = true;
        pressSpaceToContinue.SetActive(false);

        foreach (char letter in text)
        {
            if (proceedPressed)
            {
                monologueTextUI.text = text;
                break;
            }

            monologueTextUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        pressSpaceToContinue.SetActive(true);

        isTyping = false;
        proceedPressed = false;        
    }

    private void ResetMonologue()
    {
        monologueTextUI.text = null;
        monologueObject.SetActive(false);
        pressSpaceToContinue.SetActive(false);
    }

    public void ProceedMonologue()
    {
        proceedPressed = true;
    }
    #endregion

    #region Special
    public void ShowControls()
    {
        Time.timeScale = 0;

        tutorialPanel.SetActive(true);
        AudioManager.Instance.PlayClick();
    }

    public void HideControls()
    {
        Time.timeScale = 1;

        tutorialPanel.SetActive(false);
        AudioManager.Instance.PlayClick();
    }
    #endregion
}
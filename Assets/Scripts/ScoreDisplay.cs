using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text killText;
    [SerializeField] private TextMeshProUGUI popupText;

    [Header("Popup Settings")]
    [SerializeField] private float floatDistance = 50f; // local units or pixels depending on Canvas
    [SerializeField] private float duration = 0.7f;

    private int lastKillCount;
    private Coroutine popupRoutine;

    void Start()
    {
        lastKillCount = Checkpoint.TotalKills;

        if (popupText != null)
        {
            popupText.alpha = 0f;
            popupText.text = "";
        }
    }

    void Update()
    {
        int currentKills = Checkpoint.TotalKills;
        killText.text = currentKills.ToString();

        if (currentKills > lastKillCount)
        {
            ShowKillPopup("+" + (currentKills - lastKillCount));
            lastKillCount = currentKills;
        }
    }

    void ShowKillPopup(string text)
    {
        if (popupText == null) return;

        popupText.text = text;

        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine = StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        popupText.alpha = 1f;

        Vector3 startPos = popupText.transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            popupText.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            popupText.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        popupText.alpha = 0f;
        popupText.transform.localPosition = startPos;
    }
}
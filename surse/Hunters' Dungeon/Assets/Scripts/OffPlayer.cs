using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OffPlayer : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TextMeshProUGUI warning;
    public Image checkmark;
    [HideInInspector] public string playerName;
    [HideInInspector] public bool bot;
    bool firstClick = false;

    private void Start()
    {
        bot = false;
        checkmark.gameObject.SetActive(false);
        warning.gameObject.SetActive(false);
        nameInput.onValueChanged.AddListener(delegate { UpdateName(); });
    }

    private void Update()
    {
        if (playerName.Length < 4)
            warning.gameObject.SetActive(true);
        else
            warning.gameObject.SetActive(false);
    }

    public void BotClick()
    {
        if (bot)
        {
            checkmark.gameObject.SetActive(true);
        }
        else
            checkmark.gameObject.SetActive(true);
        if(!firstClick)
        {
            firstClick = true;
            return;
        }
        bot = !bot;
    }

    public void UpdateName()
    {
        playerName = nameInput.text;
    }

}

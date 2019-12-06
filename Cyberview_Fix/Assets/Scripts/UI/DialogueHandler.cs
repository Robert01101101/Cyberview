using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AvatarShown
{
    PROGAGONIST,
    HELPERBOT,
    MINEMAN,
    PROGAGONIST_Y,
    BOSS,
    UNKNOWNBOT
}


//This class is always present, as it is attached to the Player's HUD (_HUD -> Player HUD -> Dialogue Handler)

public class DialogueHandler : MonoBehaviour
{
    public Texture protagonistAvatar, protagonist_yAvatar, helperBotAvatar, mineManAvatar, bossAvatar, unknownBotAvatar;
    public GameObject panel;
    public TextMeshProUGUI text;
    public RawImage avatarRenderer;

    public void Start()
    {

    }

    public void showDialogue(AvatarShown avatarShown, string message)
    {
        panel.SetActive(true);
        text.text = message;
        switch (avatarShown)
        {
            case AvatarShown.PROGAGONIST:
                avatarRenderer.texture = protagonistAvatar;
                break;
            case AvatarShown.PROGAGONIST_Y:
                avatarRenderer.texture = protagonist_yAvatar;
                break;
            case AvatarShown.HELPERBOT:
                avatarRenderer.texture = helperBotAvatar;
                break;
            case AvatarShown.MINEMAN:
                avatarRenderer.texture = mineManAvatar;
                break;
            case AvatarShown.BOSS:
                avatarRenderer.texture = bossAvatar;
                break;
            case AvatarShown.UNKNOWNBOT:
                avatarRenderer.texture = unknownBotAvatar;
                break;
        }
    }

    public void hideDialogue()
    {
        panel.SetActive(false);
    }

    public string GetCurrentMessage()
    {
        return text.text;
    }
}

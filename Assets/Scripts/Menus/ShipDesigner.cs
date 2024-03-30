using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipDesigner : MonoBehaviour
{
    public enum ColorType
    {
        Red,
        Green,
        Blue
    }

    [Header("Sprites")]
    public List<Sprite> shipsGreen = new List<Sprite>();
    public List<Sprite> shipsRed = new List<Sprite>();
    public List<Sprite> shipsBlue = new List<Sprite>();
    public Sprite patternGreen;
    public Sprite patternRed;
    public Sprite patternBlue;

    [Header("Game Objects")]
    public List<GameObject> shipModel = new List<GameObject>();
    public GameObject menuButtons;
    public GameObject shipDesigner;
    public GameObject lockImage;

    [Header("Scriptable Objects")]
    public ShipLayoutSO shipLayout;

    [Header("Audio")]
    public AudioSource failSound;
    public AudioSource selectButton;

    [Header("Images")]
    public Image Preview;
    public Image patternPreview;

    [Header("Texts")]
    public TextMeshProUGUI _sidewinderInfo;
    public TextMeshProUGUI _goblinInfo;
    public TextMeshProUGUI _probeInfo;
    public TextMeshProUGUI _minerInfo;
    public TextMeshProUGUI _corvetteInfo;

    //internal
    bool goblinUnlock, probeUnlock, miningUnlock, corvetteUnlock;
    int currentShip = 0;
    ColorType currentColor = ColorType.Green;

    private void Start()
    {
        currentShip = 0;
        Sprite spriteVar = null;

        switch (shipLayout.RGB)
        {
            case 1:
                currentColor = ColorType.Red;
                patternPreview.sprite = patternRed;
                foreach (Sprite sprite in shipsRed)
                {
                    if (sprite == shipLayout.sprite)
                    {
                        spriteVar = sprite;
                        break;
                    }
                    else currentShip++;
                }
                break;
            case 2:
                currentColor = ColorType.Green;
                patternPreview.sprite = patternGreen;
                foreach (Sprite sprite in shipsGreen)
                {
                    if (sprite == shipLayout.sprite)
                    {
                        spriteVar = sprite;
                        break;
                    }
                    else currentShip++;
                }
                break;
            case 3:
                currentColor = ColorType.Blue;
                patternPreview.sprite = patternBlue;
                foreach (Sprite sprite in shipsBlue)
                {
                    if (sprite == shipLayout.sprite)
                    {
                        spriteVar = sprite;
                        break;
                    }
                    else currentShip++;
                }
                break;
            default:
                currentColor = ColorType.Green;
                patternPreview.sprite = patternGreen;
                foreach (Sprite sprite in shipsGreen)
                {
                    if (sprite == shipLayout.sprite)
                    {
                        spriteVar = sprite;
                        break;
                    }
                    else currentShip++;
                }
                break;
        }

        Preview.sprite = spriteVar;

        if (GameManager.Instance.scoreBoardSO.highScore > 10) goblinUnlock = true;
        if (GameManager.Instance.scoreBoardSO.highScore > 1000) probeUnlock = true;
        if (GameManager.Instance.scoreBoardSO.highScore > 10000) miningUnlock = true;
        if (GameManager.Instance.scoreBoardSO.highScore > 20000) corvetteUnlock = true;

        ChangeInfoText();
    }

    void ChangeInfoText()
    {
        _sidewinderInfo.enabled = false;
        _goblinInfo.enabled = false;
        _probeInfo.enabled = false;
        _minerInfo.enabled = false;
        _corvetteInfo.enabled = false;

        switch (currentShip)
        {
            case 0:
                _sidewinderInfo.enabled = true;
                break;
            case 1:
                _goblinInfo.enabled = true;
                break;
            case 2:
                _probeInfo.enabled = true;
                break;
            case 3:
                _minerInfo.enabled = true;
                break;
            case 4:
                _corvetteInfo.enabled = true;
                break;
            default:
                _sidewinderInfo.enabled = true;
                break;
        }
    }

    public void NextShip()
    {
        currentShip++;
        if (currentShip == shipsGreen.Count)
        {
            currentShip = 0;
        }

        if (currentShip == 0 || (currentShip == 1 && goblinUnlock) || 
            (currentShip == 2 && probeUnlock) || (currentShip == 3 && miningUnlock)
            || (currentShip == 4 && corvetteUnlock)) lockImage.SetActive(false);
        else lockImage.SetActive(true);

        if (currentColor == ColorType.Green)
        {
            Preview.sprite = shipsGreen[currentShip];
        }
        else if (currentColor == ColorType.Red)
        {
            Preview.sprite = shipsRed[currentShip];
        }
        else if (currentColor == ColorType.Blue)
        {
            Preview.sprite = shipsBlue[currentShip];
        }

        ChangeInfoText();
    }

    public void PreviousShip()
    {
        currentShip--;
        if (currentShip < 0)
        {
            currentShip = shipsGreen.Count-1;
        }

        if (currentShip == 0 || (currentShip == 1 && goblinUnlock) ||
            (currentShip == 2 && probeUnlock) || (currentShip == 3 && miningUnlock)
            || (currentShip == 4 && corvetteUnlock)) lockImage.SetActive(false);
        else lockImage.SetActive(true);

        if (currentColor == ColorType.Green)
        {
            Preview.sprite = shipsGreen[currentShip];
        }
        else if (currentColor == ColorType.Red)
        {
            Preview.sprite = shipsRed[currentShip];
        }
        else if (currentColor == ColorType.Blue)
        {
            Preview.sprite = shipsBlue[currentShip];
        }

        ChangeInfoText();
    }

    public void ChangeToGreen()
    {
        currentColor = ColorType.Green;
        patternPreview.sprite = patternGreen;
        Preview.sprite = shipsGreen[currentShip];
    }

    public void ChangeToRed()
    {
        currentColor = ColorType.Red;
        patternPreview.sprite = patternRed;
        Preview.sprite = shipsRed[currentShip];
    }

    public void ChangeToBlue()
    {
        currentColor = ColorType.Blue;
        patternPreview.sprite = patternBlue;
        Preview.sprite = shipsBlue[currentShip];
    }

    public void SaveAndReturn()
    {
        if (lockImage.activeSelf)
        {
            failSound.Play();
        }
        else
        {
            shipLayout.sprite = Preview.sprite;
            shipLayout.prefab = shipModel[currentShip];

            switch (currentColor)
            {
                case ColorType.Red:
                    shipLayout.RGB = 1;
                    break;
                case ColorType.Green:
                    shipLayout.RGB = 2;
                    break;
                case ColorType.Blue:
                    shipLayout.RGB = 3;
                    break;
                default:
                    shipLayout.RGB = 2;
                    break;
            }

            selectButton.Play();
            menuButtons.SetActive(true);
            shipDesigner.SetActive(false);
        }
    }
}
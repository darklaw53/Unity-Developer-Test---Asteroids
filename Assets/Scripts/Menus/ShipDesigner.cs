using System.Collections;
using System.Collections.Generic;
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

    public List<Sprite> shipsGreen = new List<Sprite>();
    public List<Sprite> shipsRed = new List<Sprite>();
    public List<Sprite> shipsBlue = new List<Sprite>();
    public Image Preview;
    public GameObject lockImage;
    public AudioSource failSound, selectButton;

    public List<GameObject> shipModel = new List<GameObject>();

    public Sprite patternGreen, patternRed, patternBlue;
    public Image patternPreview;

    public ShipLayoutSO shipLayout;

    public GameObject menuButtons, shipDesigner;

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
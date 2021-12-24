using UnityEngine;
using UnityEngine.UI;
using JankenDefine;

public class JankenIconView : MonoBehaviour
{

    [SerializeField]
    private Image imageIcon = default;

    [SerializeField]
    private Image imageText = default;

    [SerializeField]
    private Sprite[] spriteIcons = new Sprite[3];

    [SerializeField]
    private Sprite[] spriteTexts = new Sprite[3];

    private void Start()
    {
        SetImages(JankenHand.PA);
    }

    public void SetImages(JankenHand jankenHand)
    {
        imageIcon.sprite = spriteIcons[(int)jankenHand];
        imageText.sprite = spriteTexts[(int)jankenHand];
    }

}

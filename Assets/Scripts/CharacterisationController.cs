using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterisationController : MonoBehaviour
{
    public GameObject Base;
    public GameObject Shirt;
    public GameObject Shoes;
    public GameObject Hair;
    public GameObject Pants;
    private SpriteRenderer BaseSpriteRenderer;
    private SpriteRenderer ShirtSpriteRenderer;
    private SpriteRenderer ShoesSpriteRenderer;
    private SpriteRenderer HairSpriteRenderer;
    private SpriteRenderer PantsSpriteRenderer;
    private Animator HairAnimator;
    private Animator PantsAnimator;

    public TMP_InputField NameInputField;

    public TextMeshProUGUI NoNameErrorText;

    private readonly List<Color> _skinColorList = new List<Color>();
    private readonly List<Color> _shirtPantsShoesColorList = new List<Color>();
    private readonly List<Color> _hairColorList = new List<Color>();

    private readonly List<string> SkinColorHexes = new List<string>
	{
        "#492100",
        "#7F3900",
        "#B27747",
        "#FFC599",
        "#995E00",
        "#FFC365",
        "#FFEBCC",
        "#FFDACC"
    };
    private readonly List<string> HairColorHexes = new List<string> 
    {
		"#FFE2B2",
        "#FFB232",
        "#99421E",
        "#331609",
        "#000000",
        "#FF6565",
        "#1B7F00",
        "#3558B2",
        "#7F145A"
	};
    private readonly List<string> ShirtPantsShoesColorHexes = new List<string>
    {
        "#FFFFFF",
        "#B20000",
        "#FFAA00",
        "#6CCC00",
        "#00B279",
        "#5B82E5",
        "#5B087F",
        "#FFB2B2"
    };

    private int _skinColorIndex = 0;
    private int _shirtColorIndex = 0;
    private int _shoesColorIndex = 0;
    private int _hairColorIndex = 0;
    private int _hairStyleIndex = 0;
    private int _pantsColorIndex = 0;
    private int _pantsStyleIndex = 0;

    public List<AnimatorController> HairAnimatorController = new List<AnimatorController>();
    public List<AnimatorController> PantsAnimatorController = new List<AnimatorController>();

	public void Start()
	{
        BaseSpriteRenderer = Base.GetComponent<SpriteRenderer>();
		ShirtSpriteRenderer = Shirt.GetComponent<SpriteRenderer>();
        ShoesSpriteRenderer = Shoes.GetComponent<SpriteRenderer>();
		HairSpriteRenderer = Hair.GetComponent<SpriteRenderer>();
		PantsSpriteRenderer = Pants.GetComponent<SpriteRenderer>();
		HairAnimator = Hair.GetComponent<Animator>();
		PantsAnimator = Pants.GetComponent<Animator>();

		CalculateColorList();
	}

	private void CalculateColorList()
    {
		SkinColorHexes.ForEach(color =>
		{
			ColorUtility.TryParseHtmlString(color, out var realColor);
			_skinColorList.Add(realColor);
		});
		HairColorHexes.ForEach(color =>
		{
			ColorUtility.TryParseHtmlString(color, out var realColor);
			_hairColorList.Add(realColor);
		});
		ShirtPantsShoesColorHexes.ForEach(color =>
		{
			ColorUtility.TryParseHtmlString(color, out var realColor);
			_shirtPantsShoesColorList.Add(realColor);
		});
	}

	public void ChangeSkin(bool next)
    {
        _skinColorIndex += next ? 1 : -1;
        if (_skinColorIndex < 0) _skinColorIndex = _skinColorList.Count - 1;
        if (_skinColorIndex >= _skinColorList.Count) _skinColorIndex = 0;

		BaseSpriteRenderer.color = _skinColorList[_skinColorIndex];
    }

    public void ChangeHair(bool next)
    {
		_hairColorIndex += next ? 1 : -1;
		if (_hairColorIndex < 0) _hairColorIndex = _hairColorList.Count - 1;
		if (_hairColorIndex >= _hairColorList.Count) _hairColorIndex = 0;

		HairSpriteRenderer.color = _hairColorList[_hairColorIndex];
	}

    public void ChangeHairStyle(bool next)
    {
		_hairStyleIndex += next ? 1 : -1;
		if (_hairStyleIndex < 0) _hairStyleIndex = HairAnimatorController.Count - 1;
		if (_hairStyleIndex >= HairAnimatorController.Count) _hairStyleIndex = 0;

		HairAnimator.runtimeAnimatorController = HairAnimatorController[_hairStyleIndex];
	}

    public void ChangeShirt(bool next)
    {
		_shirtColorIndex += next ? 1 : -1;
		if (_shirtColorIndex < 0) _shirtColorIndex = _shirtPantsShoesColorList.Count - 1;
		if (_shirtColorIndex >= _shirtPantsShoesColorList.Count) _shirtColorIndex = 0;

		ShirtSpriteRenderer.color = _shirtPantsShoesColorList[_shirtColorIndex];
	}

    public void ChangePants(bool next)
    {
		_pantsColorIndex += next ? 1 : -1;
		if (_pantsColorIndex < 0) _pantsColorIndex = _shirtPantsShoesColorList.Count - 1;
		if (_pantsColorIndex >= _shirtPantsShoesColorList.Count) _pantsColorIndex = 0;

		PantsSpriteRenderer.color = _shirtPantsShoesColorList[_pantsColorIndex];
	}

    public void ChangePantsStyle(bool next)
    {
		_pantsStyleIndex += next ? 1 : -1;
		if (_pantsStyleIndex < 0) _pantsStyleIndex = PantsAnimatorController.Count - 1;
		if (_pantsStyleIndex >= PantsAnimatorController.Count) _pantsStyleIndex = 0;

		PantsAnimator.runtimeAnimatorController = PantsAnimatorController[_pantsStyleIndex];
	}

    public void ChangeShoes(bool next)
    {
		_shoesColorIndex += next ? 1 : -1;
		if (_shoesColorIndex < 0) _shoesColorIndex = _shirtPantsShoesColorList.Count - 1;
		if (_shoesColorIndex >= _shirtPantsShoesColorList.Count) _shoesColorIndex = 0;

		ShoesSpriteRenderer.color = _shirtPantsShoesColorList[_shoesColorIndex];
	}

    public void Randomize()
    {
		_skinColorIndex = Random.Range(0, _skinColorList.Count);
		BaseSpriteRenderer.color = _skinColorList[_skinColorIndex];

		_shirtColorIndex = Random.Range(0, _shirtPantsShoesColorList.Count);
		ShirtSpriteRenderer.color = _shirtPantsShoesColorList[_shirtColorIndex];

		_shoesColorIndex = Random.Range(0, _shirtPantsShoesColorList.Count);
		ShoesSpriteRenderer.color = _shirtPantsShoesColorList[_shoesColorIndex];

		_hairColorIndex = Random.Range(0, _hairColorList.Count);
		HairSpriteRenderer.color = _hairColorList[_hairColorIndex];

		_hairStyleIndex = Random.Range(0, HairAnimatorController.Count);
		HairAnimator.runtimeAnimatorController = HairAnimatorController[_hairStyleIndex];

		_pantsColorIndex = Random.Range(0, _shirtPantsShoesColorList.Count);
		PantsSpriteRenderer.color = _shirtPantsShoesColorList[_pantsColorIndex];

		_pantsStyleIndex = Random.Range(0, PantsAnimatorController.Count);
		PantsAnimator.runtimeAnimatorController = PantsAnimatorController[_pantsStyleIndex];
	}

    IEnumerator DisableNoNameError()
    {
        yield return new WaitForSecondsRealtime(2);
        NoNameErrorText.gameObject.SetActive(false);
    }

	public void StartGame() 
    {
        if (string.IsNullOrWhiteSpace(NameInputField.text))
        {
            NoNameErrorText.gameObject.SetActive(true);
            StartCoroutine(DisableNoNameError());
            return;
        }

        MainController.Instance.SavePlayerData(BaseSpriteRenderer.color, ShirtSpriteRenderer.color, ShoesSpriteRenderer.color,
            HairSpriteRenderer.color, PantsSpriteRenderer.color, HairAnimator.runtimeAnimatorController, 
            PantsAnimator.runtimeAnimatorController, NameInputField.text);

        SceneManager.LoadScene("SampleScene");
    }
}

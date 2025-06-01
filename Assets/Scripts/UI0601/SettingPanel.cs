using System;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
     public SpriteView spriteView1;
    public SpriteView spriteView2;
    public SpriteView spriteView3;


    // public TextMeshProUGUI ChImfomation1;

    public Text infoText;
    // public TextMeshProUGUI ChImfomation2;
    // public TextMeshProUGUI ChImfomation3;

    public PlayerController playerController;

    [Header("UI Elements")]
    [Header("Level")]
    public Button closeBtn;

    public string choosenLevelName;
    public Button easyBtn;
    public Button mediumBtn;
    public Button endlessBtn;
    public Color chosenColor;
  
    private Button mageBtn;
    private Button warlockBtn;
    private Button battlemageBtn;
    
  
    
    
    
    private void Awake()
    {
        
        mageBtn = spriteView1.GetComponent<Button>();
        warlockBtn = spriteView2.GetComponent<Button>();
        battlemageBtn = spriteView3.GetComponent<Button>();
        
        closeBtn.onClick.AddListener(OnCloseClick);
        easyBtn.onClick.AddListener(OnEasyClick);
        mediumBtn.onClick.AddListener(OnMediumClick);
        endlessBtn.onClick.AddListener(OnEndlessClick);
        
        mageBtn.onClick.AddListener(OnMageClick);
        warlockBtn.onClick.AddListener(OnWarlockClick);
        battlemageBtn.onClick.AddListener(OnBattlemageClick);
        
        EventCenter.AddListener(EventDefine.ShowSettingPanel,ShowSettingPanel);
    }

  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
       gameObject.SetActive(false);
    }

    void ShowSettingPanel()
    {
        gameObject.SetActive(true);
        
        Apply(spriteView1.gameObject,"mage", GameManager.Instance.playerSpriteManager.Get(0));
        Apply(spriteView2.gameObject,"warlock", GameManager.Instance.playerSpriteManager.Get(1));
        Apply(spriteView3.gameObject,"battlemage", GameManager.Instance.playerSpriteManager.Get(2));
    }
    public void Apply(GameObject characterObj,string label, Sprite sprite)
    {
        characterObj.transform.Find("sprite").GetComponent<Image>().sprite = sprite;
        characterObj.transform.Find("index").GetComponent<Text>().text = label;
        
      
    }
    
    private void OnCloseClick()
    {
        EventCenter.Broadcast(EventDefine.ChoosenLevelName, choosenLevelName);
        gameObject.SetActive(false);
    }
    
    private void OnEndlessClick()
    {
        ReLevelColor();
        endlessBtn.GetComponent<Image>().color = chosenColor;
        choosenLevelName = "Endless";
    }

    private void OnMediumClick()
    {
        ReLevelColor();
        mediumBtn.GetComponent<Image>().color = chosenColor;
        choosenLevelName = "Medium";
    }

    private void OnEasyClick()
    {
        ReLevelColor();
        easyBtn.GetComponent<Image>().color = chosenColor;
        choosenLevelName = "Easy";
    }

    void ReLevelColor()
    {
        easyBtn.GetComponent<Image>().color = Color.white;
        mediumBtn.GetComponent<Image>().color = Color.white;
        endlessBtn.GetComponent<Image>().color = Color.white;
    }
    
    private void OnBattlemageClick()
    {
        ReCharacterColor();
        DisplayInfo("battlemage");
        battlemageBtn.GetComponent<Image>().color = chosenColor;
    }

    private void OnWarlockClick()
    {
        ReCharacterColor();
        DisplayInfo("warlock");
        warlockBtn.GetComponent<Image>().color = chosenColor;
    }

    private void OnMageClick()
    {
        ReCharacterColor();
        DisplayInfo("mage");
        mageBtn.GetComponent<Image>().color = chosenColor;
    }

    
    void ReCharacterColor()
    {
        mageBtn.GetComponent<Image>().color = Color.white;
        warlockBtn.GetComponent<Image>().color = Color.white;
        battlemageBtn.GetComponent<Image>().color = Color.white;
    }

    public void Chosen(int index)
    {
        
        GameManager.Instance.playerSpriteManager.currentIconIndex = index;
        playerController.loadCharacter(index);
    }
    public void DisplayInfo(string chosenLevelName)
    {
        TextAsset jsonText = Resources.Load<TextAsset>("classes");
        if (jsonText == null)
        {
            Debug.LogError("classes.json not found in Resources!");
            return;
        }

        var root = Newtonsoft.Json.Linq.JObject.Parse(jsonText.text);

        // Get current wave and default spell power context
        int wave = GameManager.Instance.currentWave;

        string Format(JObject data)
        {
            string health = data["health"]?.ToString();
            string mana = data["mana"]?.ToString();
            string manaReg = data["mana_regeneration"]?.ToString();
            string spellPower = data["spellpower"]?.ToString();
            string speed = data["speed"]?.ToString();

            string result = "";
            result += $"Health: {RPNCalculator.EvaluateFloat(health, wave)}\n";
            result += $"Mana: {RPNCalculator.EvaluateFloat(mana, wave)}\n";
            result += $"Mana Regen: {RPNCalculator.EvaluateFloat(manaReg, wave)}\n";
            result += $"Spell Power: {RPNCalculator.EvaluateFloat(spellPower, wave)}\n";
            result += $"Speed: {RPNCalculator.EvaluateFloat(speed, wave)}\n";
            return result;
        }

        // ChImfomation1.text = Format((JObject)root[chosenLevelName]);
        infoText.text =  Format((JObject)root[chosenLevelName]);
        // ChImfomation2.text = Format((JObject)root["warlock"]);
        // ChImfomation3.text = Format((JObject)root["battlemage"]);
        
        
    }
    
  

    private void OnDestroy()
    {
        
        EventCenter.RemoveListener(EventDefine.ShowSettingPanel,ShowSettingPanel);
    }
}

using SpiritBond.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpiritBond.Battle
{
    public static class BattleSceneAutoSetup
    {
        private const string BattleSceneName = "Battle";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!string.Equals(scene.name, BattleSceneName, System.StringComparison.Ordinal))
            {
                return;
            }

            if (Object.FindFirstObjectByType<BattleSystem>() != null)
            {
                return;
            }

            BuildBattleSceneRuntime();
        }

        private static void BuildBattleSceneRuntime()
        {
            Font defaultFont = CreateRuntimeFont();

            GameObject root = new GameObject("BattleRuntime");
            BattleSystem battleSystem = root.AddComponent<BattleSystem>();

            Canvas canvas = CreateCanvas(defaultFont, out CanvasScaler _, out GraphicRaycaster _);
            canvas.transform.SetParent(root.transform, false);

            Image playerImage = CreateUnitImage("PlayerUnit", canvas.transform, new Vector2(220f, 120f), new Vector2(220f, 220f));
            Image enemyImage = CreateUnitImage("EnemyUnit", canvas.transform, new Vector2(220f, 120f), new Vector2(-220f, 220f));

            BattleUnit playerUnit = playerImage.gameObject.AddComponent<BattleUnit>();
            BattleUnit enemyUnit = enemyImage.gameObject.AddComponent<BattleUnit>();

            BattleHud playerHud = CreateHud("PlayerHud", canvas.transform, defaultFont, new Vector2(270f, 100f), new Vector2(-260f, -180f));
            BattleHud enemyHud = CreateHud("EnemyHud", canvas.transform, defaultFont, new Vector2(270f, 100f), new Vector2(260f, 120f));
            BattleDialogBox dialogBox = CreateDialogBox(canvas.transform, defaultFont);
            CreateSkillButtons(canvas.transform, defaultFont, battleSystem);

            AssignReference(battleSystem, "playerUnit", playerUnit);
            AssignReference(battleSystem, "enemyUnit", enemyUnit);
            AssignReference(battleSystem, "playerHud", playerHud);
            AssignReference(battleSystem, "enemyHud", enemyHud);
            AssignReference(battleSystem, "dialogBox", dialogBox);

            Debug.Log("[BattleSceneAutoSetup] Built fallback battle scene UI/runtime because Battle scene had no BattleSystem.");
        }

        private static Canvas CreateCanvas(Font defaultFont, out CanvasScaler scaler, out GraphicRaycaster raycaster)
        {
            GameObject canvasObject = new GameObject("BattleCanvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);

            raycaster = canvasObject.AddComponent<GraphicRaycaster>();

            Image background = canvasObject.AddComponent<Image>();
            background.color = new Color(0.19f, 0.31f, 0.47f, 1f);

            return canvas;
        }

        private static Image CreateUnitImage(string objectName, Transform parent, Vector2 size, Vector2 anchoredPosition)
        {
            GameObject imageObject = new GameObject(objectName);
            imageObject.transform.SetParent(parent, false);

            RectTransform rectTransform = imageObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = anchoredPosition;

            Image image = imageObject.AddComponent<Image>();
            image.preserveAspect = true;
            return image;
        }

        private static BattleHud CreateHud(string objectName, Transform parent, Font defaultFont, Vector2 size, Vector2 anchoredPosition)
        {
            GameObject hudObject = new GameObject(objectName);
            hudObject.transform.SetParent(parent, false);

            RectTransform hudRect = hudObject.AddComponent<RectTransform>();
            hudRect.sizeDelta = size;
            hudRect.anchoredPosition = anchoredPosition;

            BattleHud hud = hudObject.AddComponent<BattleHud>();

            Text nameText = CreateText("NameText", hudObject.transform, defaultFont, new Vector2(0f, 28f), 22);
            Text levelText = CreateText("LevelText", hudObject.transform, defaultFont, new Vector2(0f, 2f), 18);
            HPBar hpBar = CreateHpBar(hudObject.transform, new Vector2(0f, -28f));

            AssignReference(hud, "nameText", nameText);
            AssignReference(hud, "levelText", levelText);
            AssignReference(hud, "hpBar", hpBar);

            return hud;
        }

        private static BattleDialogBox CreateDialogBox(Transform parent, Font defaultFont)
        {
            GameObject dialogObject = new GameObject("DialogBox");
            dialogObject.transform.SetParent(parent, false);

            RectTransform rectTransform = dialogObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.sizeDelta = new Vector2(760f, 140f);
            rectTransform.anchoredPosition = new Vector2(0f, 22f);

            Image background = dialogObject.AddComponent<Image>();
            background.color = new Color(0f, 0f, 0f, 0.7f);

            BattleDialogBox dialogBox = dialogObject.AddComponent<BattleDialogBox>();
            Text dialogText = CreateText("DialogText", dialogObject.transform, defaultFont, Vector2.zero, 24);

            RectTransform textRect = dialogText.rectTransform;
            textRect.anchorMin = new Vector2(0f, 0f);
            textRect.anchorMax = new Vector2(1f, 1f);
            textRect.offsetMin = new Vector2(20f, 20f);
            textRect.offsetMax = new Vector2(-20f, -20f);
            dialogText.alignment = TextAnchor.MiddleCenter;

            AssignReference(dialogBox, "dialogText", dialogText);
            return dialogBox;
        }

        private static void CreateSkillButtons(Transform parent, Font defaultFont, BattleSystem battleSystem)
        {
            for (int i = 0; i < 4; i++)
            {
                int skillIndex = i;
                GameObject buttonObject = new GameObject($"SkillButton_{i + 1}");
                buttonObject.transform.SetParent(parent, false);

                RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(160f, 50f);
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                rectTransform.pivot = new Vector2(0.5f, 0f);
                rectTransform.anchoredPosition = new Vector2(-255f + (170f * i), 170f);

                Image image = buttonObject.AddComponent<Image>();
                image.color = new Color(0.95f, 0.95f, 0.95f, 0.9f);

                Button button = buttonObject.AddComponent<Button>();
                button.onClick.AddListener(() => battleSystem.PlayerUseSkill(skillIndex));

                Text text = CreateText("Text", buttonObject.transform, defaultFont, Vector2.zero, 18);
                text.text = $"Skill {i + 1}";
                text.alignment = TextAnchor.MiddleCenter;

                RectTransform textRect = text.rectTransform;
                textRect.anchorMin = new Vector2(0f, 0f);
                textRect.anchorMax = new Vector2(1f, 1f);
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;
            }
        }

        private static HPBar CreateHpBar(Transform parent, Vector2 anchoredPosition)
        {
            GameObject sliderObject = new GameObject("HPBar");
            sliderObject.transform.SetParent(parent, false);

            RectTransform rectTransform = sliderObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200f, 20f);
            rectTransform.anchoredPosition = anchoredPosition;

            Slider slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;

            GameObject backgroundObject = new GameObject("Background");
            backgroundObject.transform.SetParent(sliderObject.transform, false);
            Image background = backgroundObject.AddComponent<Image>();
            background.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            RectTransform backgroundRect = background.rectTransform;
            backgroundRect.anchorMin = new Vector2(0f, 0f);
            backgroundRect.anchorMax = new Vector2(1f, 1f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            GameObject fillAreaObject = new GameObject("Fill Area");
            fillAreaObject.transform.SetParent(sliderObject.transform, false);
            RectTransform fillAreaRect = fillAreaObject.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0f, 0f);
            fillAreaRect.anchorMax = new Vector2(1f, 1f);
            fillAreaRect.offsetMin = new Vector2(5f, 5f);
            fillAreaRect.offsetMax = new Vector2(-5f, -5f);

            GameObject fillObject = new GameObject("Fill");
            fillObject.transform.SetParent(fillAreaObject.transform, false);
            Image fillImage = fillObject.AddComponent<Image>();
            fillImage.color = new Color(0.23f, 0.8f, 0.32f, 1f);

            RectTransform fillRect = fillImage.rectTransform;
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            slider.fillRect = fillRect;
            slider.targetGraphic = fillImage;
            slider.direction = Slider.Direction.LeftToRight;

            HPBar hpBar = sliderObject.AddComponent<HPBar>();
            AssignReference(hpBar, "slider", slider);
            return hpBar;
        }

        private static Font CreateRuntimeFont()
        {
            Font font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            if (font != null)
            {
                return font;
            }

            font = Font.CreateDynamicFontFromOSFont("Tahoma", 24);
            if (font != null)
            {
                return font;
            }

            return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }

        private static Text CreateText(string objectName, Transform parent, Font font, Vector2 anchoredPosition, int fontSize)
        {
            GameObject textObject = new GameObject(objectName);
            textObject.transform.SetParent(parent, false);

            RectTransform rectTransform = textObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(240f, 30f);
            rectTransform.anchoredPosition = anchoredPosition;

            Text text = textObject.AddComponent<Text>();
            text.font = font;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = objectName;
            return text;
        }

        private static void AssignReference(Object target, string fieldName, Object value)
        {
            System.Reflection.FieldInfo field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field?.SetValue(target, value);
        }
    }
}

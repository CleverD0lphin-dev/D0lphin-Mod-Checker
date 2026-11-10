using System;
using UnityEngine;
using UnityEngine.UI;

namespace D0lphin_Mod_Checker
{
    public class CustomModMenu : MonoBehaviour
    {
        public static bool isInMenu = false;
        private GameObject menuCanvas;
        private GameObject background;
        private GameObject titleText;

        // Example mod toggle state
        public static bool flyEnabled = false;

        public void ToggleMenu ()
        {
            isInMenu = !isInMenu;

            if (menuCanvas == null)
            {
                CreateWindow();
            }
            else
            {
                menuCanvas.SetActive(isInMenu);
                // If turning it back on, reposition it in front of the player's current view
                if (isInMenu && Camera.main != null)
                {
                    menuCanvas.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1.0f);
                    menuCanvas.transform.rotation= Quaterion.LookRotation(menuCanvas.transform.position - Camera.main.transform.position);
                }
            }
        }

        public void CreateWindow()
        {
            // Fallback position if Camera.main is avalible
            Vector3 spawnPosition = Vector.forward;
            Quaternion spawnRotation = Quaternion.identity;

            if (Camera.main != null)
            {
                spawnPosition = Camera.main.transform.position + (Camera.main.transform.forward * 1.0f);
                spawnRotation = Quaternion.LookRotation(spawnPosition - Camera.main.transform.position);
            }
            
            //1. Create the root World-Space Canvas
            menuCanvas = new GameObject("D0lphinModMenu_Canvas");
            Canvas canvas = menuCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            //   Add CanvasScaler and GraphicRaycaster for UI rendering and futureraycast hits
            menuCanvas.AddComponent<CanvasScaler>();
            menuCanvas.AddComponent<GraphicRaycaster>();

            //   Scale so it fits in the VR world space
            menuCanvas.transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);

            //2. Create Background Panel
            background = new GameObject("BackgroundPanel");
            background.transform.SetParent(menuCanvas.transform, false);

            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.85f); // Dark semi-transparent Grey

            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(400, 500);

            //3. Create the Title Text
            titleText = new GameObject("TitleText");
            titleText.transform.SetParent(background.transform, false);

            Text textComponent = titleText.AddComponent<Text>();
            textComponent.text = "D0lphin Menu";
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 28;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.white;

            RectTransform titleRect = titleText.GetComponent<RectTransform>();
            titleRect.sizeDelta = new Vector2(400, 60);
            titleRect.anchoredPosition = new Vector2(0, 200);

            //4. Create a Stable Button (Fly Mod Toggle)
            CreateButton("Fly: OFF", new Vector2(0, 100), (buttonTextComponent) =>
            {
                flyEnabled = !flyEnabled;
                buttonTextComponent.text = flyEnabled ? "Fly: ON" : "Fly: OFF";
                Debug.Log("Fly mod toggled: " + flyEnabled);
            });

            Debug.Log("Custom Mod Menu Canvas created");
        }

            private void CreateButton(string lable, Vector2 anchoredPosition, Action<Text> onClickAction)
            {
            GameObject buttonObj = new GameObject("Button_" + lable);
            buttonObj.transform.SetParent(background.transform, false);

            //   Add Image for button background
            Image btnImage = buttonObj.AddComponent<Image>();
            btnImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            //   Add button component
            Button btn = buttonObj.AddComponent<Button>();

            ColorBlock colors = btn.colors;
            colors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            colors.highlightedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            colors.pressedColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            btn.colors = colors;

            RectTransform rect = buttonObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(300, 50);
            rect.anchoredPosition = anchoredPosition;

            //   Add Text component inside button
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            Text txt = textObj.AddComponent<Text>();
            txt.text = lable;
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 20;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(300, 50);

            //  Add click listener
            btn.onClick.AddListener(() =>
            {
                onClickAction(txt);
            });
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using Lelly.SDK;

namespace Lelly.Samples
{
    /// <summary>
    /// Exemplo visual de um chat. 
    /// Este script cria automaticamente uma interface básica de chat na sua cena.
    /// </summary>
    public class LellyChatWindow : MonoBehaviour
    {
        private LellyManager lelly;
        private InputField inputField;
        private Text chatLog;
        private ScrollRect scroll;

        void Start()
        {
            lelly = GetComponent<LellyManager>();
            if (lelly == null) lelly = FindAnyObjectByType<LellyManager>();
            
            if (lelly == null || string.IsNullOrEmpty(lelly.apiKey))
            {
                Debug.LogError("[Lelly] LellyManager não encontrado ou sem API Key! Por favor, arraste o componente LellyManager para a cena e configure sua chave.");
                return;
            }

            CreateUI();
            
            // Inicia uma sessão automaticamente para o teste
            lelly.StartNewSession("Jogador Teste", "player@example.com");

            // Inscreve nos eventos
            lelly.onMessageReceived.AddListener(OnMessage);
            lelly.onError.AddListener((err) => AppendLog("<color=red>Erro: " + err + "</color>"));
            
            AppendLog("<color=#6366f1><b>Lelly:</b> Olá! Como posso ajudar você hoje?</color>");
        }

        void OnMessage(string reply)
        {
            AppendLog("<color=#6366f1><b>Lelly:</b> " + reply + "</color>");
        }

        public void Send()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;
            
            string msg = inputField.text;
            AppendLog("<b>Você:</b> " + msg);
            lelly.SendChatMessage(msg);
            inputField.text = "";
            inputField.ActivateInputField();
        }

        void AppendLog(string text)
        {
            chatLog.text += "\n" + text;
            Canvas.ForceUpdateCanvases();
            scroll.verticalNormalizedPosition = 0;
        }

        void CreateUI()
        {
            GameObject canvasObj = new GameObject("LellyChatCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Font selection fallback
            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (defaultFont == null) defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

            // Background
            GameObject bg = new GameObject("Background");
            bg.transform.SetParent(canvasObj.transform);
            Image img = bg.AddComponent<Image>();
            img.color = new Color(0.05f, 0.05f, 0.07f, 0.95f);
            RectTransform rt = bg.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0);
            rt.anchorMax = new Vector2(0.5f, 0);
            rt.pivot = new Vector2(0.5f, 0);
            rt.sizeDelta = new Vector2(400, 600);
            rt.anchoredPosition = new Vector2(0, 50);

            // Scroll View
            GameObject sv = new GameObject("ScrollView");
            sv.transform.SetParent(bg.transform);
            scroll = sv.AddComponent<ScrollRect>();
            rt = sv.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = new Vector2(-40, -120);
            rt.anchoredPosition = new Vector2(0, 40);

            // Viewport & Content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(sv.transform);
            chatLog = content.AddComponent<Text>();
            chatLog.font = defaultFont;
            chatLog.fontSize = 18;
            chatLog.color = Color.white;
            rt = content.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0, 1);
            rt.sizeDelta = new Vector2(0, 500);
            scroll.content = rt;

            // Input Field
            GameObject inputObj = new GameObject("InputField");
            inputObj.transform.SetParent(bg.transform);
            inputField = inputObj.AddComponent<InputField>();
            Image inputImg = inputObj.AddComponent<Image>();
            inputImg.color = new Color(1, 1, 1, 0.1f);
            rt = inputObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.sizeDelta = new Vector2(-100, 40);
            rt.anchoredPosition = new Vector2(-40, 30);

            GameObject inputPlaceholder = new GameObject("Placeholder");
            inputPlaceholder.transform.SetParent(inputObj.transform);
            Text pText = inputPlaceholder.AddComponent<Text>();
            pText.text = "Digite sua mensagem...";
            pText.font = defaultFont;
            pText.fontStyle = FontStyle.Italic;
            pText.color = new Color(1, 1, 1, 0.3f);
            pText.alignment = TextAnchor.MiddleLeft;
            inputField.placeholder = pText;
            rt = inputPlaceholder.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.sizeDelta = Vector2.zero;

            GameObject inputText = new GameObject("Text");
            inputText.transform.SetParent(inputObj.transform);
            Text tText = inputText.AddComponent<Text>();
            tText.font = defaultFont;
            tText.color = Color.white;
            tText.alignment = TextAnchor.MiddleLeft;
            inputField.textComponent = tText;
            rt = inputText.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.sizeDelta = Vector2.zero;

            // Send Button
            GameObject btnObj = new GameObject("SendButton");
            btnObj.transform.SetParent(bg.transform);
            Button btn = btnObj.AddComponent<Button>();
            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.38f, 0.4f, 0.94f);
            rt = btnObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.sizeDelta = new Vector2(70, 40);
            rt.anchoredPosition = new Vector2(-45, 30);
            btn.onClick.AddListener(Send);

            GameObject btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform);
            Text bt = btnTextObj.AddComponent<Text>();
            bt.text = "ENVIAR";
            bt.font = defaultFont;
            bt.alignment = TextAnchor.MiddleCenter;
            bt.color = Color.white;
            bt.fontSize = 12;
            rt = btnTextObj.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.sizeDelta = Vector2.zero;
        }
    }
}

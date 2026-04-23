using UnityEngine;
using UnityEngine.Events;

namespace Lelly.SDK
{
    [HelpURL("https://lelly.chat/developers/docs")]
    public class LellyManager : MonoBehaviour
    {
        [Header("Autenticação")]
        [Tooltip("Sua chave sk_live obtida no painel Lelly")]
        public string apiKey;

        [Header("Configuração de Chat")]
        public string defaultBotSlug = "atendimento";
        
        [TextArea(3, 10)]
        [Tooltip("Instruções de sistema (Prompt) para guiar o comportamento da IA")]
        public string systemInstructions = "Você é um assistente prestativo dentro de um jogo Unity.";
        
        [Header("Eventos")]
        public UnityEvent<string> onMessageReceived = new UnityEvent<string>();
        public UnityEvent<string> onError = new UnityEvent<string>();

        private string currentSessionId;

        void Awake()
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                Debug.LogError("[Lelly] API Key não configurada no LellyManager!");
                return;
            }
            LellyAPI.Instance.Initialize(apiKey);
        }

        /// <summary>
        /// Inicia uma nova sessão de chat.
        /// </summary>
        public void StartNewSession(string userName, string userEmail)
        {
            LellyAPI.Instance.CreateSession(defaultBotSlug, userName, userEmail, systemInstructions, (res) => {
                currentSessionId = res.session_id;
                Debug.Log("[Lelly] Sessão Iniciada: " + currentSessionId);
            }, (err) => {
                onError?.Invoke(err);
            });
        }

        /// <summary>
        /// Envia uma mensagem para a sessão atual.
        /// </summary>
        public void SendChatMessage(string message)
        {
            if (string.IsNullOrEmpty(currentSessionId))
            {
                Debug.LogWarning("[Lelly] Nenhuma sessão ativa. Tente StartNewSession primeiro.");
                return;
            }

            LellyAPI.Instance.SendMessage(currentSessionId, message, (res) => {
                onMessageReceived?.Invoke(res.reply);
            }, (err) => {
                onError?.Invoke(err);
            });
        }

        /// <summary>
        /// Gera conteúdo livre usando LLM.
        /// </summary>
        public void QuickGenerate(string prompt, UnityAction<string> onComplete)
        {
            LellyAPI.Instance.GenerateContent(prompt, (res) => {
                onComplete?.Invoke(res.content);
            }, (err) => {
                onError?.Invoke(err);
            });
        }
    }
}

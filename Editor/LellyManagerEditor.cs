using UnityEngine;
using UnityEditor;
using Lelly.SDK;

namespace Lelly.Editor
{
    [CustomEditor(typeof(LellyManager))]
    public class LellyManagerEditor : UnityEditor.Editor
    {
        private string testMessage = "Olá, Lelly!";
        private string testResult = "";
        private string generatePrompt = "Escreva uma história curta sobre um robô.";
        private string generateResult = "";
        private bool showTester = false;

        public override void OnInspectorGUI()
        {
            LellyManager manager = (LellyManager)target;

            // Header Logo
            EditorGUILayout.Space();
            var style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 18;
            style.normal.textColor = new Color(0.38f, 0.4f, 0.94f); // Indigo
            EditorGUILayout.LabelField("LELLY AI CONTROL CENTER", style);
            EditorGUILayout.Space();

            // Default Inspector
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            showTester = EditorGUILayout.Foldout(showTester, "🧪 EDITOR TESTER (DEBUG)", true);
            
            if (showTester)
            {
                EditorGUILayout.HelpBox("Use esta seção para testar sua integração sem precisar dar Play.", MessageType.Info);
                
                // Chat Test
                EditorGUILayout.LabelField("Teste de Chat", EditorStyles.boldLabel);
                testMessage = EditorGUILayout.TextField("Mensagem", testMessage);
                if (GUILayout.Button("Enviar Mensagem de Teste"))
                {
                    testResult = "Aguardando resposta...";
                    manager.StartNewSession("Editor Tester", "editor@lelly.chat");
                    
                    // Delay a bit or just use the session ID once it's back. 
                    // For the editor, we'll use a slightly different flow.
                    LellyAPI.Instance.CreateSession(manager.defaultBotSlug, "Editor", "editor@test.com", manager.systemInstructions, (res) => {
                        LellyAPI.Instance.SendMessage(res.session_id, testMessage, (msg) => {
                            testResult = "Lelly: " + msg.reply;
                            Repaint();
                        }, (err) => { testResult = "Erro: " + err; Repaint(); });
                    }, (err) => { testResult = "Erro Sessão: " + err; Repaint(); });
                }
                EditorGUILayout.TextArea(testResult, GUILayout.Height(60));

                EditorGUILayout.Space();

                // Generate Test
                EditorGUILayout.LabelField("Teste de Geração (LLM)", EditorStyles.boldLabel);
                generatePrompt = EditorGUILayout.TextArea(generatePrompt, GUILayout.Height(60));
                if (GUILayout.Button("Gerar Conteúdo"))
                {
                    manager.QuickGenerate(generatePrompt, (res) => {
                        generateResult = res;
                        Repaint();
                    });
                    generateResult = "Gerando...";
                }
                EditorGUILayout.TextArea(generateResult, GUILayout.Height(80));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            if (GUILayout.Button("Abrir Documentação On-line"))
            {
                Application.OpenURL("https://lelly.chat/developers/docs");
            }
        }
    }
}

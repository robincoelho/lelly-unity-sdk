# Lelly Unity SDK 🤖🎮

O **SDK oficial da Lelly.chat** para Unity permite integrar inteligência artificial de ponta, gestão de leads e geração de conteúdo dinâmico diretamente em seus jogos e aplicações.

---

## 🌟 O que é a Lelly?

A **[Lelly.chat](https://lelly.chat)** é o sistema operacional de IA para a vida e negócios. Uma plataforma completa que combina atendimento inteligente, CRM e automação para escalar operações de forma humana e eficiente. Com a nossa Developer API, trazemos esse poder para o ecossistema de games e software.

---

## 🚀 Funcionalidades do SDK

- **Chat em Tempo Real**: Integre NPCs inteligentes e assistentes de chat que conhecem o seu negócio.
- **Gestão de Leads**: Capture informações de jogadores e sincronize automaticamente com o CRM da Lelly.
- **Geração de Conteúdo (LLM)**: Use a inteligência da Lelly para gerar diálogos, quests, nomes de itens ou qualquer conteúdo procedural.
- **Lelly Control Center**: Interface customizada no Inspector da Unity para testes rápidos e configuração de prompts sem código.

---

## 📦 Instalação (UPM)

Recomendamos a instalação via **Git URL** para receber atualizações automáticas:

1. No Unity, abra o **Window > Package Manager**.
2. Clique no botão **"+"** no canto superior esquerdo.
3. Selecione **"Add package from git URL..."**.
4. Cole o seguinte link:
   `https://github.com/robincoelho/lelly-unity-sdk.git`
5. Clique em **Add**.

---

## 🛠️ Início Rápido

### 1. Configure o Gerenciador
Arraste o componente `LellyManager` para qualquer GameObject na sua cena. Insira sua **API Key** (`sk_live_...`) obtida no seu painel em Lelly.chat.

### 2. Configure o seu Bot
Defina o `Bot Slug` do agente que você criou na Lelly e personalize as `System Instructions` para guiar o comportamento da IA.

### 3. Use sem Código (UnityEvents)
O `LellyManager` possui eventos nativos:
- `OnMessageReceived(string)`: Disparado quando a Lelly responde.
- `OnError(string)`: Disparado em caso de falha.

### 4. Teste no Editor
Use a seção **Editor Tester** no Inspector do `LellyManager` para conversar com sua IA antes mesmo de dar Play no jogo!

---

## 📖 Documentação Completa

Para detalhes sobre endpoints, custos de API e exemplos avançados, acesse:
👉 **[Documentação para Desenvolvedores](https://lelly.chat/developers/docs)**

---

## ⚖️ Licença

Este SDK está sob a licença MIT. Veja o arquivo [LICENSE.md](LICENSE.md) para detalhes.

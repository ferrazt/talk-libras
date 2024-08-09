using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationRequestHandler : MonoBehaviour
{
    private readonly Queue<string> animationQueue = new Queue<string>();
    private LeftPanelTextManager leftPanelTextManager;
    private BinAnimationLoader binAnimationLoader;
    private Queue<string> transcriptQueue = new Queue<string>();
    private static readonly HttpClient client = new HttpClient();
    private bool isProcessing = false;
    private bool isPlayingAnimations = false;

    // Método para inicializar as dependências
    public void Initialize(LeftPanelTextManager leftPanelTextManager, BinAnimationLoader binAnimationLoader)
    {
        this.leftPanelTextManager = leftPanelTextManager;
        this.binAnimationLoader = binAnimationLoader;
        Debug.Log($"AnimationRequestHandler Initialized: leftPanelTextManager = {this.leftPanelTextManager != null}, binAnimationLoader = {this.binAnimationLoader != null}");
    }

    public void SendTextAndHandleAnimation(string text)
    {
        transcriptQueue.Enqueue(text);
        Debug.Log($"SendTextAndHandleAnimation called with text: {text}");

        if (!isProcessing)
        {
            isProcessing = true;
            _ = ProcessQueue();
        }
    }

    private async Task ProcessQueue()
    {
        while (transcriptQueue.Count > 0)
        {
            var transcript = transcriptQueue.Dequeue();
            await PostRequest("https://traducao2.vlibras.gov.br/translate", transcript);
        }

        isProcessing = false;
    }

    private async Task PostRequest(string url, string text)
    {
        Debug.Log("PostRequest started...");

        // Criar o JSON com o texto
        string json = "{\"text\": \"" + text + "\", \"domain\": \"www.gov.br\"}";
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        Debug.Log($"PostRequest JSON: {json}");

        try
        {
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.Log("Resposta recebida: " + responseBody);

            // Dividir a resposta em palavras e adicionar à fila de animação
            string[] words = responseBody.Split(' ');
            foreach (string word in words)
            {
                Debug.Log("Palavra: " + word);
                string path = "Assets/BUNDLER/animations/" + word;

                if (System.IO.File.Exists(path))
                {
                    animationQueue.Enqueue(path);
                }
                else
                {
                    Debug.LogWarning("Arquivo de animação não encontrado para a palavra: " + path);
                    // Desmembrar a palavra em letras e enfileirar cada letra
                    string cleanWord = "";
                    if (word.Contains("&"))
                    {
                        int ampersandIndex = word.IndexOf('&');
                        cleanWord = word.Substring(0, ampersandIndex);
                    }
                    else
                    {
                        cleanWord = word;
                    }

                    foreach (char letter in cleanWord)
                    {
                        string letterPath = "Assets/BUNDLER/animations_alfabeto/" + letter;
                        string numberPath = "Assets/BUNDLER/animations_numeros/" + letter;
                        if (System.IO.File.Exists(letterPath))
                        {
                            animationQueue.Enqueue(letterPath);
                        }
                        else if (System.IO.File.Exists(numberPath))
                        {
                            animationQueue.Enqueue(numberPath);
                        }
                        else
                        {
                            Debug.LogWarning("Arquivo de animação não encontrado para a letra: " + letterPath);
                        }
                    }
                }
            }

            // Iniciar a execução das animações
            if (animationQueue.Count > 0)
            {
                Debug.Log("CHAMOU O PLAYANIMATIONS");
                if (!isPlayingAnimations)
                {
                    isPlayingAnimations = true;
                    _ = LoadAndPlayAnimations();
                }
            }
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Erro na requisição: {e.Message}");
            if (leftPanelTextManager != null)
            {
                leftPanelTextManager.UpdateText("Erro ao enviar: " + e.Message);
            }
        }
    }

    private async Task LoadAndPlayAnimations()
    {
        Debug.Log("LoadAndPlayAnimations started...");

        while (animationQueue.Count > 0)
        {
            string animationPath = animationQueue.Dequeue();
            string animationClipName = System.IO.Path.GetFileName(animationPath);

            Debug.Log("Carregando animação: " + animationClipName);
            binAnimationLoader.LoadAnimationTarget(animationPath, animationClipName);

            Debug.Log("Reproduzindo animação: " + animationClipName);
            
            binAnimationLoader.PlayAnimation(animationClipName);
            while (binAnimationLoader.IsAnimationPlaying())
            {
                await Task.Delay(100); // Verificar a cada 100ms
            }

            Debug.Log("Animação concluída: " + animationClipName);
        }

        isPlayingAnimations = false;
        Debug.Log("Todas as animações foram reproduzidas.");
    }
}

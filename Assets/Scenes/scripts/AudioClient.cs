using System;
using System.Text.Json;
using System.Threading.Tasks;
using Deepgram;
using Deepgram.Models.Authenticate.v1;
using Deepgram.Models.Live.v1;
using Deepgram.Logger;
using Deepgram.Microphone;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioClient : MonoBehaviour
{
    private const string DeepgramApiKey = "2c401a5aa731ae9bf1be5b9cd7a88723ed80a398";
    private LiveClient liveClient;
    private Deepgram.Microphone.Microphone microphone;
    public LeftPanelTextManager leftPanelTextManager;
    public BinAnimationLoader binAnimationLoader;
    private AnimationRequestHandler animationRequestHandler;
    public TMP_InputField inputField;
    private string accumulatedTranscript = "";
    //private string lastTranscript = "";
    private TaskCompletionSource<bool> animationCompletionSource;
    private readonly char[] sentenceEndings = { '.', ',', '?' };

    public async Task StartTranscription()
    {
        Debug.Log("Iniciando a transcrição...");
        animationCompletionSource = new TaskCompletionSource<bool>();
        animationRequestHandler = gameObject.AddComponent<AnimationRequestHandler>();

        animationRequestHandler.Initialize(leftPanelTextManager, binAnimationLoader);
        Deepgram.Library.Initialize(LogLevel.Debug);
        Deepgram.Microphone.Library.Initialize();
        liveClient = new LiveClient(DeepgramApiKey);

        await Task.Run(() => InitializeAndStartLiveClient());
        Debug.Log("Transcrição iniciada.");
    }

    public async Task StopTranscription()
    {
        Debug.Log("Parando a transcrição...");
        await Task.Run(() => StopAll());
        Debug.Log("Transcrição parada.");
    }

    private async Task InitializeAndStartLiveClient()
    {
        Debug.Log("Inicializando e iniciando LiveClient em uma tarefa separada...");

        // Subscribe to events
        liveClient.Subscribe(new EventHandler<ResultResponse>((sender, e) =>
        {
            if (e.Channel.Alternatives[0].Transcript.Trim() == "")
            {
                return;
            }
            string transcript = e.Channel.Alternatives[0].Transcript;
            //Debug.Log($"----> Falante: {transcript}");

            accumulatedTranscript += " " + transcript;

            int index = accumulatedTranscript.IndexOfAny(sentenceEndings);
            while (index >= 0)
            {
                string textToSend = accumulatedTranscript.Substring(0, index + 1).Trim();
                accumulatedTranscript = accumulatedTranscript.Substring(index + 1).Trim();

                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    animationRequestHandler.SendTextAndHandleAnimation(textToSend);
                    if (animationCompletionSource != null && !animationCompletionSource.Task.IsCompleted)
                    {
                        animationCompletionSource.SetResult(true);
                    }
                    inputField.text = ""; // Limpar o campo de entrada

                    if (leftPanelTextManager != null)
                    {
                        leftPanelTextManager.UpdateText(textToSend);
                    }
                });

                index = accumulatedTranscript.IndexOfAny(sentenceEndings);
            }
        }));

        //liveClient.Subscribe(new EventHandler<ResultResponse>((sender, e) =>
        //{
        //    if (e.Channel.Alternatives[0].Transcript.Trim() == "")
        //    {
        //        return;
        //    }

        //    string transcript = e.Channel.Alternatives[0].Transcript;
        //    int transcriptLength = transcript.Length;
        //    Debug.Log($"----> Falante: {transcript} (Tamanho: {transcriptLength} caracteres)");

        //    // Se a nova transcrição é um complemento da última
        //    if (transcriptLength >= lastTranscript.Length && transcript.Substring(0, lastTranscript.Length).Equals(lastTranscript, StringComparison.OrdinalIgnoreCase))
        //    {
        //        accumulatedTranscript = transcript;
        //        lastTranscript = transcript.Length > 3 ? transcript.Substring(0, transcript.Length - 3) : transcript;
        //        return;
        //    }
        //    else
        //    {
        //        // Se a nova transcrição é diferente, envie a acumulada e atualize
        //        Debug.Log("Se a nova transcrição é diferente, envie a acumulada e atualize: "  + accumulatedTranscript);
        //        //UnityMainThreadDispatcher.Enqueue(() =>
        //        //{
        //        //    animationRequestHandler.SendTextAndHandleAnimation(accumulatedTranscript);
        //        //});

        //        accumulatedTranscript = transcript;
        //        lastTranscript = transcript.Length > 3 ? transcript.Substring(0, transcript.Length - 3) : transcript;
        //        return;
        //    }
        //}));

        var liveSchema = new LiveSchema()
        {
            Language = "pt-BR",
            Model = "nova-2",
            Encoding = "linear16",
            SampleRate = 16000,
            Punctuate = true,
            SmartFormat = true,
            //InterimResults = true,
            //UtteranceEnd = "1000",
            //VadEvents = true
        };

        try
        {
            await liveClient.Connect(liveSchema);
            Debug.Log("Conexão com o Deepgram estabelecida.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao conectar com o Deepgram: {ex.Message}");
            return;
        }

        // Microphone streaming
        microphone = new Deepgram.Microphone.Microphone(liveClient.Send);
        microphone.Start();
    }

    private async Task StopAll()
    {
        Debug.Log("Parando o microfone e a conexão...");

        // Aguardar a conclusão da animação
        if (animationCompletionSource != null)
        {
            // Verificar se o TaskCompletionSource já foi completado
            if (!animationCompletionSource.Task.IsCompleted)
            {
                await animationCompletionSource.Task;
                Debug.LogWarning("Terminou de liberar o animation");
            }
            animationCompletionSource = null; // Limpar a referência após a conclusão
        }

        // Stop the microphone
        if (microphone != null)
        {
            microphone.Stop();
            microphone = null;
        }

        // Stop the connection
        if (liveClient != null)
        {
            await liveClient.Stop();
            liveClient = null;
        }

        Deepgram.Microphone.Library.Terminate();
        Deepgram.Library.Terminate();

        Debug.Log("Todos os recursos foram liberados.");
    }



    void OnApplicationQuit()
    {
        _ = StopAll();
    }
}

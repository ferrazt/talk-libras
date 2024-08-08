using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;

public class OpenAIWhisperClient : MonoBehaviour
{

    public TextMeshProUGUI resultText;
    private AudioClip audioClip;
    private int sampleRate = 44100;
    private int segmentLength = 10; // Segment length in seconds
    private const string apiKey = "sk-proj-POKjM6CXns2pmrixwV4cT3BlbkFJTgQcpqRHmuzLPqGlA43k"; // Substitua pela sua chave API do OpenAI
    private const string apiUrl = "https://api.openai.com/v1/audio/transcriptions";

    void Start()
    {
        StartRecording();
    }

    void StartRecording()
    {
        audioClip = Microphone.Start(null, false, segmentLength, sampleRate);
        StartCoroutine(SendAudio());
    }

    void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
        }
    }

    private IEnumerator SendAudio()
    {
        yield return new WaitForSeconds(segmentLength);
        int length = sampleRate * segmentLength;
        float[] samples = new float[length];
        audioClip.GetData(samples, 0);

        byte[] audioData = ConvertToWav(samples, length, sampleRate);

        StartCoroutine(UploadAudio(audioData));
    }

    private byte[] ConvertToWav(float[] samples, int length, int sampleRate)
    {
        int headerSize = 44;
        int byteArraySize = samples.Length * 2 + headerSize;
        byte[] bytes = new byte[byteArraySize];

        using (MemoryStream memoryStream = new MemoryStream(bytes))
        {
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
                writer.Write(byteArraySize - 8);
                writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
                writer.Write(new char[4] { 'f', 'm', 't', ' ' });
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)1);
                writer.Write(sampleRate);
                writer.Write(sampleRate * 2);
                writer.Write((ushort)2);
                writer.Write((ushort)16);
                writer.Write(new char[4] { 'd', 'a', 't', 'a' });
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
        }

        return bytes;
    }

    private IEnumerator UploadAudio(byte[] audioData)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "recordedAudio.wav", "audio/wav");

        UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            var result = JsonUtility.FromJson<TranscriptionResult>(jsonResponse);
            resultText.text = result.text;
        }
    }

    [System.Serializable]
    public class TranscriptionResult
    {
        public string text;
    }
}

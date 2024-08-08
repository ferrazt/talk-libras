using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BinAnimationLoader : MonoBehaviour
{
    public GameObject targetModel;
    public Dictionary<string, AnimationClip> loadedClips = new Dictionary<string, AnimationClip>();

    void Start()
    {
        if (targetModel == null)
        {
            Debug.LogError("Target Model não atribuído. Verifique o campo no Inspector.");
            return;
        }

        // Carregar todas as animações das pastas "animations_numeros" e "animations_alfabeto"
        LoadAllAnimationsInFolder("Assets/BUNDLER/animations_numeros");
        LoadAllAnimationsInFolder("Assets/BUNDLER/animations_alfabeto");
    }

    public void LoadAllAnimationsInFolder(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".meta") continue;

            string animationClipName = Path.GetFileName(file);
            LoadAnimationTarget(file, animationClipName);
        }
    }

    public void LoadAnimationTarget(string bundlePath, string animationClipName)
    {
        //Debug.Log("CHEGANDO AQUI...");
        if (!loadedClips.ContainsKey(animationClipName))
        {
            //Debug.Log("CHAMANOD LoadAnimationFromBundle");
            LoadAnimationFromBundle(bundlePath, animationClipName);
        }
    }

    private void LoadAnimationFromBundle(string bundlePath, string animationClipName)
    {
        //Debug.Log("TESTE 1");
        // Carregar o Asset Bundle
        AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
        if (bundle == null)
        {
            Debug.LogError("Falha ao carregar Asset Bundle: " + bundlePath);
            return;
        }
        // Debug.Log("TESTE 2");
        // Carregar o AnimationClip do Asset Bundle
        AnimationClip clip = bundle.LoadAsset<AnimationClip>(animationClipName);
        if (clip == null)
        {
            Debug.LogError("Falha ao carregar AnimationClip: " + animationClipName);
            return;
        }

        //Debug.Log("AnimationClip carregado com sucesso.");

        // Adicionar o componente Animation ao modelo se não existir
        Animation animation = targetModel.GetComponent<Animation>();
        if (animation == null)
        {
            animation = targetModel.AddComponent<Animation>();
        }

        // Adicionar o AnimationClip ao componente Animation
        animation.AddClip(clip, animationClipName);
        loadedClips[animationClipName] = clip; // Armazenar o clip no dicionário

        //Debug.Log("AnimationClip adicionado ao componente Animation.");

        // Descarregar o Asset Bundle para liberar memória
        bundle.Unload(false);

        //Debug.Log("Asset Bundle descarregado.");
    }

    public void PlayAnimation(string animationClipName, float speed)
    {
        Animation animation = targetModel.GetComponent<Animation>();
        if (animation != null && loadedClips.ContainsKey(animationClipName))
        {
            animation.clip = loadedClips[animationClipName];
            animation[animationClipName].speed = speed; // Ajustar a velocidade da animação
            animation.Play(animationClipName);
            Debug.Log("Animação reproduzida: " + animationClipName + " com velocidade: " + animation[animationClipName].speed);
        }
        else
        {
            Debug.LogError("AnimationClip " + animationClipName + " não encontrado no componente Animation.");
        }
    }

    public bool IsAnimationPlaying()
    {
        if (targetModel.TryGetComponent<Animation>(out var animation))
        {
            return animation.isPlaying;
        }
        return false;

    }
}

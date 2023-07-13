using System.Collections;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] [TextArea] private string[] itemInfo;
    [SerializeField] private float textSpeed = 0.01f;

    [Header("UI Elements")]
    [SerializeField] private TMPro.TextMeshPro itemInfoText;
    private int currentDisplayingText = 0;
    // Start is called before the first frame update
    void Start()
    {
        itemInfo = new string[] {
            "Creators/Designers: \nCharles Cusack\nJustin Fay\nJack Rutherford",
            "Funding: \nHope College\nHope College Computer Science\n",
            "Asset: Acoustic_Crash_Cymbal1.wav. (n.d.). Freesound. https://freesound.org/people/mhc/sounds/102782/",
            "Acoustic_tom1.wav. (n.d.). Freesound. https://freesound.org/people/mhc/sounds/102797/",
            "Acoustic_tom3.wav. (n.d.). Freesound. https://freesound.org/people/mhc/sounds/102799/",
            "cymbal big2.wav. (n.d.). Freesound. https://freesound.org/people/shpira/sounds/317078/\r\n",
            "Drums set | 3D Furniture | Unity Asset Store. (n.d.). Unity Asset Store. https://assetstore.unity.com/packages/3d/props/furniture/drums-set-205334",
            "Free Audio Zone. (2018, May 12). Treasure Chest Sound Effect [Video]. YouTube. https://www.youtube.com/watch?v=MmhwhVnIV2U",
            "Free Low Poly Nature Forest | 3D Landscapes | Unity Asset Store. (2022, February 2). Unity Asset Store. https://assetstore.unity.com/packages/3d/environments/landscapes/free-low-poly-nature-forest-205742",
            "Keijiro. (n.d.). GitHub - keijiro/MoonAndEarth: Moon and Earth assets for Unity. GitHub. https://github.com/keijiro/MoonAndEarth",
            "[Kick] Punchy Trance Kick 4. (n.d.). Freesound. https://freesound.org/people/waveplaySFX/sounds/344757/\r\n",
            "LlamAcademy. (2021, November 2). Navmeshagent Avoidance in depth - 5 Key Takeaways for Optimal Avoidance | AI Series Part 32 [Video]. YouTube. https://www.youtube.com/watch?v=dHYcio6fRI4\r\n",
            "Low Poly Musical Instruments | 3D Props | Unity Asset Store. (2022, April 5). Unity Asset Store. https://assetstore.unity.com/packages/3d/props/low-poly-musical-instruments-196081\r\n",
            "Low Poly Rock Pack | 3D Environments | Unity Asset Store. (2016, November 29). Unity Asset Store. https://assetstore.unity.com/packages/3d/environments/low-poly-rock-pack-57874\r\n",
            "Low Poly Tree Pack | 3D Trees | Unity Asset Store. (2016, March 24). Unity Asset Store. https://assetstore.unity.com/packages/3d/vegetation/trees/low-poly-tree-pack-57866\r\n",
            "Low-poly musical instruments | 3D Props | Unity Asset Store. (n.d.). Unity Asset Store. https://assetstore.unity.com/packages/3d/props/low-poly-musical-instruments-199705",
            "My Drum | 3D Props | Unity Asset Store. (n.d.). Unity Asset Store. https://assetstore.unity.com/packages/3d/props/my-drum-149676",
            "PBR Sand Materials free | 2D Floors | Unity Asset Store. (2020, March 23). Unity Asset Store. https://assetstore.unity.com/packages/2d/textures-materials/floors/pbr-sand-materials-free-160123",
            "Peer Play. (2015, April 5). Ripple Water Shader - Unity CG/C# Tutorial [Part 2/3] [Video]. YouTube. https://www.youtube.com/watch?v=-R7TAVNtYMA",
            "Pixabay. (n.d.). Free Electronic Snare 0-30 Sound Effects download - Pixabay. https://pixabay.com/sound-effects/search/electronic%20snare/?duration=0-30",
            "Polygonal’s Low-poly Particle Pack | VFX Particles | Unity Asset Store. (2020, April 6). Unity Asset Store. https://assetstore.unity.com/packages/vfx/particles/polygonal-s-low-poly-particle-pack-118355\r\n",
            "Sfikas, K. (n.d.). Procedural Water Surface, made in Unity3D. https://www.konsfik.com/procedural-water-surface-made-in-unity3d/",
            "Simple Button Set 01 | 2D Icons | Unity Asset Store. (2020, September 13). Unity Asset Store. https://assetstore.unity.com/packages/2d/gui/icons/simple-button-set-01-153979",
            "Simple side-menu | GUI Tools | Unity Asset Store. (2019, September 10). Unity Asset Store. https://assetstore.unity.com/packages/tools/gui/simple-side-menu-143623?aid=1011leVbt&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker",
            "SoundEffectsArchive. (2014, February 27). Snare drum hit sound effect [Video]. YouTube. https://www.youtube.com/watch?v=MR8UGcv-Wpg",
            "Spontaneous Simulations. (2022, April 28). Use compute shaders to create water effect in Unity. Now you can simulate ripples of a moving boat! [Video]. YouTube. https://www.youtube.com/watch?v=4CNad5V9wD8",
            "Terrain Sample Asset Pack | 3D Landscapes | Unity Asset Store. (2020, December 16). Unity Asset Store. https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808#description",
        };
    }

    public void ActivateText() {
        StopAllCoroutines();
        StartCoroutine(AnimateText());
        currentDisplayingText++;
        currentDisplayingText = (currentDisplayingText >= itemInfo.Length) ? 0 : currentDisplayingText;
    }

    IEnumerator AnimateText() {
        for(int i =0; i < itemInfo[currentDisplayingText].Length + 1; i++) {
            itemInfoText.text = itemInfo[currentDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
    }
}

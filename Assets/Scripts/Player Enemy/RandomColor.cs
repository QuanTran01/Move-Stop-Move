using UnityEngine;
using TMPro;

public class RandomColorText : MonoBehaviour
{
    public Material[] materials;
    public TMP_Text[] textComponents;

    void Start()
    {
        Material randomMaterial = materials[Random.Range(0, materials.Length)];
        Color randomColor = randomMaterial.color;

        GetComponent<Renderer>().material = randomMaterial;

        foreach (TMP_Text textComponent in textComponents)
        {
            if (textComponent != null)
            {
                textComponent.color = randomColor;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// The two following classes are only used to create 
// the [ReadOnly] attribute
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}





public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int grid_size;

    [Header("Primary Map")]
    [SerializeField]
    [Range (0f,1f)]
    private float PerlinScale;

    private float[,] noiseMap;

    [SerializeField]
    [ReadOnly]
    private float noiseMapMeanValue;

    [SerializeField]
    private Renderer textureRenderer;

    [SerializeField]
    private bool shouldRender_PrimaryMap;

    [Header ("Misc")]
    [SerializeField]
    private int nb_playable_blocks;

    [SerializeField]
    [Range (0,100)]
    private int train_speed;

    [SerializeField]
    private float block_size;

    [SerializeField]
    private List<GameObject> blocks_template;

    private GameObject map;

    public void generate()
    {
        /* TODO
         * 
         * - Generate map with perlin noise -> OK
         * - Create a sure path with a*
         * - Take out random blocks that the player will later place
         * - Send for rendering
         * 
         * */
        if(map == null)
        {
            map = new GameObject();
        }
        if(map.transform.childCount != 0)
        {
            Destroy();
            map = new GameObject();
        }

        computeNoiseMap();
        generateSimpleMap();
        findPath();
        breakPath();
    }

    private void computeNoiseMap()
    {
        noiseMap = new float[grid_size, grid_size];

        if (PerlinScale <= 0)
        {
            PerlinScale = 0.0001f;
        }

        for (int y = 0; y < grid_size; y++)
        {
            for (int x = 0; x < grid_size; x++)
            {
                float sampleX = x / (Time.time * PerlinScale);
                float sampleY = y / (Time.time * PerlinScale);

        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        computeNoiseMapMeanValue();

        noiseMap[0, 0] = 1f;
        noiseMap[grid_size - 1, grid_size - 1] = 1f;

        binarizeNoiseMap();

        if (shouldRender_PrimaryMap)
        {
            textureRenderer.enabled = true;
            renderNoiseMap();
        }
        else
        {
            textureRenderer.enabled = false;
        }
    }

    private void renderNoiseMap()
    {

        Texture2D texture = new Texture2D(grid_size, grid_size);

        Color[] colorMap = new Color[grid_size * grid_size];
        for (int y = 0; y < grid_size; y++)
        {
            for (int x = 0; x < grid_size; x++)
            {
                colorMap[y * grid_size + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(grid_size, 1, grid_size);
    }

    private void computeNoiseMapMeanValue()
    {
        float mean = 0;
        int nb_cells = grid_size * grid_size;

        for (int y = 0; y < grid_size; y++)
        {
            for (int x = 0; x < grid_size; x++)
            {
                mean += noiseMap[x, y];
            }
        }

        noiseMapMeanValue = mean / nb_cells;
    }

    public float retrieveNoiseMapMeanValue()
    {
        return noiseMapMeanValue;
    }
    
    private void binarizeNoiseMap()
    {
        for (int y = 0; y < grid_size; y++)
        {
            for (int x = 0; x < grid_size; x++)
            {
                if(noiseMap[x,y] < noiseMapMeanValue)
                {
                    noiseMap[x, y] = 0f;
                }
                else
                {
                    noiseMap[x,y] = 1f;
                }
            }
        }
    }

    private void generateSimpleMap()
    {
        for (int y = 0; y < grid_size; y++)
        {
            for (int x = 0; x < grid_size; x++)
            {
                if (noiseMap[x,y] == 0f)
                {
                    float real_x = x * block_size;
                    float real_z = y * block_size;
                    Instantiate(blocks_template[2], new Vector3(real_x, 10, real_z), Quaternion.identity).transform.parent = map.transform;
                }
                else
                {
                    float real_x = x * block_size;
                    float real_z = y * block_size;
                    Instantiate(blocks_template[1], new Vector3(real_x, 10, real_z), Quaternion.identity).transform.parent = map.transform;
                }

            }
        }

        // I do that because the displayed map is not facing the direction I want
        map.transform.Rotate(new Vector3(0f, 1f, 0f), 90f);
}
    
    
    private void findPath()
    {


    }

    private void breakPath()
    {

    }

    public void Destroy()
    {
        DestroyImmediate(map);
    }
}

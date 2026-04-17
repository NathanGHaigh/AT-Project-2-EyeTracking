using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif




[ExecuteInEditMode()]
public class LinearProgressBar : MonoBehaviour
{

#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Linear Progress Bar")]
    public static void CreateLinearProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ProgressBar"));
        obj.transform.SetParent(Selection.activeTransform, false);
    }
#endif


    public float minimum;
    public float maximum;
    public float currentValue;
    public Image mask;
    public Image fill;
    [SerializeField] private Color color;

    private void Start()
    {
        SetColor(color);
    }

    private void Update()
    {
        GetCurrentFill();

    }

    void GetCurrentFill()
    {
        float currentOffset = currentValue - minimum;
        float totalOffset = maximum - minimum;
        float fillAmount = currentOffset / totalOffset;
        mask.fillAmount = fillAmount;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        fill.color = newColor;
        Debug.Log("Color set to: " + newColor);
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextHoverColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Graphic graphicComponent;
    private Color originalColor;
    public Color hoverColor = Color.red; // Change this to the desired hover color

    private void Start()
    {
        graphicComponent = GetComponent<Graphic>();

        if (graphicComponent != null)
        {
            // Store the original color of the graphic component
            originalColor = graphicComponent.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ModifyColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ModifyColor(originalColor);
    }

    private void ModifyColor(Color color)
    {
        if (graphicComponent != null)
        {
            // Modify color for both Text and other Graphic components
            graphicComponent.color = color;
        }
    }
}

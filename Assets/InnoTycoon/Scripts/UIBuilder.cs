namespace innovation.tycoon
{
using System;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

public class UIBuilder : MonoBehaviour
{
    public Canvas canvas;
    public Image image;

    protected Vector3 nextLayoutPosition;

    protected virtual void Awake()
    {
        FindCanvas();
    }

    protected void AddButton(Button prefab, string name, UnityAction callback, bool interactable)
    {
        Button button = Instantiate(prefab);
        button.name = name;
        button.onClick.AddListener(callback);
        button.interactable = interactable;

        Text text = button.GetComponentInChildren<Text>();
        if (text)
            text.text = name;


        RectTransform transform = button.GetComponent<RectTransform>();
        if(image) {
            transform.SetParent(image.transform);
        }
        else {
            transform.SetParent(canvas.transform);
        }
        transform.anchoredPosition = nextLayoutPosition;
        nextLayoutPosition += Vector3.down * transform.rect.height;
    }

    private void FindCanvas()
    {
        if (!canvas) {
            canvas = FindObjectOfType<Canvas>();
            FindImage();
        }

        if (!canvas)
            throw new InvalidOperationException("You need a canvas in your scene, or add code to create one here.");
    }
    private void FindImage()
    {
        if (!image)
            image = FindObjectOfType<Image>();

    }
}
}
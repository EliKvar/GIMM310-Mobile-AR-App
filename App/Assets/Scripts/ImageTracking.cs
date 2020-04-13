using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageTracking : MonoBehaviour
{
   public ARTrackedImageManager arTrackedImageManager;
   public Text debugText;

    void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var trackedImage in args.added)
        {
            if(trackedImage.name == "washHandsImg")
            {
                debugText.text = trackedImage.name;
            }
        }
    } 
}

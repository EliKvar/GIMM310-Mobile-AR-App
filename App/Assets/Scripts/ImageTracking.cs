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


    // void Awake()
    // {
        
    // }
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
        
        // foreach(var trackedImage in args.added)
        // {
        //     if(trackedImage.referenceImage.name == "washHandsImg")
        //     {
        //         debugText.text = trackedImage.referenceImage.name;
        //     }
        //     else if(trackedImage.referenceImage.name == "img2")
        //     {
        //         debugText.text = trackedImage.referenceImage.name;
        //     }
        // }
        foreach(var trackedImage in args.removed)
        {
            debugText.text = trackedImage.referenceImage.name;
            if(trackedImage.referenceImage.name == "washHandsImg")
            {
                debugText.text = trackedImage.referenceImage.name + " removed";
            }
            else if(trackedImage.referenceImage.name == "img2")
            {
                debugText.text = trackedImage.referenceImage.name + " removed";
            }
        }
    } 
}

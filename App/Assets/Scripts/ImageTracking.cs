using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracking : MonoBehaviour
{
    public ARTrackedImageManager arTrackedImageManager;
    public Text debugText;
    public GameObject[] arObjectsToPlace;
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

   void Awake()
    { 
        // setup all game objects in dictionary
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            newARObject.SetActive(false);
            arObjects.Add(arObject.name, newARObject);
        }
    }

    void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged; // run the method below on the event the tracked images have changed in some way
    }
    void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        // int count = 1; // counter so we don't track more than one image at a time
        // foreach(var trackedImage in args.added) // looping through all the images currently being tracked (seen)
        // {
        //     if(trackedImage.trackingState == TrackingState.Limited)
        //     {
        //         trackedImage.Destroy(trackedImage);
        //     }
        //     else
        //     {
        //         if(trackedImage.referenceImage.name == "washHandsImg") // the image of someone rinsing their hands at the sink
        //         {
        //             debugText.text = trackedImage.trackingState.ToString();
        //         }
        //         else if(trackedImage.referenceImage.name == "img2") // the box/code image thing
        //         {
        //             //debugText.text = trackedImage.referenceImage.name;
        //         }
        //     }
        //     count++; // increment count for next iteration
        // }
        foreach (ARTrackedImage trackedImage in args.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            if(trackedImage.trackingState == TrackingState.Limited || trackedImage.trackingState == TrackingState.None)
            {
                arObjects[trackedImage.referenceImage.name].SetActive(false);
            }
            else
            {
                UpdateARImage(trackedImage); 
            }
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        debugText.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);

        Debug.Log($"trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if(arObjectsToPlace != null)
        {
            GameObject goARObject = arObjects[name];
            goARObject.SetActive(true);
            goARObject.transform.position = newPosition;
            //goARObject.transform.localScale = scaleFactor;
            foreach(GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if(go.name != name)
                {
                    go.SetActive(false);
                }
            } 
        }
    }
}

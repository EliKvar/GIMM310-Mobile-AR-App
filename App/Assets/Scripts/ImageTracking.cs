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
    private Vector3 scaleFactor = new Vector3(.1f, .1f, .1f);
    private List<Animator> animators = new List<Animator>();

        //This function triggers the end screen. Add where needed
       // UIController.control.SwitchToEndScreen();

void Awake()
    {
        
        // setup all game objects in dictionary
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject);
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

        // this is my code that wasn't hiding the game objects when the tracking state went to limited (uncommented code is just an ARCore workaround, apparently my code would have worked on IOS)
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
        //debugText.text = trackedImage.referenceImage.name;

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
            goARObject.transform.localScale = scaleFactor;
            foreach(GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if(go.name != name)
                {
                    StopAnimations(go.name);
                    go.SetActive(false);
                }
            }
            PlayCardAnimations(name);
        }
    }

    private void PlayCardAnimations(string name)
    {
        if(name == "Card1Animation")
        {
            GameObject goARObject = arObjects[name];
            Animator anim = goARObject.transform.Find("LeftHandPrefab").GetComponent<Animator>();
            animators.Add(anim);
            anim = goARObject.transform.Find("RightHandPrefab").GetComponent<Animator>();
            animators.Add(anim);
            anim = goARObject.transform.Find("SinkPrefab").Find("water").GetComponent<Animator>();
            animators.Add(anim);
            debugText.text = name;
        }

        foreach(Animator animator in animators)
        {
            animator.SetBool("start",true);
            //debugText.text = $"Playing {name} animation";
        }
        
    }

    private void StopAnimations(string name)
    {
        foreach(Animator animator in animators)
        {
            animator.SetBool("start", false);
            animators.Remove(animator);
        }
    }
}

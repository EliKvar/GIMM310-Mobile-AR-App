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

    private List<string> shownAnimations = new List<string>();
    //This function triggers the end screen. Add where needed
    // UIController.control.SwitchToEndScreen();

    void Awake()
    {

        // setup all game objects in dictionary
        foreach (GameObject arObject in arObjectsToPlace)
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
        foreach (ARTrackedImage trackedImage in args.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            if (trackedImage.trackingState == TrackingState.Limited || trackedImage.trackingState == TrackingState.None)
            {
                arObjects[trackedImage.referenceImage.name].SetActive(false);
            }
            else
            {
                UpdateARImage(trackedImage);
            }
        }
    }

    void Update()
    {
        if(shownAnimations.Count == 5)
        {
            StartCoroutine(sendToWin());
        }
    }
    private IEnumerator sendToWin()
    {
        yield return new WaitForSeconds(9.0f);
        UIController.control.SwitchToEndScreen();
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
        if (arObjectsToPlace != null)
        {
            GameObject goARObject = arObjects[name];
            goARObject.SetActive(true);
            goARObject.transform.position = newPosition;
            goARObject.transform.localScale = scaleFactor;

            foreach (GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if (go.name != name)
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
        GameObject goARObject = arObjects[name];
        Animator animator = goARObject.transform.GetComponent<Animator>();
        string boolName = "";
        if(name == "Card1Animation")
        {
            boolName = "card1";
        }
        else if(name == "Card2Animation")
        {
            boolName = "card2";
        }
        else if(name == "Card3Animation")
        {
            boolName = "card3";
        }
        else if(name == "Card4Animation")
        {
            boolName = "card4";
        }
        else if(name == "Card5Animation")
        {
            boolName = "card5";
        }
        animator.SetBool(boolName, true);

        if(!shownAnimations.Contains(name))
        {
            shownAnimations.Add(name);
        }
        //debugText.text = $"Playing {name} animation";
    }

    private void StopAnimations(string name)
    {
        GameObject goARObject = arObjects[name];
        Animator animator = goARObject.transform.GetComponent<Animator>();
        string boolName = "";
        if(name == "Card1Animation")
        {
            boolName = "card1";
        }
        else if(name == "Card2Animation")
        {
            boolName = "card2";
        }
        else if(name == "Card3Animation")
        {
            boolName = "card3";
        }
        else if(name == "Card4Animation")
        {
            boolName = "card4";
        }
        else if(name == "Card5Animation")
        {
            boolName = "card5";
        }
        animator.SetBool(boolName, true);
    }
}

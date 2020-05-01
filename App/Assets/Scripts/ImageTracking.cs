using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Vector3 scaleFactor = new Vector3(.3f, .3f, .3f);

    private List<string> shownAnimations = new List<string>();
    public GameObject card1UI;
    public GameObject card2AND3UI;
    public GameObject card4UI;
    public GameObject card5UI;

    public float targetTime = 3.0f;
        /* These control the UI for the cards, set them appropriately in the logic.
        card1UI.SetActive();
        card2AND3UI.SetActive();
        card4UI.SetActive();
        card5UI.SetActive();
        */
    void Awake()
    {
        //Initializing beginning card UI states. This is not neccesary but may help prevent unexpected issues
        card1UI.SetActive(true);
        card2AND3UI.SetActive(false);
        card4UI.SetActive(false);
        card5UI.SetActive(false);

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
            shownAnimations.RemoveAt(0);
            shownAnimations.RemoveAt(1);
            shownAnimations.RemoveAt(2);
            shownAnimations.RemoveAt(3);
        }
        
        //targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerEnded();
            targetTime = 50000;
        }
       
       
}
    void timerEnded()
    {

        shownAnimations.Add("test0");
        shownAnimations.Add("test1");
        shownAnimations.Add("test2");
        shownAnimations.Add("test3");
        shownAnimations.Add("test4");
        Debug.Log("shownAnimations.Count = " + shownAnimations.Count +". Now waiting for 9 seconds, representing the final animation.");
    }  
    private IEnumerator sendToWin()
    {
        yield return new WaitForSeconds(9.0f);
        SceneManager.LoadScene("EndScreenUI");
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

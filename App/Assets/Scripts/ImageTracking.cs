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
    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public AudioClip[] voiceOvers;
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
        card1UI.SetActive(false);
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
            if (trackedImage.trackingState == TrackingState.Limited || trackedImage.trackingState == TrackingState.None) // limited was added here for it to work in ARCore, for some reason in ARCore nothing ever goes to None
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
        yield return new WaitForSeconds(9.0f); // the length of the animation of card5
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
        string boolName = "";
        float seconds = 0f; // this will be to pass to the IEnumerator so when the voiceover is done the animation will play and so will the sound effects.
        int clip = 0; // the clip index to play after the voiceOver
        if(name == "Card1Animation")
        {
            boolName = "card1";
            card1UI.SetActive(true);
            // audioSource.play(voiceOvers[0]);
            // seconds = voiceOvers[0].length;
        }
        else if(name == "Card2Animation")
        {
            boolName = "card2";
            card2AND3UI.SetActive(true);
            // audioSource.play(voiceOvers[1]);
            // seconds = voiceOvers[1].length;
            clip = 1;
        }
        else if(name == "Card3Animation")
        {
            boolName = "card3";
            card2AND3UI.SetActive(true);
            // audioSource.play(voiceOvers[2]);
            // seconds = voiceOvers[2].length;
            clip = 2;
        }
        else if(name == "Card4Animation")
        {
            boolName = "card4";
            card4UI.SetActive(true);
            // audioSource.play(voiceOvers[3]);
            // seconds = voiceOvers[3].length;
            clip = 3;
        }
        else if(name == "Card5Animation")
        {
            boolName = "card5";
            card5UI.SetActive(true);
            // audioSource.play(voiceOvers[4]);
            // seconds = voiceOvers[4].length;
            clip = 4;
        }
        StartCoroutine(playAnimationAndSound(seconds, boolName, name, clip));
    }

    private IEnumerator playAnimationAndSound(float seconds, string boolName, string name, int clip)
    {
        yield return new WaitForSeconds(seconds);
        GameObject goARObject = arObjects[name];
        Animator animator = goARObject.transform.GetComponent<Animator>();
        animator.SetBool(boolName, true);
        //audioSource.play(soundEffects[clip]);
        if(!shownAnimations.Contains(name))
        {
            shownAnimations.Add(name);
        }
    }

    private void StopAnimations(string name)
    {
        GameObject goARObject = arObjects[name];
        Animator animator = goARObject.transform.GetComponent<Animator>();
        string boolName = "";
        
        if(name == "Card1Animation")
        {
            boolName = "card1";
            card1UI.SetActive(false);
        }
        else if(name == "Card2Animation")
        {
            boolName = "card2";
            card2AND3UI.SetActive(false);
        }
        else if(name == "Card3Animation")
        {
            boolName = "card3";
            card2AND3UI.SetActive(false);
        }
        else if(name == "Card4Animation")
        {
            boolName = "card4";
            card4UI.SetActive(false);
        }
        else if(name == "Card5Animation")
        {
            boolName = "card5";
            card5UI.SetActive(false);
        }
        animator.SetBool(boolName, false);
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

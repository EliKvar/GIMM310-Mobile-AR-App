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
    public AudioSource voiceOverSource;
    public AudioSource soundEffectSource;
    public AudioClip[] soundEffects;
    public AudioClip[] voiceOvers;
    public GameObject[] arObjectsToPlace;
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    private Vector3 scaleFactor = new Vector3(.3f, .3f, .3f);
    private List<string> shownAnimations = new List<string>();
    private string currentCard = "";
    public GameObject card1UI;
    public GameObject card2UI;
    public GameObject card3UI;
    public GameObject card4UI;
    public GameObject card5UI;

    public float targetTime = 3.0f;
       
    void Awake()
    {
        //Initializing beginning card UI states. This is not neccesary but may help prevent unexpected issues
        card1UI.SetActive(false);
        card2UI.SetActive(false);
        card3UI.SetActive(false);
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
        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
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
            if(!soundEffectSource.isPlaying)
            {
                if(currentCard != name)
                {
                    voiceOverSource.PlayOneShot(voiceOvers[0]);
                    currentCard = name;
                }
                seconds = 3f;
            }
        }
        else if(name == "Card2Animation")
        {
            boolName = "card2";
            card2UI.SetActive(true);
            if(!soundEffectSource.isPlaying && currentCard != name)
            {
                voiceOverSource.PlayOneShot(voiceOvers[1]);
                currentCard = name;
                seconds = 9f;
            }
            clip = 1;
        }
        else if(name == "Card3Animation")
        {
            boolName = "card3";
            card3UI.SetActive(true);
            if(!soundEffectSource.isPlaying && currentCard != name)
            {
                voiceOverSource.PlayOneShot(voiceOvers[2]);
                currentCard = name;
                seconds = 8f;
            }
            clip = 2;
        }
        else if(name == "Card4Animation")
        {
            boolName = "card4";
            card4UI.SetActive(true);
            if(!soundEffectSource.isPlaying && currentCard != name)
            {
                voiceOverSource.PlayOneShot(voiceOvers[3]);
                currentCard = name;
                seconds = 5f;
            }
            clip = 3;
        }
        else if(name == "Card5Animation")
        {
            boolName = "card5";
            card5UI.SetActive(true);
            if(!soundEffectSource.isPlaying  && currentCard != name)
            {
                voiceOverSource.PlayOneShot(voiceOvers[4]);
                currentCard = name;
                seconds = 5f;
            }
            clip = 4;
        }
        if(currentCard == name)
        {
            StartCoroutine(playAnimationAndSound(seconds, boolName, name, clip));
        }
    }

    private IEnumerator playAnimationAndSound(float seconds, string boolName, string name, int clip)
    {
        yield return new WaitForSeconds(seconds);
        GameObject goARObject = arObjects[name];
        Animator animator = goARObject.transform.GetComponent<Animator>();
        animator.SetBool(boolName, true);
        soundEffectSource.clip = soundEffects[clip];
        soundEffectSource.Play();
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
            card2UI.SetActive(false);
        }
        else if(name == "Card3Animation")
        {
            boolName = "card3";
            card3UI.SetActive(false);
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
        soundEffectSource.Pause();
    }
}

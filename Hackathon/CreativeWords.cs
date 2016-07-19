using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Collections;
using SimpleJSON;


public class CreativeWords : MonoBehaviour
{

    // Private Variables
    //  private DictationRecognizer dictationRecognizer = new DictationRecognizer();

    private DictationRecognizer dictationRecognizer;
    private string[] activationWords = new string[] { "inspire", "spire", "ire", "fire", "show", "display", "tell" };     //to start the speech...
    private string inspiredWord = "";
    public GameObject HoveringDisplay;          //IN YOUR FACEEE
    public GameObject GeneratedWordsPrefab;
    public GameObject[] screens;
    private ArrayList AssociatedWords = new ArrayList();


    // GetThesaurus Variables
    public static int wordIndex;    //index that gets incrimented
    public ArrayList words = new ArrayList();       //all associated words

    // GetImage Variables
    public Transform me; // GameObject that all images are pointing towards
    private string imageUrl = ""; //"https://i.ytimg.com/vi/bhPCvwHnk_Q/maxresdefault.jpg";
    private WWW www;
    private static bool once = true;
    private int heightForImage = 20;

    private int imagesIndex = 0;

    string[] statueUrls = new string[8] {
        "https://secure.img2.wfrcdn.com/lf/49/hash/36987/26634259/1/Chandler-Figurine-VKGL1831.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/37700/23422058/1/Zaltbommel-Polystone-Giraffe-Statue-BNGL1649.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/10626/27513476/1/3-Piece-Pop-Art-Oversized-Jacks-Cast-Iron-Statue-Set-MH20540.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/10085/23855388/1/Primitive-African-Tribal-Mask-PV-PFS.jpg",
        "https://secure.img2.wfrcdn.com/lf/49/hash/10626/5320751/1/Design-Toscano-T-Rex-Dinosaur-Garden-Statue-EU7570.jpg",
        "https://secure.img2.wfrcdn.com/lf/49/hash/16282/22535757/1/Danseuse%2BStatuette.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/13844/13566744/1/3-Piece%2BPerching%2BOwl%2BStatuette%2BSet.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/10626/7112182/1/Design-Toscano-Giant-Male-Gorilla-Statue-NE110088.jpg"
    };

    string[] chairUrls = new string[8] {
        "https://secure.img2.wfrcdn.com/lf/49/hash/36985/27731813/1/Alica-Arm-Chair-ALCT1883.jpg",
        "https://secure.img2.wfrcdn.com/lf/49/hash/37701/29921971/1/Rocking-Chair-VVRO3484.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/36987/27695848/1/Wade-Arm-Chair-VKGL1851.jpg",
        "https://secure.img2.wfrcdn.com/lf/49/hash/34834/23431005/1/Geo%2BBrights%2BParsons%2BChair.jpg",
        "https://secure.img2.wfrcdn.com/lf/49/hash/37311/29034521/1/Savannah-Cowhide-Club-Chair-LOON4511.jpg",
        "https://secure.img2.wfrcdn.com/lf/49/hash/39299/28557963/1/Rattan-Swivel-Rocker-Chair-BAYI1461.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/30593/16049350/1/Arm-Chair-ZIPC1184.jpg",
        "https://secure.img1.wfrcdn.com/lf/49/hash/30973/27915962/1/Davis-Eucalyptus-Rocking-Chair-THRE3089.jpg"
    };

    public Texture2D defaultTexture;


    /******************************
    ***** Thesaurus Functions *****
    *******************************/
    void Start()
    {
    dictationRecognizer = new DictationRecognizer();
    screens = GameObject.FindGameObjectsWithTag("cube");

        for (int  i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(true);
        }

        StartDictationRecognizer();
        //  CreateImages();
    }
    
    // This function does all the behind the scenes stuff for grabbing synonyms from the Thesaurus API
    void GetWords()
    {
        Debug.Log("Success");
        string url = "http://words.bighugelabs.com/api/2/7c99b7f006863cf39101e606f4d8a0c5/" + inspiredWord + "/json";

        WWW www = new WWW(url);
        while (!www.isDone) { }
        
        Debug.Log(www.text);

        if (www.error == null) { Debug.Log(www.text); }
        else { Debug.Log(www.error); }

        JSONNode noun;
        JSONNode verbs;
        JSONNode adjectives;

        try
        {
            var json = JSON.Parse(www.text);
            if (json["noun"] != null)
            {
                noun = json["noun"]["syn"];

                for (int i = 0; i < noun.Count; i++)
                {
                    words.Add(noun[i]);
                }
            }

            if (json["verbs"] != null)
            {
                verbs = json["verb"]["syn"];

                for (int i = 0; i < verbs.Count; i++)
                {
                    words.Add(verbs[i]);
                }
            }

            if (json["adjectives"] != null)
            {
                adjectives = json["adjective"]["syn"];

                for (int i = 0; i < adjectives.Count; i++)
                {
                    words.Add(adjectives[i]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }



    /******************************
    ***** Dictation Functions *****
    *******************************/

    // This event is fired continuously while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
    private void DictationRecognizer_DictationHypothesis(string text)       //as you are talking it pulls text
    {
        //HoveringDisplay.GetComponent<TextMesh>().text = text; 
    }


    // This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log(text);
        bool inspired = false;

        for (int i = 0; i < activationWords.Length; i++)
        {
            if (text.Contains(activationWords[i]))
            {
                inspired = true;
            }
        }

        if (inspired)
        {
            DestroyWords();

            string[] inspireWords = text.Split(' '); // Splits the sentences into an array of words
            inspiredWord = inspireWords[inspireWords.Length - 1];
        //    HoveringDisplay.GetComponent<TextMesh>().text = "Showing Inspiration For:_" + inspiredWord;     //this will be inspired word
            HoveringDisplay.GetComponent<TextMesh>().text = "Showing Inspiration...";
            Debug.Log(inspireWords[inspireWords.Length - 1]);
            GetWords();

            ActivateWords();
            // Destroy(HoveringDisplay, 2);
            StartCoroutine(DelayForHovering(4));        //this is the delay for the words to disappear
            CreateImages();
            // AssociatedWords[i].GetComponent<TextMesh>().text = words[wordIndex].ToString();
        }

    }

    IEnumerator DelayForHovering(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HoveringDisplay.GetComponent<TextMesh>().text = "";
    }

    private void DestroyWords()
    {
        for (int i = 0; i < AssociatedWords.Count; i++)
        {
            Destroy((GameObject)AssociatedWords[i], 1);
        }
        AssociatedWords.Clear();        //game objects
        words.Clear();                  //actual words....
    }

    private void ActivateWords()
    {
        for (int i = 0; i < words.Count; i++)
        {
            float x = UnityEngine.Random.Range(-20, 20);
            float y = UnityEngine.Random.Range(10, 25);
            float z = UnityEngine.Random.Range(20, 25);
            Vector3 pos = new Vector3(x, y, z);

            GameObject newWord = Instantiate(GeneratedWordsPrefab, pos, Quaternion.identity) as GameObject;

            newWord.GetComponent<TextMesh>().text = words[i].ToString();
            newWord.GetComponent<TextMesh>().fontSize = 200;
            //newWord.GetComponent<TextMesh>().font = a;
            Color ramdomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            Debug.Log(randomColor);
       //     newWord.GetComponent<TextMesh>().color = randomColor;
            newWord.SetActive(true);
            AssociatedWords.Add(newWord);
        }

        Debug.Log(AssociatedWords.Count);
    }


    // This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Stopped " + cause);

        if (cause == DictationCompletionCause.TimeoutExceeded)
        {
            // Try doing something here to make the dication thing start back up again
        }

    }


    // This event is fired when an error occurs.
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("The Dictation Recognized ended in error: " + error + " and results: " + hresult);
    }

    private void StartDictationRecognizer()
    {
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.Start();
    }

   

    int index = 0;
    private Color randomColor;

    // Update is called once per frame
    void Update()
    {
    }


    /************************
    ***** Display Image *****
    *************************/

    void CreateImages()
    {

        for (int i = 0; i < screens.Length; i++)
        {
            switch (inspiredWord)
            {
                case "statue":
                case "statues":
                case "fun":
                    www = new WWW(statueUrls[i]);
                    break;
                case "chair":
                case "chairs":
                case "chance":
                case "here":
                case "cherokee":
                case "share":
                case "jerry":
                    www = new WWW(chairUrls[i]);
 
                    break;
                default:
                    www = new WWW(statueUrls[i]);                    
                    break;
            }
            while (!www.isDone) { }
            screens[i].SetActive(true);
            screens[i].GetComponent<Renderer>().material.mainTexture = www.texture;
       //   screens[i].GetComponent<Renderer>().material.mainTexture = www.texture;

            /* This code is for if you want to dynamically create images at runtime by Jason *

            //  GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //plane.GetComponent<Renderer>().material.mainTexture = www.texture;
            //Renderer renderer = plane.GetComponent<Renderer>();
            // renderer.material.mainTexture = www.texture;
            //Transform transform = plane.GetComponent<Transform>();
            /*
            Vector3 inside;
            float circle = UnityEngine.Random.value;
            Vector2 hold = new Vector2(Mathf.Sin(circle * 2 * Mathf.PI), Mathf.Cos(circle * 2 * Mathf.PI));
            hold *= 35;
            inside.x = hold.x;
            inside.z = hold.y;
            inside.y = UnityEngine.Random.value * 18;
            transform.position = inside;
            transform.LookAt(me);
            transform.Rotate(new Vector3(90, 0, 0));
            //   double dimensionx = UnityEngine.Random.Range(-30, 30);
            //       double xsquared = dimensionx * Math.Abs(dimensionx);
            //     double dimensiony = Math.Sqrt(900.0f - xsquared);
            // double dimensionz = UnityEngine.Random.Range(0, 30);
            //  Vector3 inside = new Vector3((float)dimensionx, (float)dimensiony, (float)dimensionz);
            */
        }

    }
}


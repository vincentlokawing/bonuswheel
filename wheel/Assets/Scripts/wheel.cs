using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class wheel : MonoBehaviour
{
    [SerializeField] private Button uiButton;
    Vector3 newRotation = new Vector3(0f, 0f, 0f);
    float[] speedState = { 0.5f, 0.25f, 0.1f };
    float speed = 0f;
    public int rand = 0;
    public int value = 0;
    bool startSpin = false;
    int result = 0;
    float minAngle = 0.0f;
    float maxAngle = 0.0f;
    List<int> dropChance         = new List<int> { 45, 45, 45, 45, 45, 45, 45, 45 };
    List<int> defaultDropChance  = new List<int> { 20, 10, 10, 10,  5, 20,  5, 20 };
    List<int> updatedDropChance  = new List<int> { 45, 45, 45, 45, 45, 45, 45, 45 };
    float currentAngle;
    float stateTime;
    float deltaTime;
    bool stopSpin = false;
    string txt;
    string uploadTxt;
    public string userName;
    string[] wheelChance;

    public List<int> countWheelChanceList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
    public int maxWheelPlayed = 0;
    // Start is called before the first frame update
    void Start()
    {
        userName = "testuser01";
        uiButton.interactable = false;
        StartCoroutine(GetDropChance());
        StartCoroutine(GetText());
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpin)
        {
            SpinWheel();
        }
    }

    public void OnClickedSpinButton()
    {
        //get random
        rand = Random.Range(1, 101);
        maxWheelPlayed += 1;
        /*
        1. Life 30 min  20% 1-20
        2. Brush 3x     10% 20-30
        3. Gems 35      10% 31-40
        4. Hammer 3x    10% 41-50
        5. Coins 750    5%  51-55
        6. Brush 1x     20% 56-75
        7. Gems 75      5%  76-80
        8. Hammer 1x    20% 81-100
        */
        
        if (rand <= dropChance[0])
        { result = 1; txt = "Life 30 min."; countWheelChanceList[0] += 1; }
        else if (rand > dropChance[0] && rand <= (dropChance[0] + dropChance[1]))
        { result = 2; txt = "Brush 3X."; countWheelChanceList[1] += 1; }
        else if (rand > (dropChance[0] + dropChance[1]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2]))
        { result = 3; txt = "Gems 35."; countWheelChanceList[2] += 1; }
        else if (rand > (dropChance[0] + dropChance[1] + dropChance[2]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3]))
        { result = 4; txt = "Hammer 3X."; countWheelChanceList[3] += 1; }
        else if (rand > (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4]))
        { result = 5; txt = "Coins 750."; countWheelChanceList[4] += 1; }
        else if (rand > (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4] + dropChance[5]))
        { result = 6; txt = "Brush 1X."; countWheelChanceList[5] += 1; }
        else if (rand > (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4] + dropChance[5]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4] + dropChance[5] + dropChance[6]))
        {  result = 7; txt = "Gems 75."; countWheelChanceList[6] += 1; }
        else
        { result = 8; txt = "Hammer 1X."; countWheelChanceList[7] += 1; }

        if (result != 8)
        {
            minAngle = 45.0f * (float)(result - 1);
            maxAngle = 45.0f * (float)result;
        }
        else 
        {
            minAngle = 45.0f * (float)(result - 1);
            maxAngle = 360.0f;
        }

        stateTime = Random.Range(3f,4f);
        speed = speedState[0];
        newRotation = new Vector3(0, 0, speed);
        uiButton.interactable = false;
        startSpin = true;
        stopSpin = false;
        StartCoroutine(FirstCoroutine());

        uploadTxt = maxWheelPlayed + ". 1: " + countWheelChanceList[0] * 100 / maxWheelPlayed + "% " + 
                                                             ". 2: " + countWheelChanceList[1] * 100 / maxWheelPlayed + "% " +
                                                             ". 3: " + countWheelChanceList[2] * 100 / maxWheelPlayed + "% " +
                                                             ". 4: " + countWheelChanceList[3] * 100 / maxWheelPlayed + "% " +
                                                             ". 5: " + countWheelChanceList[4] * 100 / maxWheelPlayed + "% " +
                                                             ". 6: " + countWheelChanceList[5] * 100 / maxWheelPlayed + "% " +
                                                             ". 7: " + countWheelChanceList[6] * 100 / maxWheelPlayed + "% " +
                                                             ". 8: " + countWheelChanceList[7] * 100 / maxWheelPlayed + "% ";
        Debug.Log(uploadTxt);
        Debug.Log("Random: " + rand + ".Result: " + txt);
        StartCoroutine(Upload(uploadTxt));
    }

    IEnumerator FirstCoroutine()
    {
        while (deltaTime < stateTime)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }
        deltaTime = 0f;
        speed = speedState[1];
        newRotation = new Vector3(0, 0, speed);
        stateTime = 1.0f;
        while (deltaTime < stateTime && currentAngle > minAngle-40.0f && currentAngle < maxAngle- 40.0f)
        {
            currentAngle = transform.eulerAngles.z;
            deltaTime += Time.deltaTime;
            yield return null;
        }
        stopSpin = true;
    }

    void SpinWheel()
    {
        value = (int)this.transform.rotation.z;
        if (value < rand)
        {
            transform.eulerAngles += newRotation;
        }

        currentAngle = transform.eulerAngles.z;
        //Debug.Log(currentAngle + "Result: ");
        if (stopSpin && currentAngle > minAngle && currentAngle < maxAngle)
        {
            speed = speedState[2];
            newRotation = new Vector3(0, 0, speed);
            //bug.Log((minAngle + maxAngle) / 2);
            if (currentAngle > ((minAngle + maxAngle) / 2))
            {
                startSpin = false;
                uiButton.interactable = true;

            }
        }
    }

    void SetDropRate(List<int> _List)
    {
        for(int i=0; i< dropChance.Count; i++)
            dropChance[i] = _List[i];
            Debug.Log("1: Life 30 min: " + dropChance[0] +
                        "% 2: Brush 3X: " + dropChance[1] +
                        "% 3: Gems 35: " + dropChance[2] +
                        "% 4: Hammer 3X: " + dropChance[3] +
                        "% 5: Coins 750: " + dropChance[4] +
                        "% 6: Brush 1X: " + dropChance[5] +
                        "% 7: Gems 75: " + dropChance[6] +
                        "% 8: Hammer 1X: " + dropChance[7]);
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/UnityBackendWheel/wheel.php")) 
        {
            yield return www.Send();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Show results as text
                Debug.Log("Connection Established!");

                //Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
                //Debug.Log(www.downloadHandler.text);
            }
        }
    }
    IEnumerator GetDropChance()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/UnityBackendWheel/wheeldropchance.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                SetDropRate(defaultDropChance);
            }
            else
            {
                //Show results as text
                //Debug.Log("Connection Established!");

                //Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
                //Debug.Log(www.downloadHandler.text);
                wheelChance = www.downloadHandler.text.Split(char.Parse(" "));

                for (int i=3; i < wheelChance.Length; i++)
                {
                    if (wheelChance[i] != null)
                        updatedDropChance[i - 3] = System.Convert.ToInt32(wheelChance[i]);
                }
                if(wheelChance != null)
                    SetDropRate(updatedDropChance);
                else
                    SetDropRate(defaultDropChance);
                uiButton.interactable = true;
            }

            ////////////////////////////////////////////////////////////////////////
            /////For testing purpose, uiButton would turn on even without connection
            uiButton.interactable = true;
            ////////////////////////////////////////////////////////////////////////
        }
    }

    IEnumerator Upload(string _wheeldata)
    {
        WWWForm form = new WWWForm();
        form.AddField("userLogin", userName);
        form.AddField("userWheeldata", _wheeldata);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackendWheel/wheel.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
               // Debug.Log(www.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Debug
    /// </summary>
    /// 
    public List<int> debugCountWheelChanceList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
    public int debugMaxWheelPlayed = 0;

    public void OnClickedDebugButton()
    {
        for (int i = 0; i < debugCountWheelChanceList.Count; i++)
        {
            debugCountWheelChanceList[i] = 0;
        }
        debugMaxWheelPlayed = 0;
        for (int i = 0; i < 1000; i++)
        {
            rand = Random.Range(1, 101);
            debugMaxWheelPlayed += 1;
            if (rand <= dropChance[0])
            { result = 1; txt = "Life 30 min."; debugCountWheelChanceList[0] += 1; }
            else if (rand > dropChance[0] && rand <= (dropChance[0] + dropChance[1]))
            { result = 2; txt = "Brush 3X."; debugCountWheelChanceList[1] += 1; }
            else if (rand > (dropChance[0] + dropChance[1]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2]))
            { result = 3; txt = "Gems 35."; debugCountWheelChanceList[2] += 1; }
            else if (rand > (dropChance[0] + dropChance[1] + dropChance[2]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3]))
            { result = 4; txt = "Hammer 3X."; debugCountWheelChanceList[3] += 1; }
            else if (rand > (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4]))
            { result = 5; txt = "Coins 750."; debugCountWheelChanceList[4] += 1; }
            else if (rand > (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4] + dropChance[5]))
            { result = 6; txt = "Brush 1X."; debugCountWheelChanceList[5] += 1; }
            else if (rand > (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4] + dropChance[5]) && rand <= (dropChance[0] + dropChance[1] + dropChance[2] + dropChance[3] + dropChance[4] + dropChance[5] + dropChance[6]))
            { result = 7; txt = "Gems 75."; debugCountWheelChanceList[6] += 1; }
            else
            { result = 8; txt = "Hammer 1X."; debugCountWheelChanceList[7] += 1; }
        }
        Debug.Log(debugMaxWheelPlayed + ". 1: " + debugCountWheelChanceList[0] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 2: " + debugCountWheelChanceList[1] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 3: " + debugCountWheelChanceList[2] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 4: " + debugCountWheelChanceList[3] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 5: " + debugCountWheelChanceList[4] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 6: " + debugCountWheelChanceList[5] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 7: " + debugCountWheelChanceList[6] * 100 / debugMaxWheelPlayed + "% " +
                                                             ". 8: " + debugCountWheelChanceList[7] * 100 / debugMaxWheelPlayed + "% ");
    }
}

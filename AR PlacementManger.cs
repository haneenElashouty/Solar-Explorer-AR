
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using TMPro; // Added for UI text

[RequireComponent(typeof(ARAnchorManager))]
public class ARPlacementManager : MonoBehaviour
{
   

    public GameObject solarSystem; 
    public GameObject reticle;     
    public ARRaycastManager raycastManager;

    private float touchStartTime;
    private bool isLongPressCalculated = false;
    public float longPressDuration = 0.5f; // Adjust this in Inspector (0.5s is standard)

    [Header("Onboarding UI")]
    public GameObject onboardingPanel;
    
    [Header("UI Elements")]
    public GameObject infoPanel;      // Drag your panel here
    public TextMeshProUGUI titleText; // Drag title text here
    public TextMeshProUGUI descriptionText; // Drag description text here

    private ARAnchorManager anchorManager;
    private ARAnchor currentAnchor;

    // Tap Timing for Double-Click
    private float lastTapTime;
    private float doubleTapThreshold = 0.3f; 

    // Zoom & UX Logic
    public float currentScale = 0.05f; 
    public GameObject planetLabels; 

    void Awake()
    {
        anchorManager = GetComponent<ARAnchorManager>();
        
        if (onboardingPanel != null) onboardingPanel.SetActive(true);// Show onboarding at start, hide everything else
        if (solarSystem != null) solarSystem.SetActive(false);
        if (planetLabels != null) planetLabels.SetActive(false); 
        if (infoPanel != null) infoPanel.SetActive(false); // Ensure panel starts hidden
    }

    void Update()
    {
        HandlePlacement();
        HandlePinchToZoom(); 
    }

  void HandlePlacement()
{
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    
    if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
    {
        Pose hitPose = hits[0].pose;
        reticle.transform.position = hitPose.position;
        reticle.transform.rotation = hitPose.rotation;
        reticle.SetActive(!solarSystem.activeSelf);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Start Timer
            if (touch.phase == TouchPhase.Began)
            {
                touchStartTime = Time.time;
                isLongPressCalculated = false;
            }

            // CHECK DURING HOLD: Long Press for Panel
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                if (solarSystem.activeSelf && !isLongPressCalculated && (Time.time - touchStartTime) > longPressDuration)
                {
                    if (TryTapPlanet()) 
                    {
                        isLongPressCalculated = true; 
                    }
                }
            }

            // CHECK ON RELEASE: Single Tap (Placement) or Double Tap (Reset)
            if (touch.phase == TouchPhase.Ended)
            {
                float touchDuration = Time.time - touchStartTime;

                // 1. Double Tap Check (Reset)
                if (touchDuration < longPressDuration && (Time.time - lastTapTime) < doubleTapThreshold)
                {
                    ResetPlacement();
                }
                // 2. Single Tap Check (Placement)
                else if (touchDuration < longPressDuration && !solarSystem.activeSelf)
                {
                    PlaceSolarWithAnchor(hitPose);
                }
                // 3. Quick tap on empty space closes panel
                else if (touchDuration < longPressDuration && solarSystem.activeSelf && !isLongPressCalculated)
                {
                    CloseInfoPanel();
                }

                lastTapTime = Time.time;
            }
        }
    }
    else
    {
        reticle.SetActive(false);
    }
}

    // --- NEW PANEL FUNCTIONS ---

    bool TryTapPlanet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Planet"))
            {
                UpdateUI(hit.collider.gameObject.name);
                return true;
            }
        }
        return false;
    }

    void UpdateUI(string name)
    {
        infoPanel.SetActive(true);
        string cleanName = name.Replace("-label", "").Replace("(Clone)", "").Trim();
        titleText.text = cleanName;

        if (cleanName.Contains("Sun"))
    {
        descriptionText.text = "<b>ID:</b> The central star of the Solar System\n\n" +
                               "<b>Fact:</b> A massive ball of plasma producing most of the system's light and heat\n\n" +
                               "<b>Context:</b> The primary energy source for life on Earth.";
    }
    else if (cleanName.Contains("Mercury"))
    {
        descriptionText.text = "<b>ID:</b> The closest planet to the Sun\n\n" +
                               "<b>Fact:</b> Smallest major planet with a heavily cratered, Moon-like surface\n\n" +
                               "<b>Context:</b> Used to test extreme heat-shielding models for spacecraft.";
    }
    else if (cleanName.Contains("Venus"))
    {
        descriptionText.text = "<b>ID:</b> Earth’s “evil twin” planet\n\n" +
                               "<b>Fact:</b> Hottest planet (460°C) with a crushing greenhouse atmosphere\n\n" +
                               "<b>Context:</b> Key for studying how Earth-like worlds can become uninhabitable.";
    }
    else if (cleanName.Contains("Earth"))
    {
        descriptionText.text = "<b>ID:</b> The Blue Planet\n\n" +
                               "<b>Fact:</b> Only planet with stable liquid-water oceans and a breathable atmosphere\n\n" +
                               "<b>Context:</b> The only confirmed home of life and base for space exploration.";
    }
    else if (cleanName.Contains("Mars"))
    {
        descriptionText.text = "<b>ID:</b> The Red Planet\n\n" +
                               "<b>Fact:</b> Home to the largest volcano (Olympus Mons) in the Solar System\n\n" +
                               "<b>Context:</b> The most likely candidate for future human settlement.";
    }
    else if (cleanName.Contains("Jupiter"))
    {
        descriptionText.text = "<b>ID:</b> The gas-giant king\n\n" +
                               "<b>Fact:</b> Largest planet, more massive than all others combined\n\n" +
                               "<b>Context:</b> A major gravity-assist “slingshot” target for deep-space probes.";
    }
    else if (cleanName.Contains("Saturn"))
    {
        descriptionText.text = "<b>ID:</b> The ringed jewel of the Solar System\n\n" +
                               "<b>Fact:</b> Famous for its bright, icy ring system made of countless particles\n\n" +
                               "<b>Context:</b> Its moons like Enceladus are possible habitats for life.";
    }
    else if (cleanName.Contains("Uranus"))
    {
        descriptionText.text = "<b>ID:</b> The sideways ice giant\n\n" +
                               "<b>Fact:</b> Rotates almost on its side with an atmosphere rich in methane\n\n" +
                               "<b>Context:</b> One of the least-explored planets, raising interest for future missions.";
    }
    else if (cleanName.Contains("Neptune"))
    {
        descriptionText.text = "<b>ID:</b> The distant blue giant\n\n" +
                               "<b>Fact:</b> The farthest planet from the Sun with the fastest recorded winds\n\n" +
                               "<b>Context:</b> Helps scientists understand how planets form at the system's edge.";
    }
    else 
    {
        descriptionText.text = "A magnificent celestial body in space!";
    }
    }

    public void CloseInfoPanel()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }

    // --- END NEW PANEL FUNCTIONS ---

    void PlaceSolarWithAnchor(Pose pose)
{
    // 1. Position your solar system at the hit point first
    solarSystem.transform.position = pose.position;
    solarSystem.transform.rotation = pose.rotation;
    solarSystem.transform.localScale = Vector3.one * currentScale; 

    // 2. Add the Anchor component directly to the solar system object
    // This replaces the old: currentAnchor = anchorManager.AddAnchor(pose);
    if (solarSystem.GetComponent<ARAnchor>() == null)
    {
        currentAnchor = solarSystem.AddComponent<ARAnchor>();
    }

    // 3. Make sure it's active
    solarSystem.SetActive(true);
}

    void ResetPlacement()
    {
       ARAnchor anchor = solarSystem.GetComponent<ARAnchor>();
         if (anchor != null)
        {
             Destroy(anchor); 
        }
        solarSystem.SetActive(false);
        solarSystem.transform.parent = null;
        CloseInfoPanel(); // Hide panel on reset
    }

    void HandlePinchToZoom()
    {
        if (Input.touchCount == 2 && solarSystem.activeSelf)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevMag = (t0Prev - t1Prev).magnitude;
            float currentMag = (t0.position - t1.position).magnitude;
            float diff = currentMag - prevMag;

            currentScale += diff * 0.0001f; 
            currentScale = Mathf.Clamp(currentScale, 0.01f, 2.0f); 

            solarSystem.transform.localScale = Vector3.one * currentScale;

            if (planetLabels != null)
            {
                planetLabels.SetActive(currentScale > 0.15f); 
            }
        }
    }
    public void StartAR()
    {
        if (onboardingPanel != null)
        {
           onboardingPanel.SetActive(false);
        }
    }
    
}

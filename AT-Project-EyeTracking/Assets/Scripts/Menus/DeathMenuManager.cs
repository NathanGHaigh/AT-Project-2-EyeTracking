using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour
{
    DeathCodeStore deathCodeStore;

    int deathID;

    public Image menuPanel;

    public Button backButton;

    public Sprite scp096Image;
    public Sprite scp173Image;
    public Sprite scp689Image;

    public TextMeshProUGUI deathText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI deathInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        deathCodeStore = FindAnyObjectByType<DeathCodeStore>();
        deathID = deathCodeStore.DeathID;
        menuPanel.GetComponent<Image>().sprite = deathID switch
        {
            1 => scp689Image,
            2 => scp173Image,
            3 => scp096Image,
            _ => null
        };

        deathText.text = deathID switch
        {
            1 => "Cause: SCP-689",
            2 => "Cause: SCP-173",
            3 => "Cause: SCP-096",
            _ => "You died."
        };

        classText.text = deathID switch
        {
            1 => "Class: Keter",
            2 => "Class: Euclid",
            3 => "Class: Euclid",
            _ => "Unknown"
        };

        deathInfo.text = deathID switch
        {
            1 => "The individual was found dead with cause of death being rapid cardiac arrest, presumed to have viewed and lost focus of SCP 689. SCP 689 is missing post containment breach at site [REDACTED]. Power loss at site [REDACTED] led to its breach, " +
            "with multiple D-Class and Science personal viewing SCP 689 during a routine observation transfer. Only 4 bodies were discovered during clean up with multiple observers of SCP 689 missing.",

            2 => "Individual was found dead, cause deemed to be a neck fracture caused by SCP 173. SCP 173 was responsible for [REDACTED] number of deaths during the containment breach at site [REDACTED]. " +
            "Re-Containment of SCP 173 was achieved by MTF Unit [DATA EXPUNGED] during the retake operation of site [REDACTED]. " +
            "Transfer to a new facility is currently on-going, expected arrival at [REDACTED].",

            3 => "Blood traces found around [REDACTED] returned DNA samples of scientist DR [REDACTED]. Presumed to of been killed by SCP 096 and consumed post viewing its face. SCP 096 was found crying in the corner of containment cell 3, " +
            "with an estimated victim count of 12. Its breach of containment was discovered to be the cause of the facility power loss. " +
            "An image of SCP 096's face was found in [REDACTED] and assumed to be the cause of aggravation. ",
            _ => "[REDACTED]"
        };
        backButton.onClick.AddListener(() =>
        {
            Destroy(deathCodeStore.gameObject);
            LoadMainMenu();

        });
    }

    void LoadMainMenu()
    {        SceneManager.LoadScene("Menu");

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectivesUI : MonoBehaviour
{
    public static ObjectivesUI Instance { get; private set; }

    [SerializeField] private Transform objectiveUITemplate;
    [SerializeField] private Transform objectivesUIContainer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("DeliveryManager is singleton but tried to be set more than once.");
        }

        Instance = this;
    }

    public void SetObjectivesUI(List<string> objectives)
    {
        // clean the ui
        foreach (Transform child in objectivesUIContainer)
        {
            Destroy(child.gameObject);
        }

        // create all ui elements again
        foreach (var objectiveStr in objectives)
        {
            var recipeUI = Instantiate(objectiveUITemplate, objectivesUIContainer);
            recipeUI.GetComponent<ObjectiveTemplateUI>().SetText(objectiveStr);
        }
    }

}

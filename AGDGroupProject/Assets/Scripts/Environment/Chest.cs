using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    public string chestID; // Unieke ID voor deze kist
    public Sprite closedSprite;
    public Sprite openedSprite;
    public float interactionRadius = 2f;

    [Header("UI Prompt")]
    public GameObject promptUI; // "Press E to open chest"-prompt

    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;
    private GiveMultipleItems itemGiver;
    private Transform playerTransform;

    // Sessie-geheugen om geopende kisten te onthouden
    private static HashSet<string> openedChests = new HashSet<string>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemGiver = GetComponent<GiveMultipleItems>();

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        // Zet sprite naar geopend als deze kist al geopend is in deze sessie
        if (openedChests.Contains(chestID))
        {
            isOpened = true;
            spriteRenderer.sprite = openedSprite;
        }
        else
        {
            spriteRenderer.sprite = closedSprite;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        bool playerInRange = distance <= interactionRadius;

        if (!isOpened)
        {
            if (promptUI != null)
                promptUI.SetActive(playerInRange);

            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TryOpenChest();
            }
        }
        else
        {
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    public void TryOpenChest()
    {
        if (isOpened) return;

        Debug.Log("Opening chest...");
        isOpened = true;
        spriteRenderer.sprite = openedSprite;

        // Voeg toe aan sessie-lijst
        if (!openedChests.Contains(chestID))
        {
            openedChests.Add(chestID);
        }

        if (itemGiver != null)
        {
            Debug.Log("Giving items from chest...");
            itemGiver.GiveAllToPlayer();
        }

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }
}

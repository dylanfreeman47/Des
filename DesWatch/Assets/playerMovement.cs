using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public Interactable focus;
    const string INTERACTABLE_TAG = "Interactable";
    List<Interactable> InteractableObjects = new List<Interactable>();
    public float movementSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    CircleCollider2D pickupCircle;
    public float pickupRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Set pickup radius
        pickupCircle = this.GetComponent<CircleCollider2D>();
        pickupCircle.radius = pickupRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // When player runs into an item
        if (Input.GetButtonDown("Interact") && InteractableObjects.Count > 0) {
            Debug.Log("Focus Set");
            SetFocus(InteractableObjects[InteractableObjects.Count-1]);   
        }
    }
    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(INTERACTABLE_TAG)) {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null && !InteractableObjects.Contains(interactable)) {
                
                InteractableObjects.Add(interactable);
            }
         }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(INTERACTABLE_TAG))
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null)
            {
                InteractableObjects.Remove(interactable);
                if (focus != null)
                    RemoveFocus();
            }
        }
        
            
    }
    void SetFocus(Interactable newFocus) {
        if(newFocus != focus)
        {
            Debug.Log("Object list numebr: " + (InteractableObjects.Count));
            Debug.Log("Focus" + focus.name);
            focus.OnDefocused();
            focus = newFocus;
            InteractableObjects.RemoveAt(InteractableObjects.Count - 1);
        }
        
        newFocus.OnFocused(transform);
    }

    void RemoveFocus() {
        focus = null;
        focus.OnDefocused();
        
    }
    
}

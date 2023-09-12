using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        public GameObject gameObject;
        public Transform transform;
        public Animator animator;
        // Add other game specific systems here

        public virtual void Initialize(GameObject newGameObject) {
            // Fetch all commonly used components
            
            gameObject = newGameObject;
            transform = newGameObject.transform;
            animator = newGameObject.GetComponent<Animator>();
        }
    }
}
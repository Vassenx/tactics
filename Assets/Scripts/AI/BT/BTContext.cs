using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTContext : Context
{
    private Enemy enemyCharacter;
    
    public override void Initialize(GameObject newGameObject)
    {
        // Fetch all commonly used components
            
        enemyCharacter = newGameObject.GetComponent<Enemy>();
    }
}
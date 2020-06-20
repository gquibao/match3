using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Gem firstSelectedGem;
    public Gem secondSelectedGem;

    public void ClearSelection()
    {
        firstSelectedGem = null;
        secondSelectedGem = null;
    }
}

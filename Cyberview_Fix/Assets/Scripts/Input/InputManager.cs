using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buttons{
    Right,
    Left,
    ArmOne,
    ArmTwo,
    Legs,
    Action,
    Crouch,
    Pause
}

public enum Condition{
    GreaterThan,
    LessThan
}

[System.Serializable]
public class InputAxisState {
    public string axisName;
    public float offValue;
    public Buttons button;
    public Condition condition;

    public bool value {
        get{
            var val = Input.GetAxis(axisName);

            switch(condition){
                case Condition.GreaterThan:
                    return val > offValue;
                case Condition.LessThan:
                    return val < offValue;
            }
            return false;
        }
    }
}


public class InputManager : MonoBehaviour
{
    public InputAxisState[] inputs;
    public InputState inputState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var input in inputs){
            inputState.SetButtonValue(input.button, input.value);
        }
    }
}

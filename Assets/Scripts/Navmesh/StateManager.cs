// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Rendering;

// public class States : MonoBehaviour
// {

//     void Update()
//     {
//         RunStateMachine();
//     }

//     private void RunStateMachine()
//     {
//         States nextState = RunCurrentState?.RunCurrentState();
//         if(nextState != null)
//         {
//             SwitchState(nextState);
//         }
//     }

//     private void SwitchState (States nextState)
//     {
//         CurrentState = nextState;
//     }

// }

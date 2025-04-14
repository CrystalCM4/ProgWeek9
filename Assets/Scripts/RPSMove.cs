using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMove", menuName = "RPS/Move")]
public class RPSMove : ScriptableObject
{
    public string moveName;
    public List<RPSMove> beats;
}

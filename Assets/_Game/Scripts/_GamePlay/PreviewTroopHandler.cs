using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yuddham
{
    public class PreviewTroopHandler : MonoBehaviour
    {
        public PlacableTroopController CurrentSelectedTroop { get; private set; }
        public void SetCurrentTroop(PlacableTroopController _currentSelectedTroop)
        {
            CurrentSelectedTroop = _currentSelectedTroop;
        }
    }
}

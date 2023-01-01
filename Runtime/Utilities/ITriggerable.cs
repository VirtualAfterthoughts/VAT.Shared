using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAT.Shared
{
    /// <summary>
    /// A basic interface to receive a request for triggering an action.
    /// </summary>
    public interface ITriggerable {
        void Trigger();
    }
}

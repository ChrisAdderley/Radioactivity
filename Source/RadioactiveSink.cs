// All parts that receive radiation require a RadioactiveSink
// Handles hooking the part into the radiation simulation system
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Radioactivity
{

  public class RadioactiveSink:PartModule
  {
    // The ID of the sink
    [KSPField(isPersistant = true)]
    public string SinkID = "";
    // The Transform to use for raycast calculations
    [KSPField(isPersistant = true)]
    public string SinkTransformName = "";

    [KSPEvent(guiActive = true, guiName = "Toggle Rays")]
    public void ToggleOverlay()
    {
      ShowOverlay = !ShowOverlay;
      if (ShowOverlay)
        Radioactivity.Instance.ShowOverlay(this);
      else
        Radioactivity.Instance.HideOverlay(this);
    }

    // Access the sink transform
    public Transform SinkTransform
    {
      get { return sinkTransform;}
      set { sinkTransform = value;}
    }

    private List<GenericRadiationAbsorber> associatedAbsorbers = new List<GenericRadiationAbsorber>();

    // Registers an abosrber module to read from this sink
    public void RegisterAbsorber(GenericRadiationAbsorber abs)
    {
      associatedAbsorbers.Add(abs);
    }

    // Add radiation to the sink
    public void AddRadiation(float amt)
    {
      foreach (GenericRadiationAbsorber abs in associatedAbsorbers) {
        abs.AddRadiation(amt);
      }
    }

    public override void OnStart()
    {
      // Set up the sink transform, if it doesn't exist use the part root
      SinkTransform = part.FindTransformByName(SinkTransformName);
      if (SinkTransform == null)
      {
        Debug.LogWarning("Couldn't find Source transform, using root transform");
        SinkTransform = part.transform;
      }
      Radioactivity.Instance.RegisterSink(this);
    }

    public void OnDestroy()
    {
      Radioactivity.Instance.UnregisterSink(this);
    }
  }
}

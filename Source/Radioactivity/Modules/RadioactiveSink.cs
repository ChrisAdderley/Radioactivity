// All parts that receive radiation require a RadioactiveSink
// Handles hooking the part into the radiation simulation system
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Radioactivity.Interfaces;

namespace Radioactivity
{

  public class RadioactiveSink:PartModule
  {
    // The ID of the sink
    [KSPField(isPersistant = false)]
    public string SinkID;

    // The Transform to use for raycast calculations
    [KSPField(isPersistant = false)]
    public string SinkTransformName;

    [KSPField(isPersistant = false)]
    public bool ShowOverlay = false;

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

    private Transform sinkTransform;
    private bool registered = false;
    private List<IRadiationAbsorber> associatedAbsorbers = new List<IRadiationAbsorber>();

    // Add radiation to the sink
    public void AddRadiation(float amt)
    {
      foreach (IRadiationAbsorber abs in associatedAbsorbers) {
        abs.AddRadiation(amt);
      }
    }


    public override void OnStart(PartModule.StartState state)
    {
        // Set up the sink transform, if it doesn't exist use the part root
        if (SinkTransformName != String.Empty)
            SinkTransform = part.FindModelTransform(SinkTransformName);
        if (SinkTransform == null)
        {
            Utils.LogWarning("Couldn't find Source transform, using part root transform");
            SinkTransform = part.transform;
        }

        List<IRadiationAbsorber> absorbers = part.FindModulesImplementing<IRadiationAbsorber>();
        foreach (IRadiationAbsorber abs in absorbers)
        {
            if (abs.GetSinkName() == SinkID)
                associatedAbsorbers.Add(abs);
        }

        if (HighLogic.LoadedSceneIsFlight && !registered)
        {
            Radioactivity.Instance.RegisterSink(this);
            registered = true;
        }
    }

    public void OnDestroy()
    {
        if (registered)
        {
            Radioactivity.Instance.UnregisterSink(this);
            registered = false;
        }
        if (HighLogic.LoadedSceneIsFlight)
        {
            Radioactivity.Instance.UnregisterSink(this);
            registered = false;
        }
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Radioactivity.UI
{
    public class UIOverlayWindow: UIWindow
  {
      System.Random random;
      List<UISinkWindow> sinkWindows;
      List<UISourceWindow> sourceWindows;

        public UIOverlayWindow(System.Random randomizer, RadioactivityUI uiHost): base(randomizer, uiHost)
      {
            random = randomizer;
          sinkWindows = new List<UISinkWindow>();
          sourceWindows = new List<UISourceWindow>();
 
            Utils.Log("[UIOverlayWindow]: Initialized");
      }
        /// <summary>
        /// Draw the set of sink and source windows
        /// </summary>
      public void Draw()
      {
          for (int i=0; i < sinkWindows.Count ;i++)
          {
              sinkWindows[i].Draw();
          }
          for (int i=0; i < sourceWindows.Count ;i++)
          {
              sourceWindows[i].Draw();
          }
      }

        /// <summary>
        /// Updates the sink and source window positions
        /// </summary>
      public void Update()
       {
            if (Radioactivity.Instance.RadSim.)
           {
               if (Radioactivity.Instance.RadiationNetworkChanged)
               {

                   Utils.Log("Network Changed, rebuilding UI");
                   UpdateSourceList();
                   UpdateSinkList();
                   Radioactivity.Instance.RadiationNetworkChanged = false;
               }

               for (int i = 0; i < sinkWindows.Count; i++ )
               {
                   sinkWindows[i].UpdatePositions();
               }
               for (int i = 0; i < sourceWindows.Count; i++ )
               {
                 sourceWindows[i].UpdatePositions();
               }
           }
        }

      public void UpdateSinkList()
      {
          Utils.Log("Rebuilding Sink List");
          sinkWindows = new List<UISinkWindow>();
          for (int i = 0; i < Radioactivity.Instance.AllSinks.Count; i++ )
          {
              sinkWindows.Add(new UISinkWindow(Radioactivity.Instance.AllSinks[i], random, host));
          }

          
      }

      void UpdateSourceList()
      {
          sourceWindows = new List<UISourceWindow>();
        // Check for new sinks
        for (int i = 0; i < Radioactivity.Instance.AllSources.Count; i++ )
        {
            sourceWindows.Add(new UISourceWindow(Radioactivity.Instance.AllSources[i], random, host ));
        }
      }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Radioactivity
{

  public static class RadioactivitySettings
  {
    // RAYCAST SETTINGS
    // Maximum distance to raycast for point radiation
    public static float raycastDistance = 2000f;
    // Minimum flux level to propagate along attenuation paths
    public static float fluxCutoff = 0f;
    // Starting default flux when propagating along paths
    public static float defaultRaycastFluxStart = 1.0f;
    // How far the geometry between two radiation link endpoints can change before we need to recalculate it
    public static float maximumPositionDelta = 0.5f;
      // How much the mass through a raypath can change before we need to recalculate it
    public static float maximumMassDelta = 0.05f;

    // DEFAULT PART SETTINGS
    // Default value for the linear attenuation coefficient for parts, in cm2/g
    public static float defaultPartAttenuationCoefficient = 1.5f;
    // Default density for a part
    public static float defaultDensity = 1f;

      // OVERLAY SETTINGS
    public static int overlayRayLayer = 0;
    public static float overlayRayWidthMult = 0.005f;
    public static float overlayRayWidthMin = 0.05f;
    public static float overlayRayWidthMax = 0.5f;
    public static string overlayRayMaterial = "GUI/Text Shader";

      // SIMULATOR SETTINGS
    // do we simulate these?
    public static bool simulatePointRadiation = true;
    public static bool simulateSolarRadiation = false;
    public static bool simulateCosmicRadiation = false;

      // TRACKING SETTINGS
    public static string pluginConfigNodeName = "RadioactivityKerbalTracking";
    public static string kerbalConfigNodeName = "RadioactivityKerbal";

      // DEBUG SETTINGS
      // If on, generates UI debug messages
    public static bool debugUI = true;
      // If on, debug overlays
    public static bool debugOverlay = true;
      // If on, generates debug messages for Source and Sink objects
    public static bool debugSourceSinks = true;
      // If on, debug modules added by Radioactivity
    public static bool debugModules = true;
      // If on, generates network creation/destruction/modification messages
    public static bool debugNetwork = true;
      // If on, generates debug messages when building raycast paths
    public static bool debugRaycasting = true;

     

  }

  // Types of zone for attenuation
  public enum AttenuationType {
    Part, ParameterizedPart, Terrain, Empty
  }
}

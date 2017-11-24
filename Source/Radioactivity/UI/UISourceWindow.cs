using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Radioactivity.UI
{
    public class UISourceWindow
    {
        public RadioactiveSource Source
        {
            get { return source; }
        }

        bool showSourceInfo = false;
        bool showSinkInfo = false;
        bool showWindow = false;
        bool showRays = false;

        int windowID;

        Vector2 iconDims = new Vector2(32f, 32f);
        Vector2 infoBarDims = new Vector2(16, 16);
        Vector2 windowDims = new Vector2(150f, 20f);

        Vector3 worldPosition;
        Vector3 screenPosition;
        Rect windowPosition;
        RadioactiveSource source;

        RadioactivityUI host;

        public UISourceWindow(RadioactiveSource src, System.Random random, RadioactivityUI uiHost)
        {
            source = src;

            host = uiHost;

            windowID = random.Next();
            // Set up screen position
            screenPosition = Camera.main.WorldToScreenPoint(source.part.transform.position);
            windowPosition = new Rect(screenPosition.x + 50f, Screen.height - screenPosition.y + windowDims.y / 2f, windowDims.x, windowDims.y);
        }

        public void UpdatePositions()
        {
            screenPosition = Camera.main.WorldToScreenPoint(source.EmitterTransform.position);
            windowPosition = new Rect(screenPosition.x + iconDims.x / 2 + 5f, Screen.height - screenPosition.y + iconDims.y / 2f, windowDims.x, windowDims.y);
        }

        public void Draw()
        {
            if (showWindow)
                windowPosition = GUILayout.Window(windowID, windowPosition, DrawWindow, "", host.GUIResources.GetStyle("mini_window"), GUILayout.MinHeight(20), GUILayout.ExpandHeight(true));
            if (screenPosition.z > 0f)
                DrawButton();
        }

        internal void DrawButton()
        {
            Rect buttonRect = new Rect(screenPosition.x - iconDims.x / 2f, Screen.height - screenPosition.y - iconDims.y / 2f, iconDims.x, iconDims.y);
            Rect labelRect = new Rect(buttonRect.xMax + 5f, buttonRect.yMin + buttonRect.height / 2 - infoBarDims.y / 2f, 90f, infoBarDims.y);

            GUI.DrawTextureWithTexCoords(buttonRect, host.GUIResources.GetIcon(source.IconID).iconAtlas, host.GUIResources.GetIcon(source.IconID).iconRect);
            GUILayout.BeginArea(labelRect, host.GUIResources.GetStyle("mini_group"));
            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("{0}Sv/s", Utils.ToSI(source.CurrentEmission, "F2")), host.GUIResources.GetStyle("mini_text_body"), GUILayout.MinWidth(60f));

            if (GUILayout.Button("...", host.GUIResources.GetStyle("mini_button"), GUILayout.Width(12), GUILayout.Height(12)))
            {
                showSourceInfo = !showSourceInfo;
                if (showSourceInfo && !showWindow)
                    showWindow = true;
                if (!showSinkInfo && !showSourceInfo)
                    showWindow = false;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        internal void DrawWindow(int WindowID)
        {
            //if (DrawSinkDetails)
            //  DrawSinkDetails();
            if (showSourceInfo)
                DrawSourceDetails();
        }

        internal void DrawSourceDetails()
        {
            GUILayout.BeginVertical(host.GUIResources.GetStyle("mini_group"));

            foreach (var kvp in source.GetEmitterDetails())
            {
                GUILayout.Space(2f);
                GUILayout.BeginHorizontal();
                GUILayout.Label(kvp.Key, host.GUIResources.GetStyle("mini_text_header"));
                GUILayout.Label(kvp.Value, host.GUIResources.GetStyle("mini_text_body"));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        internal void DrawLinks()
        {
            GUILayout.BeginVertical();
            List<RadiationLink> assocLinks = source.GetAssociatedLinks();
            for (int i = 0; i < assocLinks.Count; i++)
            {
                DrawLink(assocLinks[i]);
            }
            GUILayout.EndVertical();
        }

        void DrawLink(RadiationLink lnk)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(host.GUIResources.GetStyle("mini_group"));
            GUILayout.BeginVertical();
            GUILayout.Label("<b>" + lnk.source.SourceID + "->" + lnk.sink.SinkID + "</b>", host.GUIResources.GetStyle("mini_text_header"));
            GUILayout.Label("I: " + lnk.fluxEndScale.ToString(), host.GUIResources.GetStyle("mini_text_body"));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("nZones: " + lnk.ZoneCount.ToString(), host.GUIResources.GetStyle("mini_text_body"));
            GUILayout.Label("nOcclu: " + lnk.OccluderCount.ToString(), host.GUIResources.GetStyle("mini_text_body"));
            GUILayout.EndVertical();
            DrawPathDetails(lnk);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        private void DrawPathDetails(RadiationLink lnk)
        {
            GUILayout.BeginVertical();
            int n = 1;
            foreach (AttenuationZone z in lnk.Path)
            {
                GUILayout.Label(n.ToString() + ". " + z.ToString(), host.GUIResources.GetStyle("mini_text_body"));
                n++;
            }
            GUILayout.EndVertical();
        }
    }

}

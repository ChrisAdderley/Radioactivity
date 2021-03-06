﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radioactivity
{
    internal class KerbalDatabase
    {
        internal Dictionary<string, RadioactivityKerbal> Kerbals;

        internal KerbalDatabase()
        {
            Kerbals = new Dictionary<string, RadioactivityKerbal>();
        }

        public List<RadioactivityKerbal> VesselKerbals(List<ProtoCrewMember> crew)
        {
            List<RadioactivityKerbal> toReturn = new List<RadioactivityKerbal>();
            foreach (var kvp in Kerbals)
            {
                foreach (ProtoCrewMember c in crew)
                {
                    if (kvp.Value.Kerbal == c)
                        toReturn.Add(kvp.Value);
                }
            }
          return toReturn;
        }

        public List<RadioactivityKerbal> NearbyKerbals(List<ProtoCrewMember> crew)
        {
          List<RadioactivityKerbal> toReturn = new List<RadioactivityKerbal>();
          foreach (var kvp in Kerbals)
          {
              foreach (ProtoCrewMember c in crew)
              {
                  if (kvp.Value.Kerbal == c)
                      toReturn.Add(kvp.Value);
              }
          }
          return toReturn;
        }
        public List<RadioactivityKerbal> AllKerbals()
        {
            return new List<RadioactivityKerbal>(Kerbals.Values);
        }
        public List<RadioactivityKerbal> ActiveKerbals()
        {
            List<RadioactivityKerbal> toReturn = new List<RadioactivityKerbal>();
            foreach (var kvp in Kerbals)
            {
                  if (kvp.Value.Kerbal.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
                      toReturn.Add(kvp.Value);
            }
            return toReturn;

        }
        public List<RadioactivityKerbal> KSCKerbals()
        {
          List<RadioactivityKerbal> toReturn = new List<RadioactivityKerbal>();
          foreach (var kvp in Kerbals)
          {
                if (kvp.Value.Kerbal.rosterStatus == ProtoCrewMember.RosterStatus.Available)
                    toReturn.Add(kvp.Value);
          }
          return toReturn;
        }

        public void RemoveKerbal(RadioactivityKerbal k)
        {
          Kerbals.Remove(k.Name);
          Utils.Log(String.Format("Kerbal Database: {0} removed from database", k.Name));
        }

        internal void Load(ConfigNode node)
        {
            Utils.Log("Kerbal Database: Loading...");
            Kerbals.Clear();
            Utils.Log("Kerbal Database: Loading from persistence");
            ConfigNode mNode = node.GetNode(RadioactivitySettings.pluginConfigNodeName);
            ConfigNode[] kNodes = mNode.GetNodes(RadioactivitySettings.kerbalConfigNodeName);
            foreach (ConfigNode kNode in kNodes)
            {

                if (kNode.HasValue("KerbalName"))
                {
                    string idx = kNode.GetValue("KerbalName");
                    Utils.Log(String.Format("Kerbal Database: Loading kerbal {0}", idx));
                    RadioactivityKerbal kerbal = new RadioactivityKerbal(idx);
                    kerbal.Load(kNode, idx);
                    Kerbals[idx] = kerbal;
                }
            }
            Utils.Log("Kerbal Database: Loading from roster");
            var crewList = HighLogic.CurrentGame.CrewRoster.Crew.Concat(HighLogic.CurrentGame.CrewRoster.Applicants).Concat(HighLogic.CurrentGame.CrewRoster.Tourist).Concat(HighLogic.CurrentGame.CrewRoster.Unowned).ToList();
            foreach (ProtoCrewMember crew in crewList)
            {
                if (!Kerbals.ContainsKey(crew.name))
                {
                    Utils.Log(String.Format("Kerbal Database: Loading kerbal {0}", crew.name));
                    RadioactivityKerbal kerbal = new RadioactivityKerbal(crew.name);
                    kerbal.Load(crew);
                    Kerbals[crew.name] = kerbal;
                }
            }
            Utils.Log(String.Format("Kerbal Database: Loaded {0} Kerbals",Kerbals.Count ));
            Utils.Log("Kerbal Database: Loading Complete!");
        }

        internal void Save(ConfigNode node)
        {
            Utils.Log("Kerbal Database: Saving...");

            ConfigNode dbNode;
            bool init = node.HasNode(RadioactivitySettings.pluginConfigNodeName);
            if (init)
                dbNode =node.GetNode(RadioactivitySettings.pluginConfigNodeName);
            else
                dbNode = node.AddNode(RadioactivitySettings.pluginConfigNodeName);


            foreach (KeyValuePair<string, RadioactivityKerbal> kerbal in Kerbals)
            {
                Utils.Log(string.Format("Kerbal Database: Saving kerbal {0}", kerbal.Value));
                ConfigNode kNode = kerbal.Value.Save(dbNode);
                kNode.AddValue("KerbalName",kerbal.Key);
            }
            Utils.Log("Kerbal Database: Saving completed!");

        }


    }



}

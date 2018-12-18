using System;
using Ereader.Knowledge;
using UnityEngine;
using WikiQueries;

namespace Ereader{
    
    /**
     * Bridges wikipedia API results and renders them in unity
     */
        
    public class WikiBridge {
        
        private static GameObject wiki;

        public static void LogResults(String selection) {
            if (wiki == null) {
                wiki = new GameObject();
                wiki.AddComponent<Wiki>();
            }

            wiki.GetComponent<Wiki>().PageSummary(selection);
        }
    }
}
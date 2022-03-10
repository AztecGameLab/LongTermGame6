using JetBrains.Annotations;
using UnityEngine;

namespace Utility
{
    public class LinkOpener : MonoBehaviour
    {
        [PublicAPI]
        public void OpenLink(string link)
        {
            System.Diagnostics.Process.Start(link);
        }
    }
}
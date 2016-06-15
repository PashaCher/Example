using UnityEngine;

public class FireManager : MonoBehaviour
{
        public GameObject burningDummyPrefab = null;
        public GameObject sparkingDummyPrefab = null;

        // Ignite all objects with isBurning == true
        private void Start()
        {
                var targets = FindObjectsOfType(typeof (FireController));

                foreach (FireController target in targets)
                {
                        if (target.isBurning)
                        {
                                target.Ignite();
                        }
                }
        }
}

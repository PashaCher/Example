using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
        /// <summary>
        /// All currently burning objects dictionary.
        /// Key - burning object, Value - array of burning dummy and sparking dummy instances
        /// </summary>
        private static readonly Dictionary<GameObject, GameObject[]> burningObjectDictionary = new Dictionary<GameObject, GameObject[]>();

        /// <summary>
        /// Affects particle emission amount when burning
        /// </summary>
        public int intensity = 50;

        public bool isBurning = false;
        public bool isCombustible = true;

        // Start burning
        public void Ignite()
        {
                isBurning = true;

                var fireManager = (FireManager) FindObjectOfType(typeof (FireManager));
                if (fireManager == null)
                {
                        print("[Error] Igniting failed: can't find FireManager on scene.");
                        return;
                }
                if (fireManager.burningDummyPrefab == null)
                {
                        print("[Error] Igniting failed: burningDummyPrefab is not specified in FireManager.");
                        return;
                }
                if (fireManager.sparkingDummyPrefab == null)
                {
                        print("[Error] Igniting failed: sparkingDummyPrefab is not specified in FireManager.");
                        return;
                }

                var fireDummy = (GameObject) Instantiate(fireManager.burningDummyPrefab);
                fireDummy.transform.localPosition = transform.localPosition;
                fireDummy.transform.localRotation = transform.localRotation;
                fireDummy.transform.localScale = transform.localScale;
                fireDummy.transform.parent = transform;

                var sparkingDummy = (GameObject) Instantiate(fireManager.sparkingDummyPrefab);
                sparkingDummy.transform.localPosition = transform.localPosition;
                sparkingDummy.transform.localRotation = transform.localRotation;
                sparkingDummy.transform.localScale = transform.localScale;
                sparkingDummy.transform.parent = transform;

                var burningDummyMeshFilter = (MeshFilter) fireDummy.GetComponent("MeshFilter");
                if (burningDummyMeshFilter == null)
                {
                        print("[Error] Igniting failed: burningDummyPrefab in FireManager have no MeshFilter component");
                        return;
                }

                var sparkingDummyMeshFilter = (MeshFilter) sparkingDummy.GetComponent("MeshFilter");
                if (sparkingDummyMeshFilter == null)
                {
                        print("[Error] Igniting failed: sparkingDummyPrefab in FireManager have no MeshFilter component");
                        return;
                }

                var targetMeshFilter = (MeshFilter) GetComponent("MeshFilter");
                if (targetMeshFilter == null)
                {
                        print(string.Format("[Error] Igniting faild: burning object ({0}) have no MeshFilter component", name));
                        return;
                }

                burningDummyMeshFilter.mesh = sparkingDummyMeshFilter.mesh = targetMeshFilter.mesh;

                var targetFireController = (FireController) GetComponent("FireController");
                fireDummy.particleEmitter.minEmission = fireDummy.particleEmitter.maxEmission = targetFireController.intensity;

                burningObjectDictionary.Add(gameObject, new[] {fireDummy, sparkingDummy});
        }

        // Stop burning
        public void Extinguish()
        {
                isBurning = false;

                if (burningObjectDictionary.ContainsKey(gameObject) == false)
                {
                        print(string.Format("[Warning] Can't extinguish the object ({0}) that isn't burning.", name));
                }
                var dummies = burningObjectDictionary[gameObject];
                burningObjectDictionary.Remove(gameObject);
                Destroy(dummies[0]);
                Destroy(dummies[1]);
        }

        // Ignite the object if a spark particle hits it
        private void OnParticleCollision(GameObject target)
        {
                var controller = (FireController) target.GetComponent("FireController");
                if (controller == null)
                {
                        return;
                }

                if (controller.isCombustible && controller.isBurning == false)
                {
                        controller.Ignite();
                }
        }
}

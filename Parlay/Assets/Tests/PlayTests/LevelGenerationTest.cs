using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class LevelGenerationTest
    {
        private GameObject testObject;
        private LevelGenerator levelGenerator;
        private GameObject playerObject;

        [SetUp]
        public void Setup()
        {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelGenerator"));
            levelGenerator = testObject.GetComponent<LevelGenerator>();
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
            GameObject.Destroy(playerObject);
        }

        [UnityTest]
        public IEnumerator levelGeneration()
        {
            GameObject chunckObj = GameObject.FindGameObjectWithTag("Chunck");
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            //Check if starting chuncks were generated on load
            Assert.IsTrue(chunckObj != null);

            //Check if player was generated
            Assert.IsTrue(playerObj != null);

            //Check if new chunck is genreated when player moves near edge
            if (playerObj != null && chunckObj != null)
            {
                playerObj.transform.position = new Vector3(67, 1);
                yield return new WaitForSeconds(0.1f);
                Assert.IsTrue(GameObject.FindGameObjectsWithTag("Chunck").Length > 6);
            }

            yield return null;
        }
    }
}

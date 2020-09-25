using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TimerManagerTest
    {
        private GameObject testObject;
        private Timer timer;

        [SetUp]
        public void Setup() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TimeManager"));
            timer = testObject.GetComponent<Timer>();
            timer.initTimer(2f);
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
        }

        [UnityTest]
        public IEnumerator ShowsTheSliderAndText()
        {
            Assert.IsTrue(timer.timerSlider.enabled);
            Assert.IsTrue(timer.timerText.gameObject.activeSelf);
            yield return null;
        }

        // [UnityTest]
        // public IEnumerator TimerDecreasesTimeProperly()
        // {
        //     Assert.IsFalse(timer.isTimeUp());
        //
        //     yield return new WaitForSeconds(1f);
        //     timer.updateTime();
        //
        //     // time should decrease by one second
        //     Assert.AreEqual(timer.timerText.text, "0:01");
        //     Assert.IsFalse(timer.isTimeUp());
        // }

        [UnityTest]
        public IEnumerator TimerDoesNotGoToZero()
        {
            // making the timer go for more than the max (2 seconds)
            for (int i = 0; i < 3; i++) {
                timer.updateTime();
                yield return new WaitForSeconds(1f);
            }

            // Assert.AreEqual(Mathf.FloorToInt(timer.getCurrTime()), 0);
            Assert.IsTrue(timer.isTimeUp());
            Assert.AreEqual(timer.timerText.text, "0:00");
        }
    }
}

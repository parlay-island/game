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

        [UnityTest, Order(1)]
        public IEnumerator ShowsTheSliderAndText()
        {
            Assert.IsTrue(timer.timerSlider.enabled);
            Assert.IsTrue(timer.timerText.gameObject.activeSelf);
            yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator TimerDecreasesTimeProperly()
        {
            Assert.IsFalse(timer.isTimeUp());

            while (timer.getCurrTime() > 1) {
                timer.updateTime();
            }

            // time should decrease by one second
            Assert.AreEqual(timer.timerText.text, "0:01");
            Assert.AreEqual(Mathf.Ceil(timer.getCurrTime()), 1);
            Assert.IsFalse(timer.isTimeUp());

            yield return null;

        }

        [UnityTest, Order(3)]
        public IEnumerator TimerDoesNotGoToZero()
        {
            while (timer.getCurrTime() > 0) {
                timer.updateTime();
            }

            Assert.IsTrue(timer.isTimeUp());
            Assert.AreEqual(timer.getCurrTime(), 0f);
            Assert.AreEqual(timer.timerText.text, "0:00");

            // on another update, time should NOT go below 0
            timer.updateTime();
            Assert.AreEqual(timer.getCurrTime(), 0f);
            Assert.AreEqual(timer.timerText.text, "0:00");
            yield return null;

        }
    }
}

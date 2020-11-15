using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
* This file tests the time manager
* 
* @author: Jessica Su
*/

namespace Tests
{
    public class TimerManagerTest
    {
        private GameObject testObject;
        private Timer timer;
        private Sprite neutral_image;
    		private Sprite positive_image;
    		private Sprite negative_image;

        [SetUp]
        public void Setup() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Timer/TimeManager"));
            timer = testObject.GetComponent<Timer>();
            timer.initTimer(2f);
            neutral_image = timer.neutral_image;
            positive_image = timer.positive_image;
            negative_image = timer.negative_image;
        }

        [TearDown]
        public void Teardown()
        {
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [UnityTest, Order(1)]
        public IEnumerator ShowsTheSliderAndText()
        {
            Assert.IsTrue(timer.timerSlider.enabled);
            Assert.IsTrue(timer.timerText.gameObject.activeSelf);
            Assert.AreEqual(timer.timer_image.sprite, neutral_image);
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

        [UnityTest, Order(4)]
        public IEnumerator TestTimerImageChangeForPositiveTimeAddition()
        {
            Sprite currImage = timer.timer_image.sprite;
            timer.AddTime(1f);
            Assert.AreEqual(timer.timer_image.sprite, positive_image);
            yield return null;
        }

        [UnityTest, Order(4)]
        public IEnumerator TestTimerImageChangeForNegativeTimeAddition()
        {
            Sprite currImage = timer.timer_image.sprite;
            timer.AddTime(-1f);
            Assert.AreEqual(timer.timer_image.sprite, negative_image);
            yield return null;
        }
    }
}

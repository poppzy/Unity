using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SampleTest
{
    HealthScript healthScript;

    [SetUp]
    public void SetUp()
    {
        healthScript = new HealthScript(10, 100);
    }

    [TearDown]
    public void TearDown()
    {
        healthScript = null;
    }

    [Test]
    public void CanAddHealth()
    {
        healthScript.AddHealth(10);
        Assert.Greater(healthScript.GetHealth(), 10);
    }

    [Test]
    public void CanAddHealthEvenThoughAmountIsNegative()
    {
        healthScript.AddHealth(-10);
        Assert.Greater(healthScript.GetHealth(), 10);
    }

    [Test]
    public void CanRemoveHealth()
    {
        healthScript.RemoveHealth(10);
        Assert.Less(healthScript.GetHealth(), 10);
    }

    [Test]
    public void CanRemoveHealthEvenThoughAmountIsNegative()
    {
        healthScript.RemoveHealth(-10);
        Assert.Less(healthScript.GetHealth(), 10);
    }
}

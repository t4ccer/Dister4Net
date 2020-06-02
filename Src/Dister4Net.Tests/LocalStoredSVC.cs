using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Modules.SharedValues;
using Xunit;

namespace Dister4Net.Tests.SharedValues
{
    public class LocalStored
    {
        [Theory]
        [InlineData("foo", 42)]
        [InlineData("bar", 1337)]
        [InlineData("baz", -5)]
        public void SetAndGetInt(string name, int value)
        {
            var svc = new LocalStoredSVC();
            svc.StartModule();

            svc.Set(name, value);
            Assert.Equal(value, svc.Get<int>(name));
        }

        [Theory]
        [InlineData("foo", 42)]
        [InlineData("bar", 1337)]
        [InlineData("baz", -5)]
        public void SetAndChange(string name, int value)
        {
            var svc = new LocalStoredSVC();
            svc.StartModule();

            var newValue = value + 5;

            svc.Set(name, value);
            svc.Set(name, newValue);
            Assert.Equal(newValue, svc.Get<int>(name));
        }
        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        public void GetNotExisting(string name)
        {
            var svc = new LocalStoredSVC();
            svc.StartModule();

            Assert.Throws<KeyNotFoundException>(() => svc.Get<int>(name));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDictionaryGenerator;
using Xunit;

namespace DataDictionaryGeneratorTest
{
    internal enum TestEnum
    {
        Red = 1,
        [Display(Name = "Dark green")]
        Green = 2,
        [Display(Name = "Light blue")]
        Blue = 3
    }
    public class OptionsTest
    {
        [Fact]
        public void ReturnsThreeItems()
        {
            var result = DataDictionary.GetFieldOptions(typeof(TestEnum));
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void HasDisplayName_ReturnsDisplayName()
        {
            var result = DataDictionary.GetFieldOptions(typeof(TestEnum));
            Assert.Equal("Dark green", result["2"]);
        }

        [Fact]
        public void NoDisplayName_ReturnsFieldName()
        {
            var result = DataDictionary.GetFieldOptions(typeof(TestEnum));
            Assert.Equal("Red", result["1"]);
        }
    }
}

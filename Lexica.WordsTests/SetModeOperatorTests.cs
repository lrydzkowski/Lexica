using Lexica.Words;
using Lexica.Words.Models;
using Lexica.Words.Services;
using Lexica.WordsTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Lexica.WordsTests
{
    public class SetModeOperatorTests
    {
        [Fact]
        public async void RandomizeEntries_ProperConditions_ReturnsEntriesInRandomOrder()
        {
            ISetService setService = new MockSetService();
            var setModeOperator = new SetModeOperator(setService, 1);

            await setModeOperator.LoadSet();
            if (setModeOperator.Set == null)
            {
                throw new Exception("Set is null.");
            }

            setModeOperator.Reset();
            await setModeOperator.Randomize();
            var str1 = "";
            var entry1 = await setModeOperator.GetNextEntry();
            while (entry1 != null)
            {
                str1 += string.Join(',', entry1.Words) + ',' + string.Join(',', entry1.Translations);
                entry1 = await setModeOperator.GetNextEntry();
            }

            setModeOperator.Reset();
            await setModeOperator.Randomize();
            var str2 = "";
            var entry2 = await setModeOperator.GetNextEntry();
            while (entry2 != null)
            {
                str2 += string.Join(',', entry2.Words) + ',' + string.Join(',', entry2.Translations);
                entry2 = await setModeOperator.GetNextEntry();
            }

            Assert.NotEqual(str1, str2);
        }

        [Fact]
        public async void GetAllEntries_ProperConditions_ReturnsAllEntries()
        {
            ISetService setService = new MockSetService();
            var setModeOperator = new SetModeOperator(setService, 1);

            await setModeOperator.LoadSet();
            if (setModeOperator.Set == null)
            {
                throw new Exception("Set is null.");
            }

            setModeOperator.Reset();
            await setModeOperator.Randomize();
            int index = 0;
            var entry = await setModeOperator.GetNextEntry();
            while (entry != null)
            {
                entry = await setModeOperator.GetNextEntry();
                index++;
            }

            Assert.Equal(31, index);
        }
    }
}

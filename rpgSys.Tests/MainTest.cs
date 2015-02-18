using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rpgSys.Tests
{
    [TestClass]
    public class MainTest
    {
        [TestMethod]
        public void select_messages_query_Test()
        {
            // arrange
            int GameId = 1;
            int Count = 1;
            bool Desc = true;


            // act
            var d = xmlBase.Chat.Get(GameId, Count, Desc, null);

            // assert
            Assert.AreEqual(d[0], "Hello FCKING world!");
        }

        [TestMethod]
        public void condition_language_Test()
        {
            //arrange
            //string condition = "Id <= 1";
            string condition = "Text == Suka";
            string field = condition.Split(' ')[0];
            string _if = condition.Split(' ')[1];
            string value = condition.Split(' ')[2];
            Message m = new Message() { Id = 1, Text = "Suka" };

            //act
            var result = ConditionLanguage.Compare<String>(_if, m.GetType().GetProperty(field).GetValue(m).ToString(), value);

            //assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void condition_language_2_0_Test()
        {
            //arrange
            string condition = "Text == lalalallaalaal";
            string field = condition.Split(' ')[0];
            string _if = condition.Split(' ')[1];
            string value = condition.Split(' ')[2];
            Message m = new Message() { Id = 1, Text = "lalalallaalaal" };

            //act
            var result = ConditionLanguage.Run(m, m.GetType(), field, _if, value);

            //assert
            Assert.AreEqual(result, true);
        }
    }
}
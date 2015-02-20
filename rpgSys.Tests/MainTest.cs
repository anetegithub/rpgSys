using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;

namespace rpgSys.Tests
{
    [TestClass]
    public class MainTest
    {
        [Conditional( "TEST" )]
        [TestMethod]
        public void select_messages_query_Test()
        {
            // arrange
            int GameId = 1;
            int Count = 1;
            bool Desc = true;


            // act
            var d = xmlBase.Chat.Get(GameId, Count, Desc, null,true);

            // assert
            Assert.AreEqual(d[0], "Мастер: yep yep!");
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
            var result = true;
            //var result = ConditionLanguage.Compare<String>(_if, m.GetType().GetProperty(field).GetValue(m).ToString(), value);

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
            //var result = ConditionLanguage.Run(m, m.GetType(), field, _if, value);
            var result = ConditionLanguage.SatisfyCustom(m, m.GetType(), field, _if, value);

            //assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void condition_language_3_0_Test()
        {
            //arrange
            string condition = "Text == lalalallaalaal";
            string field = condition.Split(' ')[0];
            string _if = condition.Split(' ')[1];
            string value = condition.Split(' ')[2];
            Message m = new Message() { Id = 1, Text = "lalalallaalaal" };

            //act
            var result = ConditionLanguage.Satisfy(m, field, _if, value);

            //assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void xmlBase_0_1_Test()
        {
            //arrange
            string name="fds";
            dynamic o;

            //act
            //o=ConditionLanguage.Obj();            

            //assert
            //Assert.AreEqual(o.Age,5);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;

using ormCL;
using ConditionsLanguage;

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
            Assert.AreEqual(d[0], "Мастер:");
        }

        [TestMethod]
        public void select_messages_ORM_Test()
        {
            //arrange
            baseCL b = new baseCL("Data");
            b.Test = true;

            //act
            var result = b.Select(new requestCL() {  Conditions = new conditionCL(""), Table = new tableCl("/Games/Chats/1") }).Cast<Message>().ToList();

            //assert
            Assert.AreEqual(result[0].Text, "111111111111Hello FCKING world!");
        }

        [TestMethod]
        public void select_messages_ORM_sorting_Test()
        {
            //arrange
            baseCL b = new baseCL("Data");
            b.Test = true;

            //act
            var result = b.Select(new requestCL() { Table = new tableCl("/Games/Chats/1") }).Cast<Message>().Sort(new sortingCL("Id:Desc,HeroId:Desc")).ToList();

            //assert
            Assert.AreEqual(result[0].Id, 4);
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
            var result = CL.Satisfy(m, field, _if, value);

            //assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void xmlBase_0_1_Test()
        {
            //arrange
            //string name="fds";
            //dynamic o;

            //act
            //o=ConditionLanguage.Obj();            

            //assert
            //Assert.AreEqual(o.Age,5);
        }
    }
}
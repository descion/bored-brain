using System;
using System.IO;
using BoredBrain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoredBrainTests {
    [TestClass]
    public class BoardSerializationTest {

        [TestMethod]
        public void SaveAndLoadTest() {
            if (Directory.Exists("TestFolder")) {
                Directory.Delete("TestFolder", true);
            }

            Directory.CreateDirectory("TestFolder");

            Board b = new Board("TestFolder");

            Field test1 = new TextField() {
                Name = "FirstTestField"
            };

            b.Structure.AddField(test1);

            Field test2 = new MultiselectField() {
                Name = "SecondTestField"
            };

            b.Structure.AddField(test2);

            Card newCard = b.CreateCard();

            newCard.Title = "My first Card!";
            newCard.Content = "Main content Stuff with all the nice things that you need!\n[] Done!";

            newCard.SetField(test1.Id, "This is my field1 value.");
            newCard.SetField(test2.Id, new string[] { "Tag1", "Tag2", "Tag3" });

            b.AddCard(newCard);

            b.Save();

            Board b2 = new Board("TestFolder");
            b2.Load();
            b2.Save();

            Board b3 = new Board("TestFolder");
            b3.Load();

            Assert.AreEqual(b.Structure.Fields.Count, b2.Structure.Fields.Count);
            Assert.AreEqual(b.Cards.Count, b2.Cards.Count);
            Assert.AreEqual(b.Cards[0].Id, b2.Cards[0].Id);
            Assert.AreEqual(b.Cards[0].Title, b2.Cards[0].Title);
            Assert.AreEqual(b.Cards[0].Content, b2.Cards[0].Content);
        }
    }
}

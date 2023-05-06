using System.IO;
using BoredBrain.Models;
using BoredBrain.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoredBrainTests {
    [TestClass]
    public class BoardSerializationTest {

        //---------------------------------------------------------------------------

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

            newCard.SetFieldValue(test1, "This is my field1 value.");
            newCard.SetFieldValue(test2, new string[] { "Tag1", "Tag2", "Tag3" });

            b.AddCard(newCard);

            BoardSerializer.Save(b);

            Board b2 = new Board("TestFolder");
            BoardSerializer.Load(b2);
            BoardSerializer.Save(b2);

            Board b3 = new Board("TestFolder");
            BoardSerializer.Load(b3);

            Assert.AreEqual(b.Structure.Fields.Count, b2.Structure.Fields.Count);
            Assert.AreEqual(b.Cards.Count, b2.Cards.Count);
            Assert.AreEqual(b.Cards.First.Value.Id, b2.Cards.First.Value.Id);
            Assert.AreEqual(b.Cards.First.Value.Title, b2.Cards.First.Value.Title);
            Assert.AreEqual(b.Cards.First.Value.Content, b2.Cards.First.Value.Content);
        }

        //---------------------------------------------------------------------------

        [TestMethod]
        public void RenameFieldTest() {
            if (Directory.Exists("TestFolder")) {
                Directory.Delete("TestFolder", true);
            }

            Directory.CreateDirectory("TestFolder");

            Board b = new Board("TestFolder");

            Field test1 = new TextField() {
                Name = "FirstTestField"
            };

            b.Structure.AddField(test1);

            Card newCard = b.CreateCard();

            newCard.Title = "RenameField";
            newCard.SetFieldValue(test1, "TestValue");

            b.AddCard(newCard);
            BoardSerializer.Save(b);

            test1.Name = "RenamedField";

            Board b2 = new Board("TestFolder");
            BoardSerializer.Load(b2);

            Assert.AreEqual(test1.ConvertValueToString(b.Cards.First.Value.GetFieldValue(test1)), "TestValue");
        }
    }
}

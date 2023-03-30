using AppData.Enums;
using AppData.Interfaces;
using AppData.Models;
using Moq;
using NetDeveloperTask;
using NetDeveloperTask.Validators;

namespace NetDeveloperTaskTesting
{
    [TestClass]
    public class UnitTest1
    {
        private List<ResourceShortage> Data { get; set; }

        private ResourceShortageValidator _validator;

        private Mock<IRepository> _repository;

        private CommandService commandService;

        private string _userName;

        [TestInitialize]
        public void Init()
        {
            _userName = Environment.UserName;

            Data = new List<ResourceShortage>()
            {
                new ResourceShortage {
                    UserName = _userName,
                    Title = "title1",
                    Name = "name",
                    Room = Room.MeetingRoom,
                    Category = Category.Food,
                    CreatedOn = "2023-01-15",
                    Priority = 1
                },

                new ResourceShortage {
                    UserName = _userName,
                    Title = "title2",
                    Name = "name2",
                    Room = Room.Kitchen,
                    Category = Category.Electronics,
                    CreatedOn = "2023-02-15",
                    Priority = 1
                },
                new ResourceShortage {
                    UserName = _userName,
                    Title = "title two",
                    Name = "name3",
                    Room = Room.Bathroom,
                    Category = Category.Other,
                    CreatedOn = "2023-02-15",
                    Priority = 1
                }
            };

            _validator = new ResourceShortageValidator();
            _repository = new Mock<IRepository>();

            _repository.Setup(x => x.FindAllOrderedPriority())
                .Returns(Data.AsQueryable().OrderByDescending(r => r.Priority));

            commandService = new CommandService(_repository.Object, _validator);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(null, null, null, 0, 0, 3)]
        [DataRow("title1", null, null, 0, 0, 1)]
        [DataRow("two", null, null, 0, 0, 1)]
        [DataRow(null, null, null, 1, 0, 1)]
        [DataRow(null, null, null, 0, 1, 1)]
        [DataRow(null, "2023-01-15", null, 0, 0, 1)]
        [DataRow(null, "2023-01-14", "2023-01-16", 0, 0, 1)]
        [DataRow("title1", "2023-01-14", "2023-01-16", 0, 0, 1)]
        public void GetResourceShortage_VariousFilters_CorrectAmount(string title, string date1, string date2, Room room, Category category, int amount)
        {
            var resourceShortageFilter = new ResourceShortageFilter()
            {
                Title = title,
                Date1 = date1,
                Date2 = date2,
                Room = room,
                Category = category
            };

            var results = commandService.GetResourceShortage(resourceShortageFilter);

            Assert.AreEqual(results.Count, amount);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(null, "name", 1, 1, 1)]
        [DataRow("title", null, 1, 1, 1)]
        [DataRow("title", "name", 0, 1, 1)]
        [DataRow("title", "name", 1, 0, 1)]
        [DataRow("title", "name", 1, 1, -5)]
        [DataRow("title", "name", 1, 1, 13)]
        public void Validate_NonValidValues_IsValidValueIsFalse(string title, string name, Room room, Category category, int priority)
        {
            var resourceShortage = new ResourceShortage()
            {
                Title = title,
                Name = name,
                Room = room,
                Category = category,
                Priority = priority
            };

            bool isValid = _validator.Validate(resourceShortage).IsValid;

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("title", "name", Room.MeetingRoom, Category.Electronics, 1)]
        [DataRow("title", "name", Room.MeetingRoom, Category.Electronics, 10)]
        public void Validate_ValidValues_IsValidValueIsTrue(string title, string name, Room room, Category category, int priority)
        {
            var resourceShortage = new ResourceShortage()
            {
                Title = title,
                Name = name,
                Room = room,
                Category = category,
                Priority = priority
            };

            bool isValid = _validator.Validate(resourceShortage).IsValid;

            Assert.IsTrue(isValid);
        }
    }
}
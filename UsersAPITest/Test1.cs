using UsersAPI;
using UsersAPI.Domain;
using FluentValidation;

namespace UsersAPITest
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void UserTooYoung()
        {
            var user = new User() 
            { 
                FirstName = "Bob", 
                LastName = "Malooga", 
                Email = "b@m.com", 
                PhoneNumber = "1231231234", 
                DateOfBirth = new DateTime(2020,1,1) 
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors[0].PropertyName=="DateOfBirth");
        }
        [TestMethod]
        public void FirstNameBlank()
        {
            var user = new User()
            {
                FirstName = string.Empty,
                LastName = "Malooga",
                Email = "b@m.com",
                PhoneNumber = "1231231234",
                DateOfBirth = new DateTime(2020, 1, 1)
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors[0].PropertyName == "FirstName");
        }
        [TestMethod]
        public void MissingEmail()
        {
            var user = new User()
            {
                FirstName = "Bob",
                LastName = "Malooga",
                Email = string.Empty,
                PhoneNumber = "1231231234",
                DateOfBirth = new DateTime(2020, 1, 1)
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors[0].PropertyName == "Email");
        }
        [TestMethod]
        public void BadPhoneNumber()
        {
            var user = new User()
            {
                FirstName = "Bob",
                LastName = "Malooga",
                Email = "b@m.com",
                PhoneNumber = "123123123",
                DateOfBirth = new DateTime(2020, 1, 1)
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors[0].PropertyName == "PhoneNumber");
        }
        [TestMethod]
        public void FirstNameTooLong()
        {
            var user = new User()
            {
                FirstName = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ",
                LastName = "Malooga",
                Email = "b@m.com",
                PhoneNumber = "1231231234",
                DateOfBirth = new DateTime(2020, 1, 1)
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors[0].PropertyName == "FirstName");
        }
        [TestMethod]
        public void LastNameTooLong()
        {
            var user = new User()
            {
                FirstName = "Bob",
                LastName = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ",
                Email = "b@m.com",
                PhoneNumber = "1231231234",
                DateOfBirth = new DateTime(2020, 1, 1)
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors[0].PropertyName == "LastName");
        }
    
        [TestMethod]
        public void UserValid()
        {
            var user = new User()
            {
                FirstName = "Bob",
                LastName = "Malooga",
                Email = "b@m.com",
                PhoneNumber = "1231231234",
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            UserValidator _validator = new UserValidator();
            var results = _validator.Validate(user);
            Assert.IsTrue(results.IsValid);
        }
    }
}

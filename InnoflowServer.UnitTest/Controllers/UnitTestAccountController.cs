using Moq;
using NUnit.Framework;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using InnoflowServer.Controllers;
using InnoflowServer.Services.Interfaces;
using InnoflowServer.Services.Interfaces.Users;
using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Core.DTO;
using InnoflowServer.Domain.Core.Models;

namespace InnoflowServer.UnitTest.Controllers
{
    public class UnitTestAccountController
    {
        private AccountController _controller;
        private Mock<IUserService> _mockService;
        private Mock<IMapper> _mockMapper;

        private List<UserDTO> ForTestComment()
        {
            var users = new List<UserDTO>
            {
                new UserDTO {FirstName = "Anton", LastName = "Molch", Email = "bob@gmail.com", Password = "12345678", UserJobCategories = new List<string>(){ "Programmer", "QA"} },
                new UserDTO {FirstName = "Alex", LastName = "Molodoi", Email = "gnom@gmail.com", Password = "123456789", UserJobCategories = new List<string>(){ "QA", "Plotnik"}}
            };
            return users;
        }

        public UnitTestAccountController()
        {
            _mockMapper = new Mock<IMapper>();
            _mockService = new Mock<IUserService>();
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new AccountController(_mockService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task RegisterAlreadyExist_Valid()
        {
            var model = new UserDTO { FirstName = "Anton", LastName = "Molch", Email = "bob@gmail.com", Password = "12345678", UserJobCategories = new List<string>() { "Programmer", "QA" } };
            _mockService.Setup(s => s.Register(model)).ReturnsAsync(ForTestComment().FirstOrDefault(u => u.Email == model.Email).ToString());

            var RegisterModel = new RegisterUserModel { FirstName = "Anton", LastName = "Molch", Email = "bob1@gmail.com", Password = "12345678", UserJobCategories = new List<string>() { "Programmer", "QA" } };
            var result = await _controller.Register(RegisterModel);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task LoginFailed_Valid()
        {
            var model = new UserDTO { FirstName = "Anton", LastName = "Molch", Email = "bob@gmail.com", Password = "12345678", UserJobCategories = new List<string>() { "Programmer", "QA" } };
            _mockService.Setup(s => s.Register(model)).ReturnsAsync(ForTestComment().FirstOrDefault(u => u.Email == model.Email).ToString());

            var LoginModel = new LoginUserModel {Email = "bob@gmail.com", Password = "12345678"};
            var result = await _controller.Login(LoginModel);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}

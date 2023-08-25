using Moq;
using AutoMapper;
using Principal.Telemedicine.SharedApi.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Principal.Telemedicine.DataConnectors.Mapping;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.DataConnectors.Repositories;
using Xunit;

namespace Principal.Telemedicine.SharedApi.Test;

public class UserApiControllerTest
    {
        
        [Fact(DisplayName = "Test api metody pro vrácení základních údajů uživatele")]
        public async Task GetUserInfo_Should_Return_Ok_Result()
        {
            // arrange
            var logger = new LoggerFactory().CreateLogger<UserApiController>();

            Mock<ICustomerRepository> repository = new Mock<ICustomerRepository>();

            var expected = new Customer()
            {
                Active = true,
                Id = 8,
                Deleted = false,
                CreatedDateUtc = DateTime.Now,
                OrganizationId = 1,
                FirstName = "Oskar",
                LastName = "Oskarovič",
                AddressLine = "Stará 45, Náchod",
                PostalCode = "25601",
                Email = "oskar.oskarovic@principal.cz",
                TelephoneNumber = "00420725781448",
                PersonalIdentificationNumber = "8905148109",
                Birthdate = DateTime.Today,
                IsSystemAccount = false,
                IsSuperAdminAccount = false,
                IsOrganizationAdminAccount = false,
                IsProviderAdminAccount = false,
                PasswordFormatTypeId = 0,
                InvalidLoginsCount = 0,
                FriendlyName = "Oskar Oskarovič",
                GlobalId = "1ef9d9d4d2bd4f80819635de5eb758cd",
                Street = "Stará",
                CityId = 299
            };

            repository.Setup(m => m.GetCustomerByIdTaskAsync(It.IsAny<int>()))
                .ReturnsAsync(expected);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Mapping());
            });
            var mapper = mockMapper.CreateMapper();

            // create the controller
            var controller = new UserApiController(repository.Object, logger, mapper);

            // act
            var result = await controller.GetUserInfo("api-key", 8);
            var okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.SharedApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Principal.Telemedicine.SharedApi.Test;

    public class PatientInfoApiControllerTest
    {

        [Fact(DisplayName = "Test api metody pro vrácení souhrnných informací uživatele ohledně naměřených hodnot, predikcí a podobně.")]
        public async Task GetAggregatedUserSymptomProgressionDataModel_Should_Return_Ok_Result()
        {
            // arrange
            int userId = 8;
            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();
            var dbOptionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .Options;

            using var context = new ApiDbContext(dbOptionsBuilder);

            // create the controller
            var controller = new PatientInfoApiController(logger, context);

            // act
            var result = await controller.GetAggregatedUserSymptomProgressionDataModel(userId);
            var okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact(DisplayName = "Test api metody pro vrácení výsledků predikce nemoci.")]
        public async Task GetDiseaseDetectionResultFromMLItems_Should_Return_Ok_Result()
        {
            // arrange
            int userId = 8;
            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();
            var dbOptionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .Options;

            using var context = new ApiDbContext(dbOptionsBuilder);

            using var transaction = context.Database.BeginTransaction();

            // fix up data
            context.Set<DiseaseDetectionResultFromMLItemDataModel>().Add(new DiseaseDetectionResultFromMLItemDataModel()
            {
                UserId = 8,
                Type = "actual",
                DetectionDate = DateTime.Now,
                DiseaseTypeId = 17,
                DiseaseTypeName = "Pylová alergie",
                ProbablityPercent = 80,
                Severity = 0,
                DiseaseDetectionResultId = 30290

            });

            // create the controller
            var controller = new PatientInfoApiController(logger, context);

            // act
            var result = await controller.GetDiseaseDetectionResultFromMLItems(userId);
            var okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            transaction.Rollback();
        }


        [Fact(DisplayName = "Test api metody pro vrácení výsledků predikce původce nemoci.")]
        public async Task GetDiseaseOriginDetectionResultFromMLItems_Should_Return_Ok_Result()
        {
            // arrange
            int userId = 8;
            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();
            var dbOptionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .Options;

            using var context = new ApiDbContext(dbOptionsBuilder);

            using var transaction = context.Database.BeginTransaction();

            // fix up data
            context.Set<DiseaseOriginDetectionResultFromMLItemDataModel>().Add(new DiseaseOriginDetectionResultFromMLItemDataModel()
            {
                UserId = 8,
                Type = "actual",
                DetectionDate = DateTime.Now,
                DiseaseOriginTypeId = 1,
                DiseaseOriginTypeName = "Viry",
                ProbablityPercent = 41,
                Severity = 0,
                DiseaseOriginDetectionResultId = 10639

            });

            // create the controller
            var controller = new PatientInfoApiController(logger, context);

            // act
            var result = await controller.GetDiseaseOriginDetectionResultFromMLItems(userId);
            var okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            transaction.Rollback();
        }


        [Fact(DisplayName = "Test api metody pro vrácení informací o klíčových vstupech.")]
        public async Task GetDiseaseDetectionKeyInputsToMLItems_Should_Return_Ok_Result()
        {
            // arrange
            int userId = 8;
            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();
            var dbOptionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .Options;

            using var context = new ApiDbContext(dbOptionsBuilder);
            
            using var transaction = context.Database.BeginTransaction();

            // fix up data
            context.Set<DiseaseDetectionKeyInputsToMLItemDataModel>().Add(new DiseaseDetectionKeyInputsToMLItemDataModel()
                { 
                    UserId = 8,
                    Type = "Měřitelná hodnota",
                    CreatedDate = DateTime.Now,
                    Name = "Tep",
                    DiseaseSymptomTypeId = null,
                    DiseaseSymptomCategoryId = null,
                    PhysiologicalDataTypeId = 1,
                    WeightPercent = 0,
                    Value = 84,
                    IsBooleanValue = false,
                    Range = "60.0 - 95.0 bpm",
                    Severity = Shared.Enums.SymptomSeverityEnum.Moderate,
                    UnitType = "bpm"

                });

            // create the controller
            var controller = new PatientInfoApiController(logger, context);

            // act
            var result = await controller.GetDiseaseDetectionKeyInputsToMLItems(userId);
            var okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            transaction.Rollback();
        }


        [Fact(DisplayName = "Test api metody pro vrácení přehledu o aktuálních/nadcházejících prohlídkách/karanténách/izolacích daného pacienta.")]
        public async Task GetVirtualSurgeryBasicOverview_Should_Return_Ok_Result()
        {
            // arrange
            int userId = 8;
            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();

            var dbOptionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .Options;

            using var context = new ApiDbContext(dbOptionsBuilder);

            using var transaction = context.Database.BeginTransaction();


            // fix up data
            context.Set<VirtualSurgeryBasicOverviewDataModel>().Add(new VirtualSurgeryBasicOverviewDataModel()
            {
                UserId = 8,
                IsolationStartDateUtc = null,
                IsolationEndDateUtc = null,
                IsolationNotificationDeliveryDateUtc = null,
                IsolationStateId = null,
                IsolationStateName = null,
                InvitationToMedicalExaminationRequestDateUtc = DateTime.Now,
                InvitationToMedicalExaminationDeliveryDateUtc = null,
                DiseaseDetectionValidationCreatedDateUtc = null,
                DiseaseDetectionValidationTypeId = 2,
                DiseaseDetectionValidationTypeName = "Uživatel nemá respirační infekci",
                DiseaseDetectionResultId = null,
                DiseaseOriginDetectionResultId = null,
                DiseaseTypeId = null,
                DiseaseTypeName = null,
                DiseaseOriginTypeId = null,
                DiseaseOriginTypeName = null,
                QuarantineStartDateUtc = null,
                QuarantineEndDateUtc = null,
                QuarantineNotificationDeliveryDateUtc = null,
                QuarantineStateId = null,
                QuarantineStateName = null
            });

            // create the controller
            var controller = new PatientInfoApiController(logger, context);

            // act
            var result = await controller.GetVirtualSurgeryBasicOverview(userId);
            OkObjectResult okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            transaction.Rollback();
        }


        [Fact(DisplayName = "Test api metody pro vrácení in/aktivních zařízení daného uživatele.")]
        public async Task GetAvailableDeviceListItems_Should_Return_Ok_Result()
        {
            // arrange
            string userGlobalId = "15651bb054a84e8ca2c9f7dfc9bffb07";
            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();

            var dbOptionsBuilder = new DbContextOptionsBuilder<ApiDbContext>()
                .Options;

            using var context = new ApiDbContext(dbOptionsBuilder);

            using var transaction = context.Database.BeginTransaction();


            // fix up data
            context.Set<AvailableDeviceListItemDataModel>().Add(new AvailableDeviceListItemDataModel()
            { 
                UserGlobalId = "15651bb054a84e8ca2c9f7dfc9bffb07",
                IsConsent = 0,
                DeviceGlobalId = "7e0d59b0b41047f5adc4c75a341d0a7c",
                DeviceProducerName = "Omron",
                UserAccountConsentStatusTypeId = null,
                IsOwnedByUser = false,
                IsAbstract = false,
                DeviceCategoryId = 2,
                Active = false
            });

            // create the controller
            var controller = new PatientInfoApiController(logger, context);

            // act
            var result = await controller.GetAvailableDeviceListItems(userGlobalId);
            OkObjectResult okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            transaction.Rollback();
        }
    }






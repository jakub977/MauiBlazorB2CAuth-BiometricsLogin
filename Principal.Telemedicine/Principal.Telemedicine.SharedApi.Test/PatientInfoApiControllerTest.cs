using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.SharedApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using System;
using EntityFrameworkCore.Testing.Moq;
using Xunit;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;

namespace Principal.Telemedicine.SharedApi.Test
{
    public class PatientInfoApiControllerTest 
    {
        
        [Fact]
        public async Task GetVirtualSurgeryBasicOverview_Should_Return_Ok_Result()
        {
            int userId = 8;

            var logger = new LoggerFactory().CreateLogger<PatientInfoApiController>();
            var dbOptionsBuilder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("Test");

            // arrange
            using (var db = new ApiDbContext(dbOptionsBuilder.Options))
            {
                
                // fix up data
                db.Set<VirtualSurgeryBasicOverviewDataModel>().Add(new VirtualSurgeryBasicOverviewDataModel()
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
                await db.SaveChangesAsync();

                // create the controller
                var controller = new PatientInfoApiController(logger, db);

                // act
                var result = await controller.GetVirtualSurgeryBasicOverview(userId);

                OkObjectResult okResult = result as OkObjectResult;

                // assert
                Assert.NotNull(okResult);
            }

            
        }
    }

    
}

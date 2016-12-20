using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FizzWare.NBuilder;
using Moq;
using MvcContrib.TestHelper;
using QOAM.Core;
using QOAM.Core.Import;
using QOAM.Core.Import.QOAMcorners;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;
using QOAM.Website.ViewModels.QoamCorners;
using Xunit;

namespace QOAM.Website.Tests.Controllers
{
    
    public class QoamCornersControllerTests
    {
        #region Fields

        Mock<ICornerRepository> _cornerRepository;
        IList<Corner> _corners;

        #endregion

        [Fact]
        public void A_Corner_Admin_does_not_increase_the_visitor_count()
        {
            var model = new CornersIndexViewModel { Corner = 1 };

            var sut = CreateController();

            sut.Index(model);

            var corner = _corners.Single(c => c.Id == 1);

            Assert.Equal(3, corner.NumberOfVisitors);
            _cornerRepository.Verify(x => x.Save(), Times.Once());
        }

        [Fact]
        public void A_new_visitor_increases_the_visitor_count()
        {
            var model = new CornersIndexViewModel { Corner = 2 };

            var sut = CreateController("127.0.0.1");

            sut.Index(model);

            var corner = _corners.Single(c => c.Id == 2);

            Assert.Equal(4, corner.NumberOfVisitors);
            Assert.Equal(4, corner.CornerVisitors.Count);
            Assert.Equal("127.0.0.1", corner.CornerVisitors.Last().IpAddress);

            _cornerRepository.Verify(x => x.Save(), Times.Once());
        }

        [Fact]
        public void An_immediate_returning_visitor_does_not_increase_the_visitor_count()
        {
            var model = new CornersIndexViewModel { Corner = 2 };
            var ip = "127.0.0.1";
            var sut = CreateController(ip);

            AddCornerVisitor(2, ip, DateTime.Now.AddMinutes(-5));

            sut.Index(model);

            var corner = _corners.Single(c => c.Id == 2);

            Assert.Equal(4, corner.NumberOfVisitors);
            Assert.Equal(4, corner.CornerVisitors.Count);
            Assert.Equal("127.0.0.1", corner.CornerVisitors.Last().IpAddress);

            _cornerRepository.Verify(x => x.Save(), Times.Once());
        }

        [Fact]
        public void An_visitor_returning_after_an_hour_increases_the_visitor_count()
        {
            var model = new CornersIndexViewModel { Corner = 2 };
            var ip = "127.0.0.1";
            var sut = CreateController(ip);

            AddCornerVisitor(2, ip, DateTime.Now.AddMinutes(-61));

            sut.Index(model);

            var corner = _corners.Single(c => c.Id == 2);

            Assert.Equal(5, corner.NumberOfVisitors);
            Assert.Equal(5, corner.CornerVisitors.Count);

            _cornerRepository.Verify(x => x.Save(), Times.Once());
        }


        #region Private Methods

        QoamCornersController CreateController(string ipAddress = "")
        {
            var subjectRepository = new Mock<ISubjectRepository>();
            subjectRepository.Setup(x => x.Active).Returns(new List<Subject>());

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.UserHostAddress).Returns(ipAddress);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            
            var controller =  new QoamCornersController(Mock.Of<IBaseScoreCardRepository>(),  Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IUserProfileRepository>(), CreateAuthentication(), Mock.Of<IJournalRepository>(), CreateCornerRepository(), Mock.Of<IBulkImporter<CornerToImport>>(), subjectRepository.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            return controller;
        }

        IAuthentication CreateAuthentication()
        {
            var authentication = new Mock<IAuthentication>();
            authentication.Setup(a => a.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            authentication.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);
            authentication.Setup(a => a.CurrentUserId).Returns(1);

            return authentication.Object;
        }

        ICornerRepository CreateCornerRepository()
        {
            _corners = Builder<Corner>.CreateListOfSize(10)
                .All()
                .With(x => x.CornerVisitors = Builder<CornerVisitor>.CreateListOfSize(3)
                                                .All()
                                                .With(y => y.VisitedOn = DateTime.Now.AddMinutes(-10))
                                                .Build())
                .And(x => x.NumberOfVisitors = 3)
                .Build();

            _cornerRepository = new Mock<ICornerRepository>();

            _cornerRepository.Setup(x => x.Find(It.IsAny<int>())).Returns<int>(id => _corners.FirstOrDefault(c => c.Id == id));
            _cornerRepository.Setup(x => x.All()).Returns(_corners);

            return _cornerRepository.Object;
        }

        void AddCornerVisitor(int cornerId, string ipAddress, DateTime visitedOn)
        {
            var corner = _corners.Single(c => c.Id == 2);

            corner.NumberOfVisitors++;

            corner.CornerVisitors.Add(new CornerVisitor
            {
                IpAddress = ipAddress,
                VisitedOn = visitedOn
            });
        }

        #endregion
    }
}
using System.ComponentModel.Composition;
using RoomReservation.Business.Bootstrapper;
using RoomReservation.Business.Entities;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Room = RoomReservation.Client.Entities.Room;

namespace Core.Common.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();

        }

        [TestMethod]
        public void TestRepositoryMethod()
        {
            var repositoryTestClass = new RepositoryTestClass();
            repositoryTestClass.AddARoom();
       
            Assert.IsTrue(true == true);


        }

    }


    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }


        public RepositoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;


        public void AddARoom()
        {
            var roomRepository = _dataRepositoryFactory.GetDataRepository<IRoomRepository>();

            var room = new Room(){RoomId = 123, Description= "good"};

        //    roomRepository.Add(room);
            roomRepository.Add(room);




        }




    }
}

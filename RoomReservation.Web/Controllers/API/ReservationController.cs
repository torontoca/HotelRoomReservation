using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
//using AttributeRouting.Web.Http;
//using AttributeRouting.Web.Mvc;
using AttributeRouting.Web.Mvc;
using RoomReservation.Client.Contracts;
using RoomReservation.Client.Entities;
using RoomReservation.Web.Core;
using RoomReservation.Web.Models;
using Core.Common.Contracts;

namespace RoomReservation.Web.Controllers.API
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [UsesDisposableService]
    public class ReservationController : ApiControllerBase
    {
        [ImportingConstructor]
        public ReservationController(IInventoryService inventoryService, IRentalService rentalService)
        {
            _inventoryService = inventoryService;
            _rentalService = rentalService;
        }

        IInventoryService _inventoryService;
        IRentalService _rentalService;

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(_inventoryService);
            disposableServices.Add(_rentalService);
        }

        [HttpGet]
        [AllowAnonymous]
        [GET("api/reservation/availablerooms")]
        [ActionName("availablerooms")]
        public HttpResponseMessage GetAvailableRooms(HttpRequestMessage request, DateTime pickupDate, DateTime returnDate)
        {
            return GetHttpResponse(request, () =>
            {
                Room[] rooms = _inventoryService.GetAvailableRooms(pickupDate, returnDate).ToArray();

                return request.CreateResponse<Room[]>(HttpStatusCode.OK, rooms);
            });
        }
        
        [HttpPost]
        [POST("api/reservation/reserveroom")]
        public HttpResponseMessage ReserveRoom(HttpRequestMessage request, [FromBody]ReservationModel reservationModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                string user = User.Identity.Name; // this method is secure to only the authenticated user to reserve
                Reservation reservation = _rentalService.MakeReservation(user, reservationModel.Room, reservationModel.PickupDate, reservationModel.ReturnDate);

                response = request.CreateResponse<Reservation>(HttpStatusCode.OK, reservation);

                return response;
            });
        }

        [HttpGet]
        [GET("api/reservation/getopen")]
        [ActionName("getopen")]
        public HttpResponseMessage GetOpenReservations(HttpRequestMessage request)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                string user = User.Identity.Name; // this method is secure to only the authenticated user to reserve
               
                CustomerReservationData[] reservations = _rentalService.GetCustomerReservations(user).ToArray();

                response = request.CreateResponse<CustomerReservationData[]>(HttpStatusCode.OK, reservations);

                return response;
            });
        }


        [HttpPost]
        [POST("api/reservation/cancel")]
        [ActionName("cancel")]
        public HttpResponseMessage CancelReservation(HttpRequestMessage request, [FromBody]int reservationId)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                // not that calling the WCF service here will authenticate access to the data
                Reservation reservation = _rentalService.GetReservation(reservationId);
                if (reservation != null)
                {
                    _rentalService.CancelReservation(reservationId);

                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "No reservation found under that ID.");

                return response;
            });
        }
    }
}

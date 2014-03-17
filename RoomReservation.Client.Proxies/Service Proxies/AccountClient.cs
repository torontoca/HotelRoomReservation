using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Client.Contracts;
using Core.Common.ServiceModel;

namespace RoomReservation.Client.Proxies
{
    [Export(typeof(IAccountService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountClient : UserClientBase<IAccountService>, IAccountService
    {
        public Entities.Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCustomerAccountInfo(loginEmail));
        }

        public void UpdateCustomerAccountInfo(Entities.Account account)
        {
            ExecuteFaultHandledOperation(() => Channel.UpdateCustomerAccountInfo(account));
        }

        public Task<Entities.Account> GetCustomerAccountInfoAsync(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCustomerAccountInfoAsync(loginEmail));
        }

        public Task UpdateCustomerAccountInfoAsync(Entities.Account account)
        {
            return ExecuteFaultHandledOperation(() => Channel.UpdateCustomerAccountInfoAsync(account));
        }
    }
}

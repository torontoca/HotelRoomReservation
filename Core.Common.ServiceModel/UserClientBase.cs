using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Contracts;

namespace Core.Common.ServiceModel
{
    public class UserClientBase<T> : ClientBase<T>
        where T : class, IServiceContract
    {
        protected UserClientBase(): base()
        {

            //OperationContextScope contextScope = new OperationContextScope(InnerChannel);

            //string userName = Thread.CurrentPrincipal.Identity.Name;

            //MessageHeader<string> header = new MessageHeader<string>(userName);

            //OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
            
        }


        protected U ExecuteFaultHandledOperation<U>(Func<U> codeToExecute)
        {
            try
            {
                using (new OperationContextScope(InnerChannel))
                {
                    string userName = Thread.CurrentPrincipal.Identity.Name;

                    MessageHeader<string> header = new MessageHeader<string>(userName);

                    OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));

                    return codeToExecute.Invoke();
                }
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

        }


        protected void ExecuteFaultHandledOperation(Action codeToExecute)
        {
            try
            {
                using (new OperationContextScope(InnerChannel))
                {
                    string userName = Thread.CurrentPrincipal.Identity.Name;

                    MessageHeader<string> header = new MessageHeader<string>(userName);

                    OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));

                    codeToExecute.Invoke();
                }
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

        }
    }
}

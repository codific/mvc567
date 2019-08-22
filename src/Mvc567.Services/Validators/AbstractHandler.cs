// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
// Copyright (C) 2019 Georgi Karagogov
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;

namespace Mvc567.Services.Validators
{
    internal abstract class AbstractHandler<T> : IHandler<T> where T : class
    {
        private IHandler<T> nextHandler;
        protected T requestObject;

        /// <summary>
        /// Receive object and validation message. If current validation pass successfully continue with the validation chain. If validation fail - return null.
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="validationResultMessage"></param>
        /// <returns></returns>
        public virtual T Handle(T requestObject, out string validationResultMessage)
        {
            this.requestObject = requestObject;
            validationResultMessage = HandleProcessAction();

            T returnObject = this.requestObject;
            if (returnObject != null && this.nextHandler != null)
            {
                string newResultMessage = string.Empty;
                returnObject = this.nextHandler.Handle(requestObject, out newResultMessage);

                validationResultMessage += newResultMessage;
            }

            return returnObject;
        }

        /// <summary>
        /// Set next validation handler to validation chain.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public IHandler<T> SetNext(IHandler<T> handler)
        {
            this.nextHandler = handler;

            return handler;
        }

        /// <summary>
        /// Method that contains current validation logic for current handler. If validation pass successfully return empty string as a message, if not - return validation message and set handler object to null.
        /// </summary>
        /// <returns></returns>
        protected virtual string HandleProcessAction()
        {
            throw new NotImplementedException("Handler has no process action implementation.");
        }
    }
}

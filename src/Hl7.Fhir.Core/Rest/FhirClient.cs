﻿/* 
 * Copyright (c) 2014, Firely (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/fhir-net-api/master/LICENSE
 */


using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Hl7.Fhir.Rest
{
    public partial class FhirClient : BaseFhirClient, IFhirClient
    {
        #region BaseFhirClient

        //private Requester _requester;

        ///// <summary>
        ///// The last transaction result that was executed on this connection to the FHIR server
        ///// </summary>
        //public Bundle.ResponseComponent LastResult => _requester.LastResult?.Response;
        //public byte[] LastBody => LastResult?.GetBody();
        //public string LastBodyAsText => LastResult?.GetBodyAsText();
        //public Resource LastBodyAsResource => _requester.LastResult?.Resource;

        ///// <summary>
        ///// The default endpoint for use with operations that use discrete id/version parameters
        ///// instead of explicit uri endpoints. This will always have a trailing "/"
        ///// </summary>
        //public Uri Endpoint
        //{
        //    get;
        //    private set;
        //}

        #endregion


        /// <summary>
        /// Creates a new client using a default endpoint
        /// If the endpoint does not end with a slash (/), it will be added.
        /// </summary>
        /// <param name="endpoint">
        /// The URL of the server to connect to.<br/>
        /// If the trailing '/' is not present, then it will be appended automatically
        /// </param>
        /// <param name="verifyFhirVersion">
        /// If parameter is set to true the first time a request is made to the server a 
        /// conformance check will be made to check that the FHIR versions are compatible.
        /// When they are not compatible, a FhirException will be thrown.
        /// </param>
        public FhirClient(Uri endpoint, bool verifyFhirVersion = false) : this(endpoint, new FhirClientSettings() { VerifyFhirVersion = verifyFhirVersion })
        {
        }

        public FhirClient(Uri endpoint, FhirClientSettings settings) : base(endpoint, settings)
        {
            Requester = new Requester(Endpoint, base.Settings)
            {
                BeforeRequest = this.BeforeRequest,
                AfterResponse = this.AfterResponse
            };
        }

        /// <summary>
        /// Creates a new client using a default endpoint
        /// If the endpoint does not end with a slash (/), it will be added.
        /// </summary>
        /// <param name="endpoint">
        /// The URL of the server to connect to.<br/>
        /// If the trailing '/' is not present, then it will be appended automatically
        /// </param>
        /// <param name="verifyFhirVersion">
        /// If parameter is set to true the first time a request is made to the server a 
        /// conformance check will be made to check that the FHIR versions are compatible.
        /// When they are not compatible, a FhirException will be thrown.
        /// </param>
        public FhirClient(string endpoint, bool verifyFhirVersion = false) : this(endpoint, new FhirClientSettings() { VerifyFhirVersion = verifyFhirVersion })
        {
        }

        public FhirClient(string endpoint, FhirClientSettings settings) : base(new Uri(endpoint), settings)
        {
            Requester = new Requester(Endpoint, base.Settings)
            {
                BeforeRequest = this.BeforeRequest,
                AfterResponse = this.AfterResponse
            };
        }

        #region << Client Communication Defaults (PreferredFormat, UseFormatParam, Timeout, ReturnFullResource) >>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public bool VerifyFhirVersion
        {
            get => Settings.VerifyFhirVersion;
            set => Settings.VerifyFhirVersion = value;
        }

        /// <summary>
        /// The preferred format of the content to be used when communicating with the FHIR server (XML or JSON)
        /// </summary>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public ResourceFormat PreferredFormat
        {
            get => Settings.PreferredFormat;
            set => Settings.PreferredFormat = value;
        }

        /// <summary>
        /// When passing the content preference, use the _format parameter instead of the request header
        /// </summary>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public bool UseFormatParam
        {
            get => Settings.UseFormatParameter;
            set => Settings.UseFormatParameter = value;
        }

        /// <summary>
        /// The timeout (in milliseconds) to be used when making calls to the FHIR server
        /// </summary>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public int Timeout
        {
            get => Settings.Timeout;
            set => Settings.Timeout = value;
        }


        /// <summary>
        /// Should calls to Create, Update and transaction operations return the whole updated content?
        /// </summary>
        /// <remarks>Refer to specification section 2.1.0.5 (Managing Return Content)</remarks>
        [Obsolete("In STU3 this is no longer a true/false option, use the PreferredReturn property instead")]
        public bool ReturnFullResource
        {
            get => Settings.PreferredReturn == Prefer.ReturnRepresentation;
            set => Settings.PreferredReturn = value ? Prefer.ReturnRepresentation : Prefer.ReturnMinimal;
        }

        /// <summary>
        /// Should calls to Create, Update and transaction operations return the whole updated content, 
        /// or an OperationOutcome?
        /// </summary>
        /// <remarks>Refer to specification section 2.1.0.5 (Managing Return Content)</remarks>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public Prefer? PreferredReturn
        {
            get => Settings.PreferredReturn;
            set => Settings.PreferredReturn = value;
        }

        /// <summary>
        /// Should server return which search parameters were supported after executing a search?
        /// If true, the server should return an error for any unknown or unsupported parameter, otherwise
        /// the server may ignore any unknown or unsupported parameter.
        /// </summary>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public SearchParameterHandling? PreferredParameterHandling
        {
            get => Settings.PreferredParameterHandling;
            set => Settings.PreferredParameterHandling = value;
        }


#if NET_COMPRESSION
        /// <summary>
        /// This will do 2 things:
        /// 1. Add the header Accept-Encoding: gzip, deflate
        /// 2. decompress any responses that have Content-Encoding: gzip (or deflate)
        /// </summary>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public bool PreferCompressedResponses
        {
            get => Settings.PreferCompressedResponses;
            set => Settings.PreferCompressedResponses = value;
        }
        /// <summary>
        /// Compress any Request bodies 
        /// (warning, if a server does not handle compressed requests you will get a 415 response)
        /// </summary>
        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public bool CompressRequestBody
        {
            get => Settings.CompressRequestBody;
            set => Settings.CompressRequestBody = value;
        }
#endif

        [Obsolete("Use the FhirClient.Settings property or the settings argument in the constructor instead")]
        public ParserSettings ParserSettings
        {
            get => Settings.ParserSettings;
            set => Settings.ParserSettings = value;
        }
        
        /// <summary>
        /// Returns the HttpWebRequest as it was last constructed to execute a call on the FhirClient
        /// </summary>
        public HttpWebRequest LastRequest { get { return (Requester as Requester)?.LastRequest; } }

        /// <summary>
        /// Returns the HttpWebResponse as it was last received during a call on the FhirClient
        /// </summary>
        /// <remarks>Note that the FhirClient will have read the body data from the HttpWebResponse, so this is
        /// no longer available. Use LastBody, LastBodyAsText and LastBodyAsResource to get access to the received body (if any)</remarks>
        public HttpWebResponse LastResponse { get { return (Requester as Requester)?.LastResponse; } }
        
        #endregion
        

        /// <summary>
        /// Called just before the Http call is done
        /// </summary>
        public event EventHandler<BeforeRequestEventArgs> OnBeforeRequest;

        /// <summary>
        /// Called just after the response was received
        /// </summary>
        public event EventHandler<AfterResponseEventArgs> OnAfterResponse;

        /// <summary>
        /// Inspect or modify the HttpWebRequest just before the FhirClient issues a call to the server
        /// </summary>
        /// <param name="rawRequest">The request as it is about to be sent to the server</param>
        /// <param name="body">The data in the body of the request as it is about to be sent to the server</param>
        protected virtual void BeforeRequest(HttpWebRequest rawRequest, byte[] body) 
        {
            // Default implementation: call event
            OnBeforeRequest?.Invoke(this, new BeforeRequestEventArgs(rawRequest, body));
        }

        /// <summary>
        /// Inspect the HttpWebResponse as it came back from the server
        /// </summary>
        /// <remarks>You cannot read the body from the HttpWebResponse, since it has
        /// already been read by the framework. Use the body parameter instead.</remarks>
        protected virtual void AfterResponse(HttpWebResponse webResponse, byte[] body)
        {
            // Default implementation: call event
            OnAfterResponse?.Invoke(this, new AfterResponseEventArgs(webResponse, body));
        }
    }
    
    public class BeforeRequestEventArgs : EventArgs
    {
        public BeforeRequestEventArgs(HttpWebRequest rawRequest, byte[] body)
        {
            this.RawRequest = rawRequest;
            this.Body = body;
        }

        public HttpWebRequest RawRequest { get; internal set; }
        public byte[] Body { get; internal set; }
    }

    public class AfterResponseEventArgs : EventArgs
    {
        public AfterResponseEventArgs(HttpWebResponse webResponse, byte[] body)
        {
            this.RawResponse = webResponse;
            this.Body = body;
        }

        public HttpWebResponse RawResponse { get; internal set; }
        public byte[] Body { get; internal set; }
    }
}
